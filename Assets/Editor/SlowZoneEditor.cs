using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SlowZone))]
public class SlowZoneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SlowZone slowZone = (SlowZone)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Add Waypoint"))
        {
            Undo.RecordObject(slowZone, "Add Waypoint");

            if (slowZone.waypoints.Count == 0)
            {
                slowZone.waypoints.Add(Vector2.zero);
            }
            else
            {
                slowZone.waypoints.Add(
                    slowZone.waypoints[slowZone.waypoints.Count - 1] + Vector2.right
                );
            }
            EditorUtility.SetDirty(slowZone);
        }

        if (GUILayout.Button("Remove Last Waypoint"))
        {
            Undo.RecordObject(slowZone, "Remove Waypoint");

            if (slowZone.waypoints.Count > 1)
                slowZone.waypoints.RemoveAt(slowZone.waypoints.Count - 1);

            EditorUtility.SetDirty(slowZone);
        }
    }

    private void OnSceneGUI()
    {
        SlowZone slowZone = (SlowZone)target;

        if (!slowZone.movable)
            return;

        Vector3 origin = slowZone.transform.position;

        Handles.color = Color.cyan;

        for (int i = 0; i < slowZone.waypoints.Count; i++)
        {
            Vector3 worldPoint = origin + (Vector3)slowZone.waypoints[i];

            EditorGUI.BeginChangeCheck();

            worldPoint = Handles.PositionHandle(worldPoint, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(slowZone, "Move Waypoint");

                slowZone.waypoints[i] = worldPoint - origin;

                EditorUtility.SetDirty(slowZone);
            }

            Handles.Label(worldPoint + Vector3.up * 0.25f, $"Waypoint {i}");

            Vector3 nextPoint =
                origin + (Vector3)slowZone.waypoints[(i + 1) % slowZone.waypoints.Count];

            Handles.DrawLine(worldPoint, nextPoint);
        }
    }
}