using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace API.Networking
{
    public static class BinaryWriterExtensions
    {
        /// <summary>
        /// Static serialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="writer"></param>
        /// <param name="shouldSerialize"></param>
        public static void SerializeProperties<T>(this BinaryWriter writer, Func<MemberInfo, bool> shouldSerialize) =>
            SerializeProperties(writer, typeof(T), null, shouldSerialize);

        /// <summary>
        /// Strongly typed serialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="writer"></param>
        /// <param name="state"></param>
        /// <param name="shouldSerialize"></param>
        public static void SerializeProperties<T>(this BinaryWriter writer, object state, Func<MemberInfo, bool> shouldSerialize) =>
            SerializeProperties(writer, typeof(T), state, shouldSerialize);

        /// <summary>
        /// Static populate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="shouldSerialize"></param>
        public static void PopulateObjectWProperties<T>(this BinaryReader reader, Func<MemberInfo, bool> shouldSerialize) =>
            PopulateObjectWProperties(reader, null);

        /// <summary>
        /// Strongly typed populate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="state"></param>
        /// <param name="shouldSerialize"></param>
        public static void PopulateObjectWProperties<T>(this BinaryReader reader, object state, Func<MemberInfo, bool> shouldSerialize) =>
            PopulateObjectWProperties(reader, state);

        private class PayloadHandler
        {
            public PayloadHandler(Type type, Func<BinaryReader, object> read, Action<BinaryWriter, object> write)
            {
                PayloadType = type;
                Read = read;
                Write = write;
            }

            public Type PayloadType { get; }
            public Func<BinaryReader, object> Read { get; }
            public Action<BinaryWriter, object> Write { get; }

            public object[] ReadList(BinaryReader reader)
            {
                var list = new List<object>();
                var len = reader.ReadUInt16();
                for (int i = 0; i < len; i++)
                {
                    list.Add(Read(reader));
                }
                return list.ToArray();
            }

            public void WriteList(BinaryWriter writer, List<object> list)
            {
                writer.Write((ushort)list.Count);
                for (ushort i = 0; i < list.Count; i++)
                {
                    Write(writer, list[i]);
                }
            }
        }

        private class PayloadHandler<T> : PayloadHandler
        {
            public PayloadHandler(Func<BinaryReader, T> read, Action<BinaryWriter, T> write) :
                base(typeof(T), r => read(r), (w, v) => write(w, (T)v))
            { }
        }

        private static readonly PayloadHandler[] handlers = new PayloadHandler[]
        {
            new PayloadHandler<bool>(r => r.ReadBoolean(), (w, v) => w.Write(v)),
            new PayloadHandler<byte>(r => r.ReadByte(), (w, v) => w.Write(v)),
            new PayloadHandler<sbyte>(r => r.ReadSByte(), (w, v) => w.Write(v)),
            new PayloadHandler<short>(r => r.ReadInt16(), (w, v) => w.Write(v)),
            new PayloadHandler<ushort>(r => r.ReadUInt16(), (w, v) => w.Write(v)),
            new PayloadHandler<int>(r => r.ReadInt32(), (w, v) => w.Write(v)),
            new PayloadHandler<uint>(r => r.ReadUInt32(), (w, v) => w.Write(v)),
            new PayloadHandler<long>(r => r.ReadInt64(), (w, v) => w.Write(v)),
            new PayloadHandler<ulong>(r => r.ReadUInt64(), (w, v) => w.Write(v)),
            new PayloadHandler<float>(r => r.ReadSingle(), (w, v) => w.Write(v)),
            new PayloadHandler<double>(r => r.ReadDouble(), (w, v) => w.Write(v)),
            new PayloadHandler<decimal>(r => r.ReadDecimal(), (w, v) => w.Write(v)),
            new PayloadHandler<char>(r => r.ReadChar(), (w, v) => w.Write(v)),
            new PayloadHandler<string>(r => r.ReadString(), (w, v) => w.Write(v)),
        };

        public static void SerializeProperties(this BinaryWriter writer, Type type, object state, Func<MemberInfo, bool> shouldSerialize)
        {
            var properties = new Dictionary<string, object>();
            getPropertiesOf(type, state, ref properties, null);
            var enumType = typeof(IEnumerable);

            writer.Write((ushort)properties.Count);

            foreach (var p in properties)
            {
                writer.Write(p.Key);

                var valType = p.Value.GetType();
                foreach (var handler in handlers)
                {
                    if (handler.PayloadType.IsEquivalentTo(valType))
                    {
                        if (enumType.IsAssignableFrom(valType))
                        {
                            // because linq apparently doesn't support IEnumerable
                            var arr = (IEnumerable)p.Value;
                            var enumr = arr.GetEnumerator();
                            var list = new List<object>();
                            do
                            {
                                list.Add(enumr.Current);
                            }
                            while (enumr.MoveNext());

                            handler.WriteList(writer, list);
                        }
                        else
                        {
                            handler.Write(writer, p.Value);
                        }
                        break;
                    }
                }
            }

            void getPropertiesOf(Type propertyType, object propertyState, ref Dictionary<string, object> dict, string fieldName)
            {
                foreach (var p in propertyType.GetProperties())
                {
                    if (shouldSerialize(p))
                    {
                        fieldName = fieldName != null ?
                            fieldName + '.' + p.Name
                            : p.Name;

                        if (p.PropertyType.IsPrimitive)
                        {
                            dict.Add(fieldName, p.GetValue(propertyState));
                        }
                        else
                        {
                            getPropertiesOf(p.PropertyType, p.GetValue(propertyState), ref dict, fieldName);
                        }
                    }
                }
            }
        }

        public static void PopulateObjectWProperties(this BinaryReader reader, object state)
        {
            var stateType = state.GetType();
            var enumType = typeof(IEnumerable);

            var count = reader.ReadUInt16();

            for (ushort i = 0; i < count; i++)
            {
                object propState = null;
                Type propType = null;
                PropertyInfo propInfo = null;
                var propertyName = reader.ReadString();
                var split = propertyName.Split('.');

                propInfo = stateType.GetProperty(split[0]);
                propType = propInfo.PropertyType;
                propState = propInfo.GetValue(state);

                for (byte k = 1; k < split.Length; k++)
                {
                    propInfo = propType.GetProperty(split[k]);
                    propType = propInfo.PropertyType;
                    propState = propInfo.GetValue(propState);
                }

                foreach (var handler in handlers)
                {
                    if (handler.PayloadType.IsEquivalentTo(propType))
                    {
                        if (enumType.IsAssignableFrom(propType))
                        {
                            propInfo.SetValue(propState, handler.ReadList(reader));
                        }
                        else
                        {
                            propInfo.SetValue(propState, handler.Read(reader));
                        }
                        break;
                    }
                }
            }
        }
    }
}
