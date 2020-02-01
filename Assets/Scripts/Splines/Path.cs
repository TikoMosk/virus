using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path 
{
    [SerializeField,HideInInspector]
    List<Vector3> points;
    [SerializeField,HideInInspector]
    bool smoothControlPoints = true;

    public bool SmoothControlPoints {
        get {
            return smoothControlPoints;
        }
        set {
            if(smoothControlPoints != value) {
                smoothControlPoints = value;
                if(smoothControlPoints) {
                    SmoothAllControlPoints();
                }
            }
        }
    }

    public Vector3 GetPoint(int i) {
        return points[i];
    }
    public void SetPoint(int i,Vector3 point) {
        points[i] = point;
    }
    public int NumberOfPoints {
        get {
            return points.Count;
        }
    }
    public int NumberOfSegments {
        get {
            return (points.Count - 4) / 3 + 1;
        }
    }

    //Constructor
    public Path(Vector3 center) {
        points = new List<Vector3> {
            center + Vector3.left,
            center + (Vector3.left + Vector3.up) * 0.5f,
            center + Vector3.right,
            center + (Vector3.right + Vector3.down) * 0.5f,

        };
    }

    //Adds a segment (consists of 1 Anchor point and 2 Control points
    public void AddSegment(Vector3 anchorPosition) {
        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
        points.Add((points[points.Count - 1] + anchorPosition) * 0.5f);
        points.Add(anchorPosition);
        SmoothAllUpdatedControlPoints(points.Count-1);
    }
    //Returns all points in the segment
    public Vector3[] GetPointsInSegment(int i) {
        return new Vector3[] { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[i * 3 + 3] };
    }
    //Smooths AnchorPoints
    private void SmoothAnchorControlPoints(int anchorIndex) {
        Vector3 anchorPos = GetPoint(anchorIndex);
        Vector3 dir = Vector3.zero;
        float[] neighbourDistances = new float[2];

        if(anchorIndex - 3 >= 0) {
            Vector3 offset = GetPoint(LoopIndex(anchorIndex - 3)) - anchorPos;
            dir += offset.normalized;
            neighbourDistances[0] = offset.magnitude;
        }
        if (anchorIndex + 3 >= 0) {
            Vector3 offset = GetPoint(LoopIndex(anchorIndex + 3)) - anchorPos;
            dir -= offset.normalized;
            neighbourDistances[1] = -offset.magnitude;
        }
        dir.Normalize();
        for (int i = 0; i < 2; i++) {
            int controlIndex = anchorIndex + i * 2 - 1;
            if(controlIndex >= 0 && controlIndex < points.Count) {
                SetPoint(LoopIndex(controlIndex), anchorPos + dir * neighbourDistances[i] * 0.5f); 
            }
        }

    }
    //Smooths Control points on the ends of the path
    private void SmoothEndControlPoints() {
        SetPoint(1,(GetPoint(0) + GetPoint(2)) *0.5f);
        SetPoint(points.Count - 2, (GetPoint(points.Count - 1) + GetPoint(points.Count -3)) * 0.5f);
    }
    private void SmoothAllUpdatedControlPoints(int updatedAnchorIndex) {
        for (int i = updatedAnchorIndex - 3; i <= updatedAnchorIndex + 3; i+=3) {
            if(i >= 0 && i< points.Count) {
                SmoothAnchorControlPoints(LoopIndex(i));
            }

        }
        SmoothEndControlPoints();
    }
    //Smooths all points on the path
    private void SmoothAllControlPoints() {
        for (int i = 0; i < points.Count; i+=3) {
            SmoothAnchorControlPoints(i);
        }
        SmoothEndControlPoints();
    }
    int LoopIndex(int i) {
        if(i < 0 || i >= points.Count) {
            return (i + points.Count) % points.Count;
        }
        else {
            return i;
        }
        
    }
    //Moves point and does appropriate calculations for its control points
    public void MovePoint(int i,Vector3 newPosition) {
        Vector3 deltaMove = newPosition - GetPoint(i);
        SetPoint(i, newPosition);
        if(smoothControlPoints) {
            SmoothAllUpdatedControlPoints(i);
        }
        else {
            if (i % 3 == 0) {

                if (i + 1 < points.Count) {
                    SetPoint(i + 1, GetPoint(i + 1) + deltaMove);
                }
                if (i - 1 >= 0) {
                    SetPoint(i - 1, GetPoint(i - 1) + deltaMove);
                }
            }
            else {
                bool nextPointIsAnchor = (i + 1) % 3 == 0;
                int correspondingControlIndex = (nextPointIsAnchor) ? i + 2 : i - 2;
                int anchorIndex = (nextPointIsAnchor) ? i + 1 : i - 1;

                if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count) {
                    float dst = (GetPoint(anchorIndex) - GetPoint(correspondingControlIndex)).magnitude;
                    Vector3 dir = (GetPoint(anchorIndex) - newPosition).normalized;
                    SetPoint(correspondingControlIndex, GetPoint(anchorIndex) + dir * dst);
                }

            }
        }
        
    }
    public Vector3[] EvenlySpacedPoints(float spacing, float resolution = 1) {
        List<Vector3> evenlySpacedPoints = new List<Vector3>();
        evenlySpacedPoints.Add(GetPoint(0));
        Vector3 previousPoint = GetPoint(0);
        float distanceSinceLastPoint = 0;

        for (int i = 0; i < NumberOfSegments; i++) {
            Vector3[] p = GetPointsInSegment(i);
            float controlNetLength = Vector3.Distance(p[0], p[1]) + Vector3.Distance(p[1], p[2]) + Vector3.Distance(p[2], p[3]);
            float estimatedCurveLength = Vector3.Distance(p[0], p[3]) + controlNetLength / 2f;
            int divisions = Mathf.CeilToInt(estimatedCurveLength * resolution * 10);
            float t = 0;
            while(t<= 1) {
                t += 1f/divisions;
                Vector3 pointOnCurve = Bezier.Cubic(p[0], p[1], p[2], p[3], t);
                distanceSinceLastPoint += Vector3.Distance(previousPoint, pointOnCurve);

                while (distanceSinceLastPoint >= spacing) {
                    float overShootDistance = distanceSinceLastPoint - spacing;
                    Vector3 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overShootDistance;
                    evenlySpacedPoints.Add(newEvenlySpacedPoint);
                    distanceSinceLastPoint = overShootDistance;
                    previousPoint = newEvenlySpacedPoint;
                }
                previousPoint = pointOnCurve;
            }
        }
        return evenlySpacedPoints.ToArray();
    }
    public List<Vector3> EvenlySpacedSpherePoints(Vector3 point,Vector3 planeA, Vector3 planeB, float radius, int amount) {
        List<Vector3> spherePoints = new List<Vector3>();
        for (int i = 0; i < amount; i++) {
            float angle = i * Mathf.PI * 2f / amount;
            Vector2 newPos = new Vector2(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius);
            //Debug.DrawLine(point, point + Vector3.Cross(planeA,planeB).normalized* 0.5f);
            Vector3 a = planeA * newPos.x;
            Vector3 b = planeB * newPos.y;
            spherePoints.Add(point + a + b);
        }
        return spherePoints;
    }
}
