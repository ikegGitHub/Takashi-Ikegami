using System;
using System.Text.RegularExpressions;

namespace XFlag.Alter3Simulator
{
    public class CommandParser
    {
        private static readonly Regex SeparatorPattern = new Regex(@"\s+");

        private const int PriorityMax = 10000;

        public ICommand ParseCommandLine(string line)
        {
            var args = SeparatorPattern.Split(line.Trim());
            if (args.Length == 0)
            {
                throw new ApplicationException("invalid command line");
            }

            var command = args[0].ToUpper();
            switch (command)
            {
                case "ADDAXIS": // <<ADDAXIS>> ::= "ADDAXIS" <<GENERIC_PARAMS>>
                    return new AddAxisCommand { Param = ParseGenericAxisArgs(args) };
                case "APPENDAXIS": // <<APPENDAXIS>> ::= "APPENDAXIS" <<GENERIC_PARAMS>>
                    return new AppendAxisCommand { Param = ParseGenericAxisArgs(args) };
                case "GETAXIS": // <<GETAXIS>> ::= "GETAXIS" [<AXIS_NUM>]
                    if (args.Length == 1)
                    {
                        return new GetAxisCommand();
                    }
                    else if (args.Length == 2)
                    {
                        var axisNum = ParseAxisNum(args[1]);
                        return new GetAxisCommand { AxisNumber = axisNum };
                    }
                    break;
                case "HELLO": // <<HELLO>> ::= "HELLO" <STRING>
                    return new HelloCommand { ClientName = args[1] };
                case "HELP":
                    return new HelpCommand();
                case "MOVEAXIS": // <<MOVEAXIS>> ::= "MOVEAXIS" <<GENERIC_PARAMS>>
                    return new MoveAxisCommand { Param = ParseGenericAxisArgs(args) };
                case "MOVEAXES": // <<MOVEAXES>> ::= "MOVEAXES" <<MOVEAXES_PARAMS>> {<<MOVEAXES_PARAMS>>}
                    return new MoveAxesCommand { Params = ParseMultipleAxisArgs(args) };
                case "NOOP":
                    return new NoopCommand();
                case "PLAYMOTION": // <<PLAYMOTION>> ::= "PLAYMOTION" ["CLEAR"] <STRING> [<PRIORITY>]
                    var offset = 0;
                    var clear = false;
                    if (args[1].ToUpper() == "CLEAR")
                    {
                        ++offset;
                        clear = true;
                    }
                    var path = args[offset + 1];
                    var priority = 0;
                    if (offset + 2 < args.Length)
                    {
                        priority = ParsePriority(args[offset + 2]);
                    }
                    return new PlayMotionCommand { IsClear = clear, Path = path, Priority = priority };
                case "PRINTQUEUE": // <<PRINTQUEUE>> ::= "PRINTQUEUE" [<AXIS_NUM>]
                    if (args.Length == 1)
                    {
                        return new PrintQueueCommand();
                    }
                    else if (args.Length == 2)
                    {
                        var axisNum = ParseAxisNum(args[1]);
                        return new PrintQueueCommand { AxisNumber = axisNum };
                    }
                    break;
                case "QUIT":
                case "BYE": // <<QUIT_BYE>> ::= "QUIT" | "BYE"
                    return new QuitCommand();
                case "CLEARQUEUE": // <<CLEARQUEUE>> ::= "CLEARQUEUE" [<AXIS_NUM>]
                    if (args.Length == 1)
                    {
                        return new ClearQueueCommand();
                    }
                    else if (args.Length == 2)
                    {
                        var axisNum = ParseAxisNum(args[1]);
                        return new ClearQueueCommand { AxisNumber = axisNum };
                    }
                    break;
                case "CLIENTSINFO":
                    return new ClientsInfoCommand();
                case "CONNECTROBOT":
                    return new ConnectRobotCommand();
                case "DISCONNECTROBOT":
                    return new DisconnectRobotCommand();
                case "ISCONNECTED":
                    return new IsConnectedCommand();
                case "ISRECORDINGMOTION":
                    return new IsRecordingMotionCommand();
                case "RECORDMOTION": // <<RECORDMOTION>> ::= "RECORDMOTION" ("START" | "STOP")
                    switch (args[1].ToUpper())
                    {
                        case "START":
                            return new RecordMotionCommand { IsStop = false };
                        case "STOP":
                            return new RecordMotionCommand { IsStop = true };
                    }
                    break;
                case "RESETPOSE":
                    return new ResetPoseCommand();
                case "ROBOTINFO":
                    return new RobotInfoCommand();
                case "WHOAMI":
                    return new WhoAmICommand();
                case "CALIB":
                    return new CalibCommand();
            }

            throw new ApplicationException($"unknown command: '{command}'");
        }

        private AxisParam[] ParseMultipleAxisArgs(string[] args)
        {
            if (args.Length < 5 || (args.Length - 1) % 4 != 0)
            {
                throw new ApplicationException("invalid count");
            }

            var axisParams = new AxisParam[(args.Length - 1) / 4];
            for (int i = 0; i < axisParams.Length; ++i)
            {
                var k = 1 + 4 * i;
                axisParams[i] = ParseAxisArgs(args[k], args[k + 1], args[k + 2], args[k + 3]);
            }
            return axisParams;
        }

        private AxisParam ParseGenericAxisArgs(string[] args)
        {
            switch (args.Length)
            {
                case 3:
                    return ParseAxisArgs(args[1], args[2]);
                case 4:
                    return ParseAxisArgs(args[1], args[2], args[3]);
                case 5:
                    return ParseAxisArgs(args[1], args[2], args[3], args[4]);
            }
            throw new ApplicationException("invalid length");
        }

        private AxisParam ParseAxisArgs(string arg0, string arg1, string arg2 = null, string arg3 = null)
        {
            var axisNum = ParseAxisNum(arg0);

            double value;
            if (!double.TryParse(arg1, out value))
            {
                throw new ApplicationException($"VALUE invalid format: {arg1}");
            }

            var priority = 0;
            if (arg2 != null)
            {
                priority = ParsePriority(arg2);
            }

            var duration = 0;
            if (arg3 != null)
            {
                duration = ParseDuration(arg3);
            }

            return new AxisParam { AxisNumber = axisNum, Value = value, Priority = priority, Duration = duration };
        }

        private int ParseAxisNum(string s)
        {
            int axisNum;
            if (!int.TryParse(s, out axisNum))
            {
                throw new ApplicationException($"AXIS_NUM invalid format: {s}");
            }
            if (axisNum < 1)
            {
                throw new ApplicationException($"AXIS_NUM must be >= 1: {axisNum}");
            }
            return axisNum;
        }

        private int ParsePriority(string s)
        {
            int priority;
            if (!int.TryParse(s, out priority))
            {
                throw new ApplicationException($"PRIORITY invalid format: {s}");
            }
            if (priority < 0 || PriorityMax < priority)
            {
                throw new ApplicationException($"PRIORITY out of bounds: {priority}");
            }
            return priority;
        }

        private int ParseDuration(string s)
        {
            int duration;
            if (!int.TryParse(s, out duration))
            {
                throw new ApplicationException($"DURATION invalid format: {s}");
            }
            if (duration < 0)
            {
                throw new ApplicationException($"DURATION must be >= 0: {duration}");
            }
            return duration;
        }
    }
}
