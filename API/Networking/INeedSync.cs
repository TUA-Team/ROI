namespace API.Networking
{
    public interface INeedSync
    {
        int Identifier { get; }

        INeedSync Identify(int identity);
    }
}
