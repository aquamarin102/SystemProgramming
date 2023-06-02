using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Star)), CanEditMultipleObjects]
public class StarEditor : Editor
{
    private SerializedProperty _center;
    private SerializedProperty _points;
    private SerializedProperty _frequency;
    
    private void OnEnable()
    {
        _center = serializedObject.FindProperty("_center");
        _points = serializedObject.FindProperty("_points");
        _frequency = serializedObject.FindProperty("_frequency");

        ((Star)target).UpdateMesh();
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_center);
        EditorGUILayout.PropertyField(_points);
        EditorGUILayout.IntSlider(_frequency, 1, 20);
        var totalPoints = _frequency.intValue * _points.arraySize;
        if (totalPoints < 3)
        {
            EditorGUILayout.HelpBox("At least three points are needed.", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.HelpBox(totalPoints + " points in total.", MessageType.Info);
        }

        if (!serializedObject.ApplyModifiedProperties() && (Event.current.type != EventType.ExecuteCommand || Event.current.commandName != "UndoRedoPerformed"))
        {
            return;
        }

        foreach (var obj in targets)
        {
            if (obj is Star star)
            {
                star.UpdateMesh();
            }
        }
    }

    private void OnSceneGUI()
    {
        if (!(target is Star star))
        {
            return;
        }
        var starTransform = star.transform;
        var angle = -360f / (star.Frequency * star.Points.Length);
        Vector3 _pointSnap = Vector3.one * 0.05f;
        for (var i = 0; i < star.Points.Length; i++)
        {
            var rotation = Quaternion.Euler(0f, 0f, angle * i);
            var oldPoint = starTransform.TransformPoint(rotation * star.Points[i].Position);
            var fmh_63_61_638213305314597796 = Quaternion.identity; var newPoint = Handles.FreeMoveHandle(oldPoint, 0.02f, _pointSnap, Handles.DotHandleCap);
            if (oldPoint == newPoint)
            {
                continue;
            }
            star.Points[i].Position = Quaternion.Inverse(rotation) *
            starTransform.InverseTransformPoint(newPoint);
            star.UpdateMesh();
        }
    }

}