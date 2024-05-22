using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkingAuthenticator : NetworkAuthenticator
{
    public struct AuthReqMsg : NetworkMessage
    {
        // 인증을 위해 사용
        // OAuth 같은 걸 사용 시 이 부분에 엑세스 토큰 같은 변수를 추가하면 됨
        public string authUserName;
    }

    public struct AuthResMsg : NetworkMessage
    {
        public byte code;
        public string message;
    }

#region ServerSide
    [UnityEngine.RuntimeInitializeOnLoadMethod]
    static void ResetStatics() 
    {
    }

    public override void OnStartServer()
    {
    }

    public override void OnStopServer()
    {
    }

    public override void OnServerAuthenticate(NetworkConnectionToClient conn)
    {
    }

    public void OnAuthRequestMessage(NetworkConnectionToClient conn)
    {
    }

    IEnumerator DelayedDisconnect(NetworkConnectionToClient conn, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        yield return null;
    }
#endregion

}
