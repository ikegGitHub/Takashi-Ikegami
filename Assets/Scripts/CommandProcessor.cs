using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    public class CommandProcessor : ICommandVisitor<IEnumerable<string>>
    {
        private uint _clientId;

        public CommandProcessor(uint clientId)
        {
            _clientId = clientId;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(HelpCommand command)
        {
            throw new System.NotImplementedException();
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
            Debug.Log($"client name = {command.ClientName}");
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
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
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
