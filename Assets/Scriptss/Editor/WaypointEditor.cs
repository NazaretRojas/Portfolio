using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor
{
    private Waypoint waypoint;

    private void OnSceneGUI()
    {
        waypoint = (Waypoint)target;

        
        if (waypoint == null || waypoint.Points == null || waypoint.Points.Length == 0)
            return;

        for (int i = 0; i < waypoint.Points.Length; i++)
        {
            Vector3 currentWorldPos = waypoint.transform.position + waypoint.Points[i];

            EditorGUI.BeginChangeCheck();

            
            Vector3 newWorldPos = Handles.PositionHandle(currentWorldPos, Quaternion.identity);

            
            Handles.Label(
                newWorldPos + Vector3.down * 0.35f + Vector3.right * 0.35f,
                $"{i + 1}",
                new GUIStyle
                {
                    fontStyle = FontStyle.Bold,
                    fontSize = 16,
                    normal = { textColor = Color.white }
                });

            if (EditorGUI.EndChangeCheck())
            {
               
                Undo.RecordObject(waypoint, "Move Waypoint Point");
                waypoint.SetPoint(i, newWorldPos - waypoint.transform.position);
                EditorUtility.SetDirty(waypoint);
            }
        }
    }
}