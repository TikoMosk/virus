using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    float speed = 250;

    public float smooth = 1f;
    private Vector3 targetAngles;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 moveVector = -transform.forward * speed;
        rb.velocity = moveVector;

        transform.Translate(Input.acceleration.x, 0, 0);
        if (Input.GetKey(KeyCode.A))
        {
            //transform.localRotation = Quaternion.Euler(0, 0 ,1);
            targetAngles = transform.eulerAngles + 10f * Vector3.forward;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //transform.localRotation = Quaternion.Euler(0, 0,1);
            targetAngles = transform.eulerAngles + 10f * Vector3.back;
        }

        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngles, 10 * smooth * Time.deltaTime);
        //else if (Input.GetKey(KeyCode.W))
        //{
        //    rb.velocity = new Vector3(0, speed / 5, speed);
        //}
        //else if (Input.GetKey(KeyCode.S))
        //{
        //    rb.velocity = new Vector3(0, -speed / 5, speed);
        //}
        //rb.AddForce(Vector3.forward * speed * Time.deltaTime - rb.velocity.normalized,ForceMode.VelocityChange);
    }
}
