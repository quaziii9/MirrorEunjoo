using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ChattingUI : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] Text Text_ChatHistory;
    [SerializeField] Scrollbar ScrollBar_Chat;
    [SerializeField] InputField Input_ChatMsg;
    [SerializeField] Button Btn_Send;

    public override void OnStartServer()
    {
    }

    public override void OnStartClient()
    {
    }
}
