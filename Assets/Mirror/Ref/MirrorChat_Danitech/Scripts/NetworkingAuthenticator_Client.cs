using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public partial class NetworkingAuthenticator
{
    [SerializeField] LoginPopup _loginPopup;

    [Header("Client Username")]
    public string _playerName;

    public void OnInputValueChanged_SetPlayerName(string username)
    {
        _playerName = username;
        _loginPopup.SetUIOnAuthValueChanged();
    }

    public override void OnStartClient()
    {
    }

    public override void OnStopClient()
    {
    }

    // 클라에서 인증 요청 시 불려짐
    public override void OnClientAuthenticate()
    {
    }

    public void OnAuthResponseMessage(AuthResMsg msg)
    {

    }

}
