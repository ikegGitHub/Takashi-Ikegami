namespace XFlag.Alter3Simulator
{
    public static class CommandFactory
    {
        public static AddAxisCommand CreateAddAxisCommand(AxisParam param)
        {
            return new AddAxisCommand { Param = param };
        }

        public static AppendAxisCommand CreateAppendAxisCommand(AxisParam param)
        {
            return new AppendAxisCommand { Param = param };
        }

        public static GetAxisCommand CreateGetAxisCommand(int axisNumber)
        {
            return new GetAxisCommand { AxisNumber = axisNumber };
        }

        public static HelloCommand CreateHelloCommand(string clientName)
        {
            return new HelloCommand { ClientName = clientName };
        }

        public static MoveAxisCommand CreateMoveAxisCommand(AxisParam param)
        {
            return new MoveAxisCommand { Param = param };
        }

        public static MoveAxesCommand CreateMoveAxesCommand(params AxisParam[] @params)
        {
            return new MoveAxesCommand { Params = @params };
        }

        public static PlayMotionCommand CreatePlayMotionCommand(bool isClear, string path, int priority)
        {
            return new PlayMotionCommand { IsClear = isClear, Path = path, Priority = priority };
        }

        public static RecordMotionCommand CreateRecordMotionCommand(bool isStop)
        {
            return new RecordMotionCommand { IsStop = isStop };
        }

        public static PrintQueueCommand CreatePrintQueueCommand(int axisNumber)
        {
            return new PrintQueueCommand { AxisNumber = axisNumber };
        }

        public static ClearQueueCommand CreateClearQueueCommand(int axisNumber)
        {
            return new ClearQueueCommand { AxisNumber = axisNumber };
        }

        public static HelpCommand CreateHelpCommand() => Singleton<HelpCommand>.Instance;

        public static NoopCommand CreateNoopCommand() => Singleton<NoopCommand>.Instance;

        public static QuitCommand CreateQuitCommand() => Singleton<QuitCommand>.Instance;

        public static ClientsInfoCommand CreateClientsInfoCommand() => Singleton<ClientsInfoCommand>.Instance;

        public static ConnectRobotCommand CreateConnectRobotCommand() => Singleton<ConnectRobotCommand>.Instance;

        public static DisconnectRobotCommand CreateDisconnectRobotCommand() => Singleton<DisconnectRobotCommand>.Instance;

        public static IsConnectedCommand CreateIsConnectedCommand() => Singleton<IsConnectedCommand>.Instance;

        public static IsRecordingMotionCommand CreateIsRecordingMotionCommand() => Singleton<IsRecordingMotionCommand>.Instance;

        public static ResetPoseCommand CreateResetPoseCommand() => Singleton<ResetPoseCommand>.Instance;

        public static RobotInfoCommand CreateRobotInfoCommand() => Singleton<RobotInfoCommand>.Instance;

        public static WhoAmICommand CreateWhoAmICommand() => Singleton<WhoAmICommand>.Instance;

        public static CalibCommand CreateCalibCommand() => Singleton<CalibCommand>.Instance;

        public static GetPositionsCommand CreateGetPositionsCommand() => Singleton<GetPositionsCommand>.Instance;

        private class Singleton<T> where T : ICommand, new()
        {
            public static readonly T Instance = new T();
        }
    }
}
