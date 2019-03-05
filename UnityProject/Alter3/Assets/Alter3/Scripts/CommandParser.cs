using System;
using System.Collections.Generic;

namespace XFlag.Alter3Simulator
{
    public class CommandParser
    {
        private const int PriorityMax = 10000;

        public ICommand ParseCommandLine(string line)
        {
            var args = Split(line);
            if (args.Length == 0)
            {
                throw new ApplicationException("invalid command line");
            }

            var command = args[0].ToUpper();
            switch (command)
            {
                case "ADDAXIS": // <<ADDAXIS>> ::= "ADDAXIS" <<GENERIC_PARAMS>>
                    return CommandFactory.CreateAddAxisCommand(ParseGenericAxisArgs(args));
                case "APPENDAXIS": // <<APPENDAXIS>> ::= "APPENDAXIS" <<GENERIC_PARAMS>>
                    return CommandFactory.CreateAppendAxisCommand(ParseGenericAxisArgs(args));
                case "GETAXIS": // <<GETAXIS>> ::= "GETAXIS" [<AXIS_NUM>]
                    if (args.Length == 1)
                    {
                        return CommandFactory.CreateGetAxisCommand(0);
                    }
                    else if (args.Length == 2)
                    {
                        var axisNum = ParseAxisNum(args[1]);
                        return CommandFactory.CreateGetAxisCommand(axisNum);
                    }
                    break;
                case "HELLO": // <<HELLO>> ::= "HELLO" <STRING>
                    return CommandFactory.CreateHelloCommand(args[1]);
                case "HELP":
                    return CommandFactory.CreateHelpCommand();
                case "MOVEAXIS": // <<MOVEAXIS>> ::= "MOVEAXIS" <<GENERIC_PARAMS>>
                    return CommandFactory.CreateMoveAxisCommand(ParseGenericAxisArgs(args));
                case "MOVEAXES": // <<MOVEAXES>> ::= "MOVEAXES" <<MOVEAXES_PARAMS>> {<<MOVEAXES_PARAMS>>}
                    return CommandFactory.CreateMoveAxesCommand(ParseMultipleAxisArgs(args));
                case "NOOP":
                    return CommandFactory.CreateNoopCommand();
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
                    return CommandFactory.CreatePlayMotionCommand(clear, path, priority);
                case "PRINTQUEUE": // <<PRINTQUEUE>> ::= "PRINTQUEUE" [<AXIS_NUM>]
                    if (args.Length == 1)
                    {
                        return CommandFactory.CreatePrintQueueCommand(0);
                    }
                    else if (args.Length == 2)
                    {
                        var axisNum = ParseAxisNum(args[1]);
                        return CommandFactory.CreatePrintQueueCommand(axisNum);
                    }
                    break;
                case "QUIT":
                case "BYE": // <<QUIT_BYE>> ::= "QUIT" | "BYE"
                    return CommandFactory.CreateQuitCommand();
                case "CLEARQUEUE": // <<CLEARQUEUE>> ::= "CLEARQUEUE" [<AXIS_NUM>]
                    if (args.Length == 1)
                    {
                        return CommandFactory.CreateClearQueueCommand(0);
                    }
                    else if (args.Length == 2)
                    {
                        var axisNum = ParseAxisNum(args[1]);
                        return CommandFactory.CreateClearQueueCommand(axisNum);
                    }
                    break;
                case "CLIENTSINFO":
                    return CommandFactory.CreateClientsInfoCommand();
                case "CONNECTROBOT":
                    return CommandFactory.CreateConnectRobotCommand();
                case "DISCONNECTROBOT":
                    return CommandFactory.CreateDisconnectRobotCommand();
                case "ISCONNECTED":
                    return CommandFactory.CreateIsConnectedCommand();
                case "ISRECORDINGMOTION":
                    return CommandFactory.CreateIsRecordingMotionCommand();
                case "RECORDMOTION": // <<RECORDMOTION>> ::= "RECORDMOTION" ("START" | "STOP")
                    switch (args[1].ToUpper())
                    {
                        case "START":
                            return CommandFactory.CreateRecordMotionCommand(false);
                        case "STOP":
                            return CommandFactory.CreateRecordMotionCommand(true);
                    }
                    break;
                case "RESETPOSE":
                    return CommandFactory.CreateResetPoseCommand();
                case "ROBOTINFO":
                    return CommandFactory.CreateRobotInfoCommand();
                case "WHOAMI":
                    return CommandFactory.CreateWhoAmICommand();
                case "CALIB":
                    return CommandFactory.CreateCalibCommand();
                case "GETPOSITIONS":
                    return CommandFactory.CreateGetPositionsCommand();
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

        private string[] Split(string line)
        {
            var args = new List<string>(8);

            int pos = 0;

            while (pos < line.Length && char.IsWhiteSpace(line[pos]))
            {
                ++pos;
            }

            while (pos < line.Length)
            {
                int i = pos + 1;
                while (i < line.Length && !char.IsWhiteSpace(line[i]))
                {
                    ++i;
                }
                args.Add(line.Substring(pos, i - pos));

                pos = i + 1;
                while (pos < line.Length && char.IsWhiteSpace(line[pos]))
                {
                    ++pos;
                }
            }

            return args.ToArray();
        }
    }
}
