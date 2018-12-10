using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    public class CommandProcessor : ICommandVisitor<IEnumerable<string>>
    {
        private uint _clientId;
        private IPEndPoint _endPoint;
        private string _clientName;

        public CommandProcessor(uint clientId, IPEndPoint endPoint)
        {
            _clientId = clientId;
            _endPoint = endPoint;
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
            _clientName = command.ClientName;
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
            yield return $"{_clientName} {_clientId}@{_endPoint.Address} {_endPoint.Address} User";
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
