using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyActions))]
public class ConeEdit : Editor
{
    private void OnSceneGUI()
    {
        EnemyActions eai = (EnemyActions)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(eai.transform.position, Vector3.up, eai.transform.forward, eai.detectionAngle/2, eai.detectionRadius);
        Handles.DrawWireArc(eai.transform.position, Vector3.up, eai.transform.forward, -eai.detectionAngle / 2, eai.detectionRadius);
        Vector3 viewAngle1 = eai.DirectionFromAngle(eai.detectionAngle / -2, false);
        Vector3 viewAngle2 = eai.DirectionFromAngle(eai.detectionAngle / 2, false);

        Handles.DrawLine(eai.transform.position, eai.transform.position + viewAngle1 * eai.detectionRadius);
        Handles.DrawLine(eai.transform.position, eai.transform.position + viewAngle2 * eai.detectionRadius);
    }
}
