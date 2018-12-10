using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFlag.Alter3Simulator;

public class Test : MonoBehaviour
{
    class SampleVisitor : CommandVisitorBase
    {
        protected internal override void Visit(AddAxisCommand command)
        {
            Debug.Log($"AddAxis: {command}");
        }

        protected internal override void Visit(MoveAxesCommand command)
        {
            Debug.Log($"MoveAxes: {command}");
        }

        protected internal override void Visit(PrintQueueCommand command)
        {
            Debug.Log("PrintQueue!");
        }
    }

    [SerializeField]
    private TextAsset _exampleSource = null;

    private TcpListener _listener;

    private List<TcpClient> _clients = new List<TcpClient>();

    private void Awake()
    {
        var parser = new CommandParser();
        var visitor = new SampleVisitor();
        using (var reader = new StringReader(_exampleSource.text))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                try
                {
                    var command = parser.ParseCommandLine(line);
                    command.AcceptVisitor(visitor);
                }
                catch (ApplicationException e)
                {
                    Debug.Log($"Parse Error: {e.Message}");
                }
            }
        }
    }

    private void OnDestroy()
    {
        StopListen();
    }

    [ContextMenu("Start Listen")]
    private async Task StartListen()
    {
        if (_listener == null)
        {
            await Listen();
        }
    }

    [ContextMenu("Stop Listen")]
    private void StopListen()
    {
        _listener?.Stop();
    }

    private async Task Listen()
    {
        Debug.Log("Start Listen.");
        _listener = new TcpListener(new IPEndPoint(IPAddress.Any, 3000));
        _listener.Start();

        while (true)
        {
            try
            {
                var client = await _listener.AcceptTcpClientAsync();
                _clients.Add(client);
                Debug.Log("Client Connected.");
                Task.Run(() => ClientSequence(client));
            }
            catch (ObjectDisposedException)
            {
                Debug.Log("server stopped");
                break;
            }
        }
        _listener = null;

        lock (_clients)
        {
            foreach (var client in _clients)
            {
                client.Close();
                client.Dispose();
            }
        }
    }

    private void ClientSequence(TcpClient client)
    {
        try
        {
            using (var reader = new StreamReader(client.GetStream(), Encoding.UTF8))
            using (var writer = new StreamWriter(client.GetStream(), Encoding.UTF8))
            {
                while (client.Connected)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                    {
                        Debug.Log("Disconnected");
                        break;
                    }
                    Debug.Log($"Received: {line}");
                    var parser = new CommandParser();
                    try
                    {
                        var command = parser.ParseCommandLine(line);
                        Debug.Log("Respond: OK");
                        writer.WriteLine("OK");
                        writer.Flush();
                        if (command is QuitCommand)
                        {
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log($"Respond: ERROR: {e.Message}");
                        writer.WriteLine($"ERROR: {e.Message}");
                        writer.Flush();
                    }
                }
            }
        }
        finally
        {
            client.Close();
            client.Dispose();
            lock (_clients)
            {
                _clients.Remove(client);
            }
            Debug.Log("client end");
        }
    }
}
