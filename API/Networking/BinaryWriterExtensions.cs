using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            PopulateObjectWProperties(reader, typeof(T), null, shouldSerialize);

        /// <summary>
        /// Strongly typed populate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="state"></param>
        /// <param name="shouldSerialize"></param>
        public static void PopulateObjectWProperties<T>(this BinaryReader reader, object state, Func<MemberInfo, bool> shouldSerialize) =>
            PopulateObjectWProperties(reader, typeof(T), state, shouldSerialize);

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

            public void WriteList(BinaryWriter writer, object[] list)
            {
                writer.Write((ushort)list.Length);
                for (int i = 0; i < list.Length; i++)
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
            foreach (var p in type.GetProperties())
            {
                if (shouldSerialize(p))
                {
                    var handler = handlers.FirstOrDefault(x => x.PayloadType.IsEquivalentTo(type));
                    if (handler != default)
                    {
                        if (type.IsArray)
                        {
                            handler.WriteList(writer, (object[])p.GetValue(state));
                        }
                        else
                        {
                            handler.Write(writer, p.GetValue(state));
                        }
                    }
                    else
                    {
                        // serialize non primitive fields
                        SerializeProperties(writer, type, p.GetValue(state), shouldSerialize);
                    }
                }
            }
        }

        public static void PopulateObjectWProperties(this BinaryReader reader, object state, Func<MemberInfo, bool> shouldSerialize)
        {
            var type = state.GetType();
            foreach (var p in type.GetProperties())
            {
                if (shouldSerialize(p))
                {
                    var handler = handlers.FirstOrDefault(x => x.PayloadType.IsEquivalentTo(type));
                    if (handler != default)
                    {
                        if (type.IsArray)
                        {
                            p.SetValue(state, handler.ReadList(reader));
                        }
                        else
                        {
                            p.SetValue(state, handler.Read(reader));
                        }
                    }
                    else
                    {
                        // serialize non primitive fields
                        PopulateObjectWProperties(reader, type, p.GetValue(state), shouldSerialize);
                    }
                }
            }
        }
    }
}