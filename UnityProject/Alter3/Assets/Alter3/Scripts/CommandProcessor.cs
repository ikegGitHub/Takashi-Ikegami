using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFlag.Alter3Simulator.Network;

namespace XFlag.Alter3Simulator
{
    public class CommandProcessor : CommandVisitorBase
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
            MakeResponse();
        }

        private void MakeResponse()
        {
            try
            {
                _commandParser.ParseCommandLine(_requestContext.ReceivedString).AcceptVisitor(this);
                _requestContext.ResponseWriter.WriteLine(Response.OK);
            }
            catch (Exception e)
            {
                _requestContext.ResponseWriter.WriteLine($"ERROR: {e.Message}");
            }
        }

        public override void Visit(RecordMotionCommand command)
        {
            _coreSystem.IsRecording = !command.IsStop;
        }

        public override void Visit(IsRecordingMotionCommand command)
        {
            _requestContext.ResponseWriter.WriteLine(_coreSystem.IsRecording ? StatusText.Recording : StatusText.NotRecording);
        }

        public override void Visit(RobotInfoCommand command)
        {
            _requestContext.ResponseWriter.WriteLine("DummyRobot 0 0");
        }

        public override void Visit(ResetPoseCommand command)
        {
        }

        public override void Visit(HelloCommand command)
        {
            _coreSystem.SetClientName(_requestContext.ClientId, command.ClientName);
        }

        public override void Visit(QuitCommand command)
        {
            _requestContext.IsClose = true;
        }

        public override void Visit(IsConnectedCommand command)
        {
            _requestContext.ResponseWriter.WriteLine(_coreSystem.IsRobotConnected ? StatusText.Connected : StatusText.NotConnected);
        }

        public override void Visit(ConnectRobotCommand command)
        {
            _coreSystem.IsRobotConnected = true;
        }

        public override void Visit(DisconnectRobotCommand command)
        {
            _coreSystem.IsRobotConnected = false;
        }

        public override void Visit(PrintQueueCommand command)
        {
        }

        public override void Visit(ClearQueueCommand command)
        {
        }

        public override void Visit(WhoAmICommand command)
        {
            _requestContext.ResponseWriter.WriteLine(_coreSystem.GetClient(_requestContext.ClientId).ToString());
        }

        public override void Visit(ClientsInfoCommand command)
        {
            foreach (var client in _coreSystem.Clients)
            {
                _requestContext.ResponseWriter.WriteLine(client.ToString());
            }
        }

        public override void Visit(GetAxisCommand command)
        {
            if (command.AxisNumber == 0)
            {
                var buf = new StringBuilder();
                for (int i = 0; i < _coreSystem.Robot.AxisCount; ++i)
                {
                    if (i != 0)
                    {
                        buf.Append(' ');
                    }
                    buf.Append(_coreSystem.Robot.GetAxis(i + 1));
                }
                _requestContext.ResponseWriter.WriteLine(buf.ToString());
            }
            else
            {
                var axisValue = _coreSystem.Robot.GetAxis(command.AxisNumber);
                _requestContext.ResponseWriter.WriteLine(axisValue.ToString());
            }
        }

        public override void Visit(AppendAxisCommand command)
        {
        }

        public override void Visit(AddAxisCommand command)
        {
        }

        public override void Visit(MoveAxisCommand command)
        {
            _coreSystem.Robot.MoveAxis(command.Param);
        }

        public override void Visit(MoveAxesCommand command)
        {
            _coreSystem.Robot.MoveAxes(command.Params);
        }

        public override void Visit(PlayMotionCommand command)
        {
        }

        public override void Visit(GetPositionsCommand command)
        {
            var positions = _coreSystem.Robot.GetHandsPositionArray();
            var first = true;
            foreach (var position in positions)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    _requestContext.ResponseWriter.Write(' ');
                }

                _requestContext.ResponseWriter.Write(position.x);
                _requestContext.ResponseWriter.Write(' ');
                _requestContext.ResponseWriter.Write(position.y);
                _requestContext.ResponseWriter.Write(' ');
                _requestContext.ResponseWriter.Write(position.z);
            }
            _requestContext.ResponseWriter.WriteLine();
        }

        protected internal override void Default(ICommand command)
        {
        }
    }
}
