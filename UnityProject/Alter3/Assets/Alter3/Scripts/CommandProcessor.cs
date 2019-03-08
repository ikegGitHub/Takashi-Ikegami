using System;
using System.Text;
using XFlag.Alter3Simulator.Network;

namespace XFlag.Alter3Simulator
{
    public class CommandProcessor
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
            try
            {
                var command = _commandParser.ParseCommandLine(_requestContext.ReceivedString);
                _requestContext.ResponseWriter.WriteLine(Response.OK);
            }
            catch (Exception e)
            {
                _requestContext.ResponseWriter.WriteLine($"ERROR: {e.Message}");
            }
        }

        private void ProcessCommand(ICommand originalCommand)
        {
            switch (originalCommand)
            {
                case RecordMotionCommand command:
                    {
                        _coreSystem.IsRecording = !command.IsStop;
                    }
                    break;
                case IsRecordingMotionCommand _:
                    {
                        _requestContext.ResponseWriter.WriteLine(_coreSystem.IsRecording ? StatusText.Recording : StatusText.NotRecording);
                    }
                    break;
                case RobotInfoCommand _:
                    {
                        _requestContext.ResponseWriter.WriteLine("DummyRobot 0 0");
                    }
                    break;
                case HelloCommand command:
                    {
                        _coreSystem.SetClientName(_requestContext.ClientId, command.ClientName);
                    }
                    break;
                case QuitCommand _:
                    {
                        _requestContext.IsClose = true;
                    }
                    break;
                case IsConnectedCommand _:
                    {
                        _requestContext.ResponseWriter.WriteLine(_coreSystem.IsRobotConnected ? StatusText.Connected : StatusText.NotConnected);
                    }
                    break;
                case ConnectRobotCommand _:
                    {
                        _coreSystem.IsRobotConnected = true;
                    }
                    break;
                case DisconnectRobotCommand _:
                    {
                        _coreSystem.IsRobotConnected = false;
                    }
                    break;
                case WhoAmICommand _:
                    {
                        _requestContext.ResponseWriter.WriteLine(_coreSystem.GetClient(_requestContext.ClientId).ToString());
                    }
                    break;
                case ClientsInfoCommand _:
                    foreach (var client in _coreSystem.Clients)
                    {
                        _requestContext.ResponseWriter.WriteLine(client.ToString());
                    }
                    break;
                case GetAxisCommand command:
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
                    break;
                case MoveAxisCommand command:
                    {
                        _coreSystem.Robot.MoveAxis(command.Param);
                    }
                    break;
                case MoveAxesCommand command:
                    {
                        _coreSystem.Robot.MoveAxes(command.Params);
                    }
                    break;
                case GetPositionsCommand _:
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
                    break;
            }
        }
    }
}
