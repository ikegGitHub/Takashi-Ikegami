using System.Collections.Generic;
using System.Linq;

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
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(RecordMotionCommand command)
        {
            _coreSystem.IsRecording = !command.IsStop;
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(IsRecordingMotionCommand command)
        {
            yield return _coreSystem.IsRecording ? "RECORDING" : "NOT RECORDING";
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(RobotInfoCommand command)
        {
            yield return "DummyRobot 0 0";
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(ResetPoseCommand command)
        {
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(HelloCommand command)
        {
            _coreSystem.SetClientName(_clientId, command.ClientName);
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(QuitCommand command)
        {
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(NoopCommand command)
        {
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(IsConnectedCommand command)
        {
            yield return _coreSystem.IsRobotConnected ? "CONNECTED" : "NOT CONNECTED";
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(ConnectRobotCommand command)
        {
            _coreSystem.IsRobotConnected = true;
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(DisconnectRobotCommand command)
        {
            _coreSystem.IsRobotConnected = false;
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(PrintQueueCommand command)
        {
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(ClearQueueCommand command)
        {
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(WhoAmICommand command)
        {
            yield return _coreSystem.GetClient(_clientId).ToString();
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(ClientsInfoCommand command)
        {
            foreach (var client in _coreSystem.Clients)
            {
                yield return client.ToString();
            }
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(GetAxisCommand command)
        {
            if (command.AxisNumber == 0)
            {
                yield return _coreSystem.Axes.Select(x => x.ToString()).Aggregate((a, b) => $"{a} {b}");
            }
            else
            {
                var axisValue = _coreSystem.GetAxis(command.AxisNumber);
                yield return axisValue.ToString();
            }
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(AppendAxisCommand command)
        {
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(AddAxisCommand command)
        {
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(MoveAxisCommand command)
        {
            if (!_coreSystem.IsRobotConnected)
            {
                yield return Response.MakeErrorResponse("Not connected to robot");
                yield break;
            }

            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(MoveAxesCommand command)
        {
            if (!_coreSystem.IsRobotConnected)
            {
                yield return Response.MakeErrorResponse("Not connected to robot");
                yield break;
            }

            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(PlayMotionCommand command)
        {
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(CalibCommand command)
        {
            yield return Response.OK;
        }
    }
}
