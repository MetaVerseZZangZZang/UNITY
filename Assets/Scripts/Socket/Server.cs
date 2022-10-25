using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Server : MonoBehaviour
{

    public static Server instance;


    #region private members
    private TcpListener tcpListener;
    private Thread tcpListenerThread;
    private TcpClient connectedTcpClient;
    #endregion

    void Awake()
    {
        Debug.Log("Start Server");
        instance = this;

        // Start TcpServer background thread
        tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequest));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Runs in background TcpServerThread; Handles incomming TcpClient requests
    private void ListenForIncommingRequest()
    {
        try
        {
            // Create listener on 192.168.0.2 port 50001
            tcpListener = new TcpListener(IPAddress.Parse("192.168.0.11"), 50001);
            tcpListener.Start();
            Debug.Log("Server is listening");

            while (true)
            {
                using (connectedTcpClient = tcpListener.AcceptTcpClient())
                {
                    // Get a stream object for reading
                    using (NetworkStream stream = connectedTcpClient.GetStream())
                    {
                        // Read incomming stream into byte array.
                        do
                        {
                            // TODO
                        } while (true);
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("SocketException " + socketException.ToString());
        }
    }
}

