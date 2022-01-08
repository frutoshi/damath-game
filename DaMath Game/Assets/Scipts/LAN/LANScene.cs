using MLAPI;
using MLAPI.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LANScene : MonoSingleton<LANScene>
{
    public GameObject mainMenu;
    public GameObject lobbyMenu;

    private void Start()
    {
        lobbyMenu.SetActive(false);
        
        DontDestroyOnLoad(gameObject);
    }

    //main
    public void onMainHostBtn()
    {
        mainMenu.SetActive(false);
        lobbyMenu.SetActive(true);
        NetworkManager.Singleton.StartHost();
        Debug.Log("Host");
    }

    public void OnMainConnectBtn()
    {
        mainMenu.SetActive(false);
        lobbyMenu.SetActive(true);
        NetworkManager.Singleton.StartClient();
        Debug.Log("Connect");
    }

    

    //lobby
    public void BackBtn()
    {
        mainMenu.SetActive(true);
        lobbyMenu.SetActive(false);
    }

    public void OnLobbyStartBtn() 
    {
        NetworkSceneManager.SwitchScene("Game");
    }
}
