using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

    public Button restartButton;
    public Button mainmenuButton;
 
    public void onRestart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void onMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        restartButton.onClick.AddListener(onRestart);
        mainmenuButton.onClick.AddListener(onMainMenu);
    }
}
