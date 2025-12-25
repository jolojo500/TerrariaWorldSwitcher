using System.Net.Sockets;

public static class WorldReceiver
{
    public static string ReceiveWorld(string hostIp)
    {
        AppContext.SetState(AppState.Receiving);

        Directory.CreateDirectory(Paths.Staging);

        string receivedZipPath = Path.Combine(
            Paths.Staging,
            $"received_{DateTime.Now:yyyyMMdd_HHmmss}.zip"
        );

        using var client = new TcpClient();
        client.Connect(hostIp, TransferProtocol.Port);

        using var networkStream = client.GetStream(); //using basically makes the stuff handled and closed on end
        using var fileStream = File.Create(receivedZipPath);

        byte[] buffer = new byte[TransferProtocol.BufferSize];
        int bytesRead;

        Console.WriteLine("Receiving world...");

        while ((bytesRead = networkStream.Read(buffer, 0, buffer.Length)) > 0)
        {
            fileStream.Write(buffer, 0, bytesRead); //reading whats in the network and writing to file
        }

        Console.WriteLine("World received successfully.");

        AppContext.SetState(AppState.Done);
        return receivedZipPath;
    }
}
