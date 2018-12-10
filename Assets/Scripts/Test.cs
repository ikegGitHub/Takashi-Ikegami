using System;
using System.IO;
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

    private static readonly Encoding Encoding = new UTF8Encoding(false, false);

    [SerializeField]
    private TextAsset _exampleSource = null;

    private ConnectionManager _server = new ConnectionManager();

    private void Awake()
    {
        _server.OnReceived += OnReceived;
    }

    private void OnDestroy()
    {
        StopListen();
    }

    [ContextMenu("Start Listen")]
    private async Task StartListen()
    {
        await _server.StartServerAsync("0.0.0.0", 3000);
    }

    [ContextMenu("Stop Listen")]
    private void StopListen()
    {
        _server.StopServer();
    }

    private bool OnReceived(uint clientId, string line, StreamWriter writer)
    {
        var parser = new CommandParser();
        try
        {
            var command = parser.ParseCommandLine(line);
            Debug.Log($"[{clientId}] respond: OK");
            writer.WriteLine("OK");
            writer.Flush();
            if (command is QuitCommand)
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.Log($"[{clientId}] respond: ERROR: {e.Message}");
            writer.WriteLine($"ERROR: {e.Message}");
            writer.Flush();
        }
        return true;
    }
}
