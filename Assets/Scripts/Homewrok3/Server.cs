using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    private const int MAX_CONNECTION = 10;
    private int port = 5805;
    private int hostID;
    private int reliableChannel;
    private bool isStarted = false;
    private byte error;
    List<int> connectionIDs = new List<int>();
    private Dictionary<int, string> connectionNames = new Dictionary<int, string>();

    private void SetConnectionName(int id, string name)
    {
        connectionNames.Add(id, name);
    }

    private string GetConnectionName(int id)
    {
        if (connectionNames.ContainsKey(id))
        {
            return connectionNames[id];
        }
        else
        {
            return id.ToString();
        }
    }

    public void StartServer()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Reliable);
        HostTopology topology = new HostTopology(cc, MAX_CONNECTION);
        hostID = NetworkTransport.AddHost(topology, port);
        isStarted = true;
    }

    public void ShutDownServer()
    {
        if (!isStarted) return;
        NetworkTransport.RemoveHost(hostID);
    }
}