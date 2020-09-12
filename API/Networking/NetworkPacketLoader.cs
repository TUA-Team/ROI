namespace API.Networking
{
    public class NetworkPacketLoader : CollectionLoader<NetworkPacket>
    {
        protected override void OnAdd<TUnique>(TUnique element)
        {
            element.Mod = Mod;
        }

        public TUnique Get<TUnique>() where TUnique : NetworkPacket => (TUnique)this[IdHolder<TUnique>.Id];
    }
}
