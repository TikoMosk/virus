using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{
    public float radius;
    private float gameSpeed;
    public List<Material> curveMats;
    public GameObject tube;
    private List<GameObject> tubes;
    Vector3 spawnPos;
    public float minCurve;
    public float maxCurve;
    float currentTime;
    private float curveX;
    private float curveY;
    public float curveChangeTime;
    public float tubeSize = 90;
    private int index = 0;
    private void Start() {
        gameSpeed = GameController.Instance.gameSpeed;
        SpawnTubes();
        InvokeRepeating("ChangeCurve", 0.5f /gameSpeed, 1 /gameSpeed);
        InvokeRepeating("MoveCube", 0.5f, 0.9f / gameSpeed);
    }

    private void SpawnTubes() {
        tubes = new List<GameObject>();
        for (int i = 0; i < 20; i++) {
            Vector3 spawnPos = Vector3.forward *  (i * tubeSize);
            GameObject go = Instantiate(tube, spawnPos, Quaternion.identity);
            tubes.Add(go);
        }
        
    }
    private void MoveCube() {
        Vector3 movePos = tubes[index].transform.position + Vector3.forward * (20 * tubeSize);
        tubes[index].transform.position = movePos;
        GameObject temp = tubes[index];
        index++;
        if(index >= 20) {
            index = 0;
        }
    }
    private void ChangeCurve() {

        float currentX = curveMats[0].GetFloat("_CurvatureX");
        float currentY = curveMats[0].GetFloat("_CurvatureY");
        float x = Random.Range(minCurve, maxCurve);
        float y = Random.Range(minCurve, maxCurve);
        StartCoroutine(CurveCurve(currentX, currentY, x, y));
        
    }
    private IEnumerator CurveCurve(float currentX,float currentY,float x, float y) {
        float t = 0;
        while (t < 10 / gameSpeed) {
            t += Time.deltaTime * gameSpeed;
            curveX = Mathf.Lerp(currentX, x, t);
            curveY = Mathf.Lerp(currentY, y, t);

            for (int i = 0; i < curveMats.Count; i++) {

                curveMats[i].SetFloat("_CurvatureX", curveX);
                curveMats[i].SetFloat("_CurvatureY", curveY);
            }
            yield return new WaitForEndOfFrame();
        }
    }


}
