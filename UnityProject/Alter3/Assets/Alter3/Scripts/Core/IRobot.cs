namespace XFlag.Alter3Simulator
{
    /// <summary>
    /// ロボットの軸コントロールのためのインターフェースを規定します。
    /// </summary>
    public interface IRobot
    {
        void MoveAxis(AxisParam axisParam);
        void MoveAxes(AxisParam[] axisParams);
    }
}
