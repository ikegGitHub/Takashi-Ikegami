using UnityEngine;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class JointController : MonoBehaviour
    {
        [SerializeField]
        JointTable jointTable = null;

        public JointItem[] GetItem(int axisNum)
        {
            var entity = jointTable?.GetEntity(axisNum);
            return entity.JointItems;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 GetTangentVector(Vector3 normal)
            {
                var d = Vector3.Dot(normal, Vector3.right) / normal.magnitude;
                if (Mathf.Abs(d) == 1.0f)
                {
                    return Vector3.forward;
                }
                return Vector3.right - d * normal.normalized;
            }

            var selected = UnityEditor.Selection.activeGameObject;
            if (selected == null)
            {
                return;
            }

            foreach (var entry in jointTable.list)
            {
                foreach (var joint in entry.JointItems)
                {
                    if (selected.name != joint.JointName)
                    {
                        continue;
                    }

                    var t = transform.FindName(joint.JointName)?.transform;
                    if (t != null)
                    {
                        var tangentDir = GetTangentVector(joint.Axis);
                        var minRot = Quaternion.AngleAxis(joint.rangeMin, joint.Axis);
                        var maxRot = Quaternion.AngleAxis(joint.rangeMax, joint.Axis);

                        Gizmos.color = Color.red;
                        Gizmos.DrawRay(t.position, joint.Axis);
                        UnityEditor.Handles.color = new Color(1, 1, 0, 0.1f);
                        UnityEditor.Handles.DrawSolidArc(t.position, joint.Axis, minRot * tangentDir, joint.rangeMax - joint.rangeMin, 0.9f);
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawRay(t.position, minRot * tangentDir);
                        Gizmos.DrawRay(t.position, maxRot * tangentDir);
                        GUI.color = Color.white;
                        UnityEditor.Handles.Label(t.position + joint.Axis, joint.JointName);
                        UnityEditor.Handles.Label(t.position + minRot * tangentDir, joint.rangeMin.ToString());
                        UnityEditor.Handles.Label(t.position + maxRot * tangentDir, joint.rangeMax.ToString());
                    }
                }
            }
        }
#endif
    }
}
