using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Tube tube;
    private float gameSpeed;
    private void Awake() {
        tube = GetComponent<Tube>();
    }
    [System.Serializable]
    public struct SpawnObject {
        public string name;
        public GameObject prefab;
        public float intensity;
    }
    public List<SpawnObject> spawnList;

    private void Start() {
        gameSpeed = GameController.Instance.gameSpeed;
        foreach(SpawnObject g in spawnList) {
            StartCoroutine(Spawn(g.prefab,g.intensity));
        }
        
    }
    private IEnumerator Spawn(GameObject pref,float _intensity ) {
        float t = 0;
        while(true) {
            t += Time.deltaTime;
            if (t > 10/_intensity/gameSpeed) {
                Vector3 pos = GetRandomPosition();
                Instantiate(pref, pos, GetRotation(pos));
                t = 0;
            }
            yield return null;
        }
    }
    private Vector3 GetRandomPosition() {
        Vector3 pos = Random.insideUnitCircle.normalized * tube.radius;
        Vector3 normal = (pos - new Vector3(0, 0, pos.z)).normalized;
        pos -= normal * 0.5f;
        return new Vector3(pos.x,pos.y,50);
    }
    private Quaternion GetRotation(Vector3 position) {
        return Quaternion.LookRotation(Vector3.forward,position - new Vector3(0,0,position.z));
    }
}
