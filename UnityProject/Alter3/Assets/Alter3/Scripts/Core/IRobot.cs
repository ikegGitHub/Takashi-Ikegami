using System.Collections.Generic;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    /// <summary>
    /// ロボットの軸コントロールのためのインターフェースを規定します。
    /// </summary>
    public interface IRobot
    {
        int AxisCount { get; }

        void MoveAxis(AxisParam axisParam);

        void MoveAxes(AxisParam[] axisParams);

        double GetAxis(int axisNumber);

        IReadOnlyList<Vector3> GetHandsPositionArray();
    }
}
