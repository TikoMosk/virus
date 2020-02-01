using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EditorCurvature : MonoBehaviour
{
    public Material curveMat;
    private void Start() {
        if (!EditorApplication.isPlaying) {
            curveMat.SetFloat("_CurvatureX", 0);
            curveMat.SetFloat("_CurvatureY", 0);
        }

    }
}
