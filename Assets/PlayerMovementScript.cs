using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementScript : MonoBehaviour {

    public Slider slider;
    public int health = 5;
    public GameObject gameOver;

    private void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, 0, 50 * Time.deltaTime * 5); 
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0, -50 * Time.deltaTime * 5); 
        }
        slider.value = health;
        if(health <= 0)
        {
            slider.gameObject.SetActive(false);
            gameOver.SetActive(true);
            Time.timeScale = 0;
        }
        
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Shootable" || col.gameObject.tag == "Friendly")
        {
            health -= 1;
            Destroy(col.collider.gameObject);
        }
    }
}
