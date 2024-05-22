using UnityEngine;
using Mirror;

public class NetworkingManager : NetworkManager
{
    public void OnInputValueChanged_SetHostName(string hostName)
    {
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
    }
}
