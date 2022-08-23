namespace HashGrinder.HashRootFinders
{
    internal interface IHashRootFinder
    {
        byte[]? FindRoot(int length, byte[] reference);
    }
}
