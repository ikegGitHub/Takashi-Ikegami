using System;
using System.Collections.Generic;
using System.Linq;

namespace XFlag.Alter3Simulator
{
    public class CommandProcessor : ICommandVisitor<IEnumerable<string>>
    {
        private CoreSystem _coreSystem;
        private CommandParser _commandParser = new CommandParser();
        private RequestContext _requestContext;

        public CommandProcessor(CoreSystem coreSystem, RequestContext requestContext)
        {
            _coreSystem = coreSystem;
            _requestContext = requestContext;
        }

        public void ProcessCommand()
        {
            try
            {
                var command = _commandParser.ParseCommandLine(_requestContext.ReceivedString);
                foreach (var responseLine in command.AcceptVisitor(this))
                {
                    _requestContext.AppendResponseLine(responseLine);
                }
            }
            catch (Exception e)
            {
                _requestContext.AppendResponseLine(Response.MakeErrorResponse(e.Message));
            }
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
            yield return _coreSystem.IsRecording ? StatusText.Recording : StatusText.NotRecording;
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
            _coreSystem.SetClientName(_requestContext.ClientId, command.ClientName);
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(QuitCommand command)
        {
            _requestContext.IsClose = true;
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(NoopCommand command)
        {
            yield return Response.OK;
        }

        IEnumerable<string> ICommandVisitor<IEnumerable<string>>.Visit(IsConnectedCommand command)
        {
            yield return _coreSystem.IsRobotConnected ? StatusText.Connected : StatusText.NotConnected;
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
            yield return _coreSystem.GetClient(_requestContext.ClientId).ToString();
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
                yield return _coreSystem.Axes.Aggregate("", (a, b) => $"{a} {b}");
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
