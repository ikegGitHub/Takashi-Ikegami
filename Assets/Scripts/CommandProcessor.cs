using System.Collections.Generic;

namespace XFlag.Alter3Simulator
{
    public class CommandProcessor : ICommandVisitor<IEnumerable<string>>
    {
        private CoreSystem _coreSystem;
        private uint _clientId;

        public CommandProcessor(CoreSystem coreSystem, uint clientId)
        {
            _coreSystem = coreSystem;
            _clientId = clientId;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(HelpCommand command)
        {
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(RecordMotionCommand command)
        {
            _coreSystem.IsRecording = !command.IsStop;
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(IsRecordingMotionCommand command)
        {
            yield return _coreSystem.IsRecording ? "RECORDING" : "NOT RECORDING";
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(RobotInfoCommand command)
        {
            yield return "DummyRobot 0 0";
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(ResetPoseCommand command)
        {
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(HelloCommand command)
        {
            _coreSystem.SetClientName(_clientId, command.ClientName);
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(QuitCommand command)
        {
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(NoopCommand command)
        {
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(IsConnectedCommand command)
        {
            yield return _coreSystem.IsRobotConnected ? "CONNECTED" : "NOT CONNECTED";
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(ConnectRobotCommand command)
        {
            _coreSystem.IsRobotConnected = true;
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(DisconnectRobotCommand command)
        {
            _coreSystem.IsRobotConnected = false;
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(PrintQueueCommand command)
        {
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(ClearQueueCommand command)
        {
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(WhoAmICommand command)
        {
            yield return _coreSystem.GetClient(_clientId).ToString();
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(ClientsInfoCommand command)
        {
            foreach (var client in _coreSystem.Clients)
            {
                yield return client.ToString();
            }
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(GetAxisCommand command)
        {
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(AppendAxisCommand command)
        {
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(AddAxisCommand command)
        {
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(MoveAxisCommand command)
        {
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(MoveAxesCommand command)
        {
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(PlayMotionCommand command)
        {
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(CalibCommand command)
        {
            yield return "OK";
        }
    }
}
