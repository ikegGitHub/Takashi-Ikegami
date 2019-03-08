using System;
using XFlag.Alter3Simulator.Network;

namespace XFlag.Alter3Simulator
{
    public class CommandProcessor
    {
        private static readonly CommandParser _commandParser = new CommandParser();

        private CoreSystem _coreSystem;

        public CommandProcessor(CoreSystem coreSystem)
        {
            _coreSystem = coreSystem;
        }

        public void ProcessCommand(RequestContext requestContext)
        {
            try
            {
                var command = _commandParser.ParseCommandLine(requestContext.ReceivedString);
                ProcessCommand(requestContext, command);
                requestContext.ResponseWriter.WriteLine(Response.OK);
            }
            catch (Exception e)
            {
                requestContext.ResponseWriter.WriteLine($"ERROR: {e.Message.Replace("\r", "<CR>").Replace("\n", "<LF>")}");
            }
        }

        private void ProcessCommand(RequestContext requestContext, ICommand originalCommand)
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
                        requestContext.ResponseWriter.WriteLine(_coreSystem.IsRecording ? StatusText.Recording : StatusText.NotRecording);
                    }
                    break;
                case RobotInfoCommand _:
                    {
                        requestContext.ResponseWriter.WriteLine("DummyRobot 0 0");
                    }
                    break;
                case HelloCommand command:
                    {
                        _coreSystem.SetClientName(requestContext.ClientId, command.ClientName);
                    }
                    break;
                case QuitCommand _:
                    {
                        requestContext.IsClose = true;
                    }
                    break;
                case IsConnectedCommand _:
                    {
                        requestContext.ResponseWriter.WriteLine(_coreSystem.IsRobotConnected ? StatusText.Connected : StatusText.NotConnected);
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
                        requestContext.ResponseWriter.WriteLine(_coreSystem.GetClient(requestContext.ClientId).ToString());
                    }
                    break;
                case ClientsInfoCommand _:
                    foreach (var client in _coreSystem.Clients)
                    {
                        requestContext.ResponseWriter.WriteLine(client.ToString());
                    }
                    break;
                case GetAxisCommand command:
                    if (command.AxisNumber == 0)
                    {
                        for (int i = 0; i < _coreSystem.Robot.AxisCount; ++i)
                        {
                            if (i != 0)
                            {
                                requestContext.ResponseWriter.Write(' ');
                            }
                            requestContext.ResponseWriter.Write(_coreSystem.Robot.GetAxis(i + 1));
                        }
                        requestContext.ResponseWriter.WriteLine();
                    }
                    else
                    {
                        var axisValue = _coreSystem.Robot.GetAxis(command.AxisNumber);
                        requestContext.ResponseWriter.WriteLine(axisValue.ToString());
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
                                requestContext.ResponseWriter.Write(' ');
                            }

                            requestContext.ResponseWriter.Write(position.x);
                            requestContext.ResponseWriter.Write(' ');
                            requestContext.ResponseWriter.Write(position.y);
                            requestContext.ResponseWriter.Write(' ');
                            requestContext.ResponseWriter.Write(position.z);
                        }
                        requestContext.ResponseWriter.WriteLine();
                    }
                    break;
            }
        }
    }
}
