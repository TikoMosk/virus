using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    [HideInInspector]
    public Path path;
    private int amountOnCircle = 20;
    private List<Vector3> vertices;
    private List<int> triangles;
    public Material mat;

    private void Awake() {
        triangles = new List<int>();
        vertices = new List<Vector3>();
    }
    public void CreatePath() {
        
        path = new Path(Vector3.zero);

    }
    private void CreateCircles() {
        Vector3[] evenPoints = path.EvenlySpacedPoints(0.3f, 1);
        List<Vector3> currentVertices = new List<Vector3>();
        int amountOnCircle = 20;
        for (int i = 0; i < evenPoints.Length; i++) {
            Vector3 currentP = evenPoints[i];
            Vector3 nextP = checkInRange(i + 1, evenPoints.Length) ? evenPoints[i + 1] : currentP;
            Vector3 nextNextP = checkInRange(i + 2, evenPoints.Length) ? evenPoints[i + 2] : currentP;
            Vector3 dirA = nextP - currentP;
            Vector3 dirB = nextNextP - nextP;
            Vector3 a = Vector3.Cross(dirA, dirB).normalized;
            Vector3 b = Vector3.Cross(a, dirA).normalized;
            currentVertices = path.EvenlySpacedSpherePoints(currentP, a, b, 0.2f, amountOnCircle);
            foreach (Vector3 v in currentVertices) {
                vertices.Add(v);
            }
        };
        
        
    }
    

    private void Start() {
        CreateCircles();
        CreateMesh();
    }
    public void CreateMesh() {

        int amountOfRows = vertices.Count/amountOnCircle;
        int i = 0;
        int j = amountOnCircle;
        int row = 0;

        while(j < vertices.Count - amountOnCircle) {
            while (i < amountOnCircle * row + amountOnCircle && j < amountOnCircle * (row + 1) + amountOnCircle) {
                    triangles.Add(j);
                    triangles.Add(j+1);
                    triangles.Add(i);

                    triangles.Add(j+1);
                    triangles.Add(i+1);
                    triangles.Add(i);
                    i++;
                    j++;
                }
            row++;
        }
        while (triangles.Count % 3 != 0) {
            //triangles.RemoveAt(triangles.Count-1);
        }
        CreateTunnelObject();
    }
    public void CreateTunnelObject() {
        GameObject obj = new GameObject();
        obj.AddComponent<MeshFilter>().mesh.vertices = vertices.ToArray();
        obj.GetComponent<MeshFilter>().mesh.triangles = triangles.ToArray();
        obj.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        obj.AddComponent<MeshRenderer>();
        obj.GetComponent<MeshRenderer>().material = mat;
        obj.name = "path";
    }
    private bool checkInRange(int index, int size) {
        if (index >= 0 && index < size) {
            return true;
        }
        else {
            return false;
        }
    }
}
