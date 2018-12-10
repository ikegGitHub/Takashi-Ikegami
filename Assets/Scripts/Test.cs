using System;
using System.Threading.Tasks;
using UnityEngine;
using XFlag.Alter3Simulator;

public class Test : MonoBehaviour
{
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

    private void OnReceived(RequestContext context)
    {
        var parser = new CommandParser();
        var processor = new CommandProcessor(context.ClientId);
        try
        {
            var command = parser.ParseCommandLine(context.ReceivedString);
            foreach (var responseLine in command.AcceptVisitor(processor))
            {
                Debug.Log($"[{context.ClientId}] {responseLine}");
                context.ResponseWriter.WriteLine(responseLine);
            }
            context.ResponseWriter.Flush();
            if (command is QuitCommand)
            {
                context.IsClose = true;
            }
        }
        catch (Exception e)
        {
            Debug.Log($"[{context.ClientId}] respond: ERROR: {e.Message}");
            context.ResponseWriter.WriteLine($"ERROR: {e.Message}");
            context.ResponseWriter.Flush();
        }
    }
}
