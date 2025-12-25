using System.Net;
using System.Net.Sockets;

public static class WorldSender
{
    public static void SendWorld(string zipPath)
    {
        AppContext.SetState(AppState.Sending);

        var listener = new TcpListener(IPAddress.Any, TransferProtocol.Port);
        listener.Start();

        Console.WriteLine("Waiting for client to connect...");

        using var client = listener.AcceptTcpClient();
        using var networkStream = client.GetStream();//using basically makes the stuff handled and closed on end
        using var fileStream = File.OpenRead(zipPath);

        Console.WriteLine("Client connected. Sending world...");

        byte[] buffer = new byte[TransferProtocol.BufferSize];
        int bytesRead;

        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
        {
            networkStream.Write(buffer, 0, bytesRead); //reading whats in the file and writing to network
        }

        Console.WriteLine("World sent successfully.");

        listener.Stop();
        AppContext.SetState(AppState.Done);
    }
}
