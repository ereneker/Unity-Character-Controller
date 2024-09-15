using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

public class HeadViewGizmo : MonoBehaviour
{
    [SerializeField] private GameObject headIK;
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;
        Gizmos.color = Color.yellow;
        Vector3 forwardDir = headIK.transform.forward;
        
        Vector3 leftBoundary = Quaternion.Euler(0, -180f / 2, 0) * forwardDir;
        Vector3 rightBoundary = Quaternion.Euler(0, 180f / 2, 0) * forwardDir;

        Gizmos.DrawLine(headIK.transform.position, headIK.transform.position + leftBoundary * 5f);
        Gizmos.DrawLine(headIK.transform.position, headIK.transform.position + rightBoundary * 5f);

        Handles.DrawSolidArc(headIK.transform.position, Vector3.up, leftBoundary, 180f, 5f);
    }
}
