using XFlag.Alter3Simulator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;
using System.Text;

namespace Sandbox
{
    public interface IQueryState { }
    public class Initial : IQueryState { private Initial() { } }
    public class Secondary : IQueryState { private Secondary() { } }
    public class Final : IQueryState { private Final() { } }

    class Query<T> where T : IQueryState
    {
        public override string ToString()
        {
            return $"Query ({typeof(T).Name})";
        }
    }

    static class QueryExtension
    {
        public static Query<Secondary> Next(this Query<Initial> query)
        {
            return new Query<Secondary>();
        }

        public static Query<Final> Final(this Query<Secondary> query)
        {
            return new Query<Final>();
        }
    }

    public class RateValue { }
    public class PercentValue { }

    public class Value<T>
    {
        public float value;

        public static implicit operator float(Value<T> value)
        {
            return value.value;
        }
    }

    public static class ValueExtension
    {
        public static Value<PercentValue> ToPercent(this Value<RateValue> rate)
        {
            return new Value<PercentValue> { value = rate * 100.0f };
        }

        public static Value<RateValue> ToRate(this Value<PercentValue> percent)
        {
            return new Value<RateValue> { value = percent / 100.0f };
        }
    }
}

public class Test : MonoBehaviour
{
    class SampleVisitor : CommandVisitorBase
    {
        public override void Visit(AddAxisCommand command)
        {
            Debug.Log($"AddAxis: {command}");
        }

        public override void Visit(MoveAxesCommand command)
        {
            Debug.Log($"MoveAxes: {command}");
        }

        public override void Visit(PrintQueueCommand command)
        {
            Debug.Log("PrintQueue!");
        }
    }

    [SerializeField]
    private TextAsset _exampleSource = null;

    private TcpListener _listener;

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

        TcpClient client;
        try
        {
            client = await _listener.AcceptTcpClientAsync();
            _listener.Stop();
        }
        catch (ObjectDisposedException e)
        {
            Debug.Log("stop server");
            _listener = null;
            return;
        }
        _listener = null;

        Debug.Log("Client Connected.");
        try
        {
            using (var reader = new StreamReader(client.GetStream(), Encoding.UTF8))
            using (var writer = new StreamWriter(client.GetStream(), Encoding.UTF8))
            {
                while (client.Connected)
                {
                    var line = await reader.ReadLineAsync();
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
                        if (command is QuitCommand)
                        {
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log($"Respond: ERROR {e.Message}");
                        writer.WriteLine($"ERROR {e.Message}");
                    }
                }
            }
        }
        finally
        {
            client?.Close();
            Debug.Log("Listen end.");
        }
    }
}
