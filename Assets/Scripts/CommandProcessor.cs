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
            throw new System.NotImplementedException();
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(IsRecordingMotionCommand command)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(AppendAxisCommand command)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(RobotInfoCommand command)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(ResetPoseCommand command)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(HelloCommand command)
        {
            _coreSystem.SetClientName(_clientId, command.ClientName);
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(GetAxisCommand command)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(QuitCommand command)
        {
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(NoopCommand command)
        {
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(MoveAxisCommand command)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(IsConnectedCommand command)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(PrintQueueCommand command)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(ClearQueueCommand command)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(WhoAmICommand command)
        {
            yield return _coreSystem.GetClient(_clientId).ToString();
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(ConnectRobotCommand command)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(DisconnectRobotCommand command)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(ClientsInfoCommand command)
        {
            foreach (var client in _coreSystem.Clients)
            {
                yield return client.ToString();
            }
            yield return "OK";
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(AddAxisCommand command)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(PlayMotionCommand command)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(CalibCommand command)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(MoveAxesCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}
