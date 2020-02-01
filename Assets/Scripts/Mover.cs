using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    float speed;
    private void Start() {
        speed = 50f * GameController.Instance.gameSpeed;
    }
    void Update()
    {
        transform.position += new Vector3(0, 0, -1) * Time.deltaTime * speed;
    }
}
