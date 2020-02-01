using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnObject {
    public string name;
    public GameObject prefab;
    public float percentToSpawn;
    public float spawnRatio;
}
public class Spawner : MonoBehaviour
{
    private Tube tube;
    private float gameSpeed;
    private void Awake() {
        tube = GetComponent<Tube>();
    }
    public List<SpawnObject> spawnList;
    float cumulative = 0;

    private void Start() {
        gameSpeed = GameController.Instance.gameSpeed;
        CalculateSpawnRatios();
        StartCoroutine(Spawn());
        
    }
    private void CalculateSpawnRatios() {
        float totalPercentages = 0;
        for (int i = 0; i < spawnList.Count; i++) {
            totalPercentages += spawnList[i].percentToSpawn;
        }
        for (int i = 0; i < spawnList.Count; i++) {
            spawnList[i].spawnRatio = cumulative + spawnList[i].percentToSpawn/totalPercentages;
            cumulative += spawnList[i].percentToSpawn;
        }

    }
    private IEnumerator Spawn() {
        float t = 0;
        while(true) {
            t += Time.deltaTime;
            if (t > 1/gameSpeed) {
                float rand = Random.Range(0, cumulative);
                Debug.Log("CUMULATIVE" + cumulative);
                float smallestDistance = 1;
                int smallestIndex = 0;
                for (int i = 0; i < spawnList.Count; i++) {
                    if( Mathf.Abs(rand - spawnList[i].spawnRatio) < smallestDistance) {
                        smallestIndex = i;
                        smallestDistance = Mathf.Abs(rand - spawnList[smallestIndex].spawnRatio);
                    }
                    
                }
                Debug.Log(rand + ": " + smallestIndex);
                Vector3 pos = GetRandomPosition();
                Instantiate(spawnList[smallestIndex].prefab, pos, GetRotation(pos));
                t = 0;
            }
            yield return null;
        }
    }
    private Vector3 GetRandomPosition() {
        Vector3 pos = Random.insideUnitCircle.normalized * tube.radius;
        return new Vector3(pos.x,pos.y,150);
    }
    private Quaternion GetRotation(Vector3 position) {
        return Quaternion.LookRotation(Vector3.forward,-(position - new Vector3(0,0,position.z)).normalized);
    }
}
