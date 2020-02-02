using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEatingPill : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("SampleScene");
    }
}
