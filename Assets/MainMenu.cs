using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button optionsButton;
    public Button quitButton;
    public Animation anim;
    public Canvas canvas;
    private bool pressed;
    private void Start()
    {
        anim = GetComponent<Animation>();
    }
    public void PlayAnim()
    {
        pressed = true;
        anim.Play();
        canvas.gameObject.SetActive(false);
    }

    public void Options()
    {
        //No options
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    public void Update()
    {
        if(pressed)
        {
            StartCoroutine(DisableCamera());
        }
        playButton.onClick.AddListener(PlayAnim);
        quitButton.onClick.AddListener(OnApplicationQuit);
        
    }

    public GameObject player;
    IEnumerator DisableCamera()
    {
        yield return new WaitForSeconds(3.3f);
        player.SetActive(true);
        gameObject.SetActive(false);
    }
}
