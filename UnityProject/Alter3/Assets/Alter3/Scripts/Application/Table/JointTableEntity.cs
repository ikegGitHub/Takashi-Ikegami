using System;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    [Serializable]
    public class JointTableEntity : TableEntityBase<int>
    {
        [Header("名称")]
        public string Name;     //名称(プログラムでは使用しない)
        [Header("動作内容")]
        public string Content;          //動作内容(プログラムでは使用しない)
        [Header("Joint定義")]
        public JointItem[] JointItems;
    }
}
