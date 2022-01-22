using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;

public class NetworkConnector : NetworkManager
{
    [Header("DUMMY PLAYER")]
    [SerializeField] public GameObject dummyPlayer;

    public override void Start()
    {
        base.Start();
        //Get the current player that is selected by the user. For now lez just do predefine value
        //singleton.playerPrefab = GameObject.Find("MyPlayerPrefabs").GetComponent<PlayerPrefab>().getPrefab(GameObject.Find("Opening_Game_Script").GetComponent<Database>().UsedCharacter);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {

        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            GetComponent<NetworkDiscoveryHUD>().JoinAsClient();
        }
    }

    public void StartHostLan()
    {
        GetComponent<NetworkDiscoveryHUD>().StartHostLan();
    }

    public void StartClientLan()
    {
        GetComponent<NetworkDiscoveryHUD>().JoinAsClient();
    }
}
