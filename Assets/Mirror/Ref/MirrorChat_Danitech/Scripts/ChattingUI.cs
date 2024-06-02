using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Mirror.Examples.Chat;
using System.Collections.Generic;

public class ChattingUI : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] Text Text_ChatHistory;
    [SerializeField] Scrollbar ScrollBar_Chat;
    [SerializeField] InputField Input_ChatMsg;
    [SerializeField] Button Btn_Send;

    // 서버 온리 - 연결된 플레이어들 이름
    internal static readonly Dictionary<NetworkConnectionToClient, string> _connectedNameDic = new Dictionary<NetworkConnectionToClient, string>();
    public override void OnStartServer()
    {
        _connectedNameDic.Clear();
    }

    public override void OnStartClient()
    {
        Text_ChatHistory.text = string.Empty;
    }

    // [Command] 라는 어트리뷰트를 이용해 클라 -> 서버로 특정 기능 수행을 요청
    // 예제의 경우는 CommandSendMsg라는 함수를 통해 서버에 메세지 송신
    [Command(requiresAuthority = false)]
    void CommandSendMsg(string msg, NetworkConnectionToClient sender = null)
    {
        if(!_connectedNameDic.ContainsKey(sender))
        {
            // -GetComponent로 Player를 가져오고, Player의 playerName 가져옴
            // -가져온 playerName을 Dictionary에 보관
            // - Player 자료형을 가져올 수 있도록 using Mirror.Examples.Chat을 선언

            var player = sender.identity.GetComponent<Player>();
            var playerName = player.playerName;
            _connectedNameDic.Add(sender, playerName);
        }

            // -CommandSendMsg에 OnRecvMessage 함수 호출해 브로드캐스팅 부분 추가
        if (!string.IsNullOrWhiteSpace(msg))
        {
            var senderName = _connectedNameDic[sender];
            OnRecvMessage(senderName, msg.Trim());
        }
    }

    // -서버사이드에서 모든 클라이언트에게 특정 함수를 실행시킬 수 있도록[ClientRpc]를 붙임
    // - [ClientRpc]를 붙인 OnRecvMessage() 함수 추가
    // -클라들이 특정 시점에 모드 받기 때문에 "On"을 붙여 수동형 함수라는 것을 명시
    [ClientRpc]
    void OnRecvMessage(string senderName, string msg)
    {

    }

    public void OnClick_SendMsg()
    {
        var currentChatMsg = Input_ChatMsg.text;
        if(!string.IsNullOrWhiteSpace(Input_ChatMsg.text))
        {
            CommandSendMsg(currentChatMsg.Trim());
        }
    }
}
