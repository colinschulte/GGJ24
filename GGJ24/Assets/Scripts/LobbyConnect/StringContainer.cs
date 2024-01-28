using Unity.Netcode;

public class StringContainer : INetworkSerializable
{
    public string containedString;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (serializer.IsWriter)
            serializer.GetFastBufferWriter().WriteValueSafe(containedString);
        else
            serializer.GetFastBufferReader().ReadValueSafe(out containedString);
    }
}