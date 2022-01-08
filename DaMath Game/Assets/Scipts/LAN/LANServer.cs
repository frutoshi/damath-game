using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.IO;

public class LANServer : MonoBehaviour
{
    public int port = 6688;

    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;

    private TcpListener server;
    private bool serverStarted;

    public void Init()
    {
        DontDestroyOnLoad(gameObject);
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            StartListening();
            serverStarted = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket Error: " + e.Message);
        }
    }
    public void Update()
    {
        if (!serverStarted)
            return;

        foreach(ServerClient c in clients)
        {
            //if the client is still connected?
            if (!IsConnected(c.tcp))
            {
                c.tcp.Close();
                disconnectList.Add(c);
                continue;
            }
            else
            {
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();

                    if (data != null)
                        OnIncomingData(c, data);
                }
            }
        }

        for (int i = 0; i < disconnectList.Count - 1; i++)
        {

            //tell player that somebody disconnected

            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
        }
    }


    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }
    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;

        string allUsers = "";
        foreach (ServerClient sCl in clients)
        {
            allUsers += sCl.clientName + '|';
        }

        ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar));
        clients.Add(sc);
        StartListening();

        //sending message from server to client
        Broadcast("S-WHO|" + allUsers, clients[clients.Count - 1]) ;

        //Debug.Log("Somebody has connected!");
            
    }

    private bool IsConnected(TcpClient tcpC)
    {
        try
        {
            if (tcpC != null && tcpC.Client != null && tcpC.Client.Connected)
            {
                if (tcpC.Client.Poll(0, SelectMode.SelectRead))
                    return !(tcpC.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                return true;
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }

    //server send
    private void Broadcast(string data, List<ServerClient> cl)
    {
        foreach(ServerClient sc in cl)
        {
            try
            {
                StreamWriter writer = new StreamWriter(sc.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch (Exception e)
            {
                Debug.Log("Error : " + e.Message);
            }
        }
    }

    private void Broadcast(string data, ServerClient sC)
    {
        List<ServerClient> sc = new List<ServerClient> { sC };
        Broadcast(data, sc);
    }

    //server read
    private void OnIncomingData(ServerClient c, string data)
    {
        Debug.Log("Server: " + data);
        string[] aData = data.Split('|');

        switch (aData[0])
        {
            case "C-WHO":
                c.clientName = aData[1];
                c.isHost = (aData[2] == "0") ? false : true;
                Broadcast("S-CNCTION|" + c.clientName, clients);
                break;
            case "C-MOV":
                data.Replace('C', 'S');
                Broadcast(data, clients);
                break;
        }
    }
}

public class ServerClient
{
    public string clientName;
    public TcpClient tcp;
    public bool isHost;
    public ServerClient(TcpClient tcp)
    {
        this.tcp = tcp;
    }
}
