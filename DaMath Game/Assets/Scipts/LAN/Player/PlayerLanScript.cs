using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerLanScript : NetworkBehaviour
{
    void Start()
    {
        if (isLocalPlayer) 
        {
            if (isServer && isClient) //For Host
            {
                try
                {
                    Debug.Log("I am Server and Client");
                    GameObject.Find("InGameManager").GetComponent<LanGameEngine>().HostInit();
                    GameObject.Find("InGameManager").GetComponent<LanGameEngine>().CmdSetObject(this.gameObject, 1);
                }
                catch (Exception e) 
                {
                    Debug.LogError(e.Message);
                }
            }
            if (!isServer && isClient) //For Client
            {
                try
                {
                    GameObject.Find("InGameManager").GetComponent<LanGameEngine>().CmdSetObject(this.gameObject, 2);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
