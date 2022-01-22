using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    /*public GameObject loadingScreen;
    public Slider loadingBar;*/
   // public static bool GameIsPaused;
    public GameObject pauseMenuUI;
    public Animator animPause;

    public static int algo;

    public static GameManager Instance
    {
        set;
        get;
    }

    public void pauseBtn(bool gameIsPaused)
    {
        if (gameIsPaused)
        {
            pause();
        }
        else
        {
            resume();
        }

    }
    public void pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 1f;

    }

    public void resume()
    {
        Time.timeScale = 1f;
        StartCoroutine(PlayAndDisappear("Resume"));
    }

    private void Awake()
    {
        Time.timeScale = 1f;
        //GameIsPaused = false;

        //Instance = this;
        //hostMenu.SetActive(false);
        //connectMenu.SetActive(false);
        //DontDestroyOnLoad(gameObject);
    }


    public void BacktoMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayBtn()
    {
        SceneManager.LoadScene(1);
        // StartCoroutine(LoadSceneAsynchronously(levelindex));
    }

    public void SameDeviceBtn()
    {
        SceneManager.LoadScene(2);
    }
    public void ExitBtn()
    {
        Application.Quit();
        Debug.Log("Quitted the game!");
    }

    public void Easy()
    {
        algo = 1;
        SceneManager.LoadScene("GameEasy");

    }
    public void Medium()
    {
        algo = 2;
        SceneManager.LoadScene("GameMedium");

    }
    public void Hard()
    {
        algo = 3;
        SceneManager.LoadScene("GameHard");

    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
        
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        SceneManager.LoadScene(0);
    }
    IEnumerator PlayAndDisappear(string ani)
    {
        if (ani == "Resume")
        {
            animPause.SetTrigger("Closing");

            yield return new WaitForSeconds(2f);
            pauseMenuUI.SetActive(false);
        }

    }
}

