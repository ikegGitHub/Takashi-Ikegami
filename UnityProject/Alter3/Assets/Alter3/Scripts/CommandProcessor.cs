using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFlag.Alter3Simulator.Network;

namespace XFlag.Alter3Simulator
{
    public class CommandProcessor : CommandVisitorBase<IEnumerable<string>>
    {
        private static readonly CommandParser _commandParser = new CommandParser();

        private CoreSystem _coreSystem;
        private RequestContext _requestContext;

        public CommandProcessor(CoreSystem coreSystem, RequestContext requestContext)
        {
            _coreSystem = coreSystem;
            _requestContext = requestContext;
        }

        public void ProcessCommand()
        {
            _requestContext.ResponseLines = MakeResponse();
        }

        private string[] MakeResponse()
        {
            try
            {
                return _commandParser.ParseCommandLine(_requestContext.ReceivedString).AcceptVisitor(this).ToArray();
            }
            catch (Exception e)
            {
                return Response.MakeErrorResponse(e.Message);
            }
        }

        public override IEnumerable<string> Visit(RecordMotionCommand command)
        {
            _coreSystem.IsRecording = !command.IsStop;
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(IsRecordingMotionCommand command)
        {
            yield return _coreSystem.IsRecording ? StatusText.Recording : StatusText.NotRecording;
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(RobotInfoCommand command)
        {
            yield return "DummyRobot 0 0";
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(ResetPoseCommand command)
        {
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(HelloCommand command)
        {
            _coreSystem.SetClientName(_requestContext.ClientId, command.ClientName);
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(QuitCommand command)
        {
            _requestContext.IsClose = true;
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(IsConnectedCommand command)
        {
            yield return _coreSystem.IsRobotConnected ? StatusText.Connected : StatusText.NotConnected;
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(ConnectRobotCommand command)
        {
            _coreSystem.IsRobotConnected = true;
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(DisconnectRobotCommand command)
        {
            _coreSystem.IsRobotConnected = false;
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(PrintQueueCommand command)
        {
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(ClearQueueCommand command)
        {
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(WhoAmICommand command)
        {
            yield return _coreSystem.GetClient(_requestContext.ClientId).ToString();
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(ClientsInfoCommand command)
        {
            foreach (var client in _coreSystem.Clients)
            {
                yield return client.ToString();
            }
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(GetAxisCommand command)
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

        public override IEnumerable<string> Visit(AppendAxisCommand command)
        {
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(AddAxisCommand command)
        {
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(MoveAxisCommand command)
        {
            _coreSystem.Robot.MoveAxis(command.Param);
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(MoveAxesCommand command)
        {
            _coreSystem.Robot.MoveAxes(command.Params);
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(PlayMotionCommand command)
        {
            yield return Response.OK;
        }

        public override IEnumerable<string> Visit(GetPositionsCommand command)
        {
            var positions = _coreSystem.Robot.GetHandsPositionArray();
            var buf = new StringBuilder();
            var first = true;
            foreach (var position in positions)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    buf.Append(' ');
                }

                buf.Append(position.x);
                buf.Append(' ');
                buf.Append(position.y);
                buf.Append(' ');
                buf.Append(position.z);
            }
            yield return buf.ToString();
            yield return Response.OK;
        }

        protected internal override IEnumerable<string> Default(ICommand command)
        {
            yield return Response.OK;
        }
    }
}
