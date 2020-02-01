using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    PathCreator creator;
    Path path;
    private List<Vector3> sphereVertices;
    //int amountOnCircle = 20;

    private void OnSceneGUI() {
        Input();
        Draw();
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if(GUILayout.Button("Create New Empty Path")) {
            creator.CreatePath();
            path = creator.path;
            SceneView.RepaintAll();
        }
        if (GUILayout.Button("Generate Random Path")) {
            creator.CreatePath();
            path = creator.path;
            SceneView.RepaintAll();
        }
    }
    private void Input() {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if(guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift) {
            Undo.RecordObject(creator, "Add Segment");
            path.AddSegment(new Vector3(mousePos.x,mousePos.y,Random.Range(-5,5)));
            
        }
    }

    private void Draw() {
        for (int i = 0; i < path.NumberOfSegments; i++) {
            Vector3[] points = path.GetPointsInSegment(i);
            Handles.color = Color.black;
            Handles.DrawLine(points[1], points[0]);
            Handles.DrawLine(points[2], points[3]);
            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.blue,null,2);

        }
        
        for (int i = 0; i < path.NumberOfPoints; i++) {
            float cylinderRadius;
            if(i%3 == 0) {
                Handles.color = Color.red;
                cylinderRadius = 0.1f;
            }
            else {
                Handles.color = Color.white;
                cylinderRadius = 0.05f;
            }
            Vector3 newPos = Handles.FreeMoveHandle(path.GetPoint(i), Quaternion.identity, cylinderRadius, Vector2.zero, Handles.CylinderHandleCap);
            if(path.GetPoint(i) != newPos) {
                Undo.RecordObject(creator, "MovePoint");
                path.MovePoint(i, newPos);
            }
        }
        
        
        
    }
    
    private void OnEnable() {
        creator = (PathCreator)target;
        if(creator.path == null) {
            creator.CreatePath();
        }
        path = creator.path; 
    }
}
