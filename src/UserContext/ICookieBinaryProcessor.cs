namespace UserContext
{
    public interface ICookieBinaryProcessor
    {
        byte[] Write(string value);
        string Read(byte[] objectArray);
    }
}
