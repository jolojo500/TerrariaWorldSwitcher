using System.Net.Sockets;

public static class WorldReceiver
{
    public static string ReceiveWorld(string hostIp, Action<long, long>? onProgress = null)
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

        //lire la taille du fichier
        byte[] sizeBuffer = new byte[8];
        networkStream.Read(sizeBuffer,0,8); //TODO maybe helper func that has looping because currently reads until 8 bytes. Means can be less we never know but tcp should handle so idk why warning
        long totalBytes = BitConverter.ToInt64(sizeBuffer, 0);

        long receivedBytes = 0;        
        byte[] buffer = new byte[TransferProtocol.BufferSize];

        Console.WriteLine("Receiving world...");

        while ( receivedBytes < totalBytes )
        {
            int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
            if(bytesRead == 0) break;
            
            fileStream.Write(buffer, 0, bytesRead); //reading whats in the network and writing to file

            receivedBytes += bytesRead;
            onProgress?.Invoke(receivedBytes, totalBytes);
        }

        Console.WriteLine("World received successfully.");

        AppContext.SetState(AppState.Done);
        return receivedZipPath;
    }
}
