using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float timeToSelfDestruct;
    private float t = 0;
    private void Update() {
        t += Time.deltaTime;
        if(t > timeToSelfDestruct) {
            Destroy(this.gameObject);
        }
    }
}
