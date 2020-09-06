namespace API
{
    public interface IHaveState
    {
        T StaticGet<T>() where T : IHaveState;
    }
}
