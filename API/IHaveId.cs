using Terraria.ModLoader;

namespace API
{
    public interface IHaveId : ILoadable
    {
        int MyId { get; set; }
    }
}
