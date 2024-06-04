using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class NetSpawnedObject : NetworkBehaviour
{
    [Header("Components")]
    public NavMeshAgent NavAgent_Player;        // 플레이어 현재 속도 판단용으로 사용
    public Animator Animator_Player;            // 플레이어 애니메이션 제어용
    public TextMesh TextMesh_HealthBar;         // 플레이어 HP 표기용 텍스트 메쉬
    public Transform Transform_Player;          // 플레이어 위치, 회전 제어용

    [Header("Movement")]
    public float _rotationSpeed = 100.0f;       // 회전 속도 값

    [Header("Attack")]
    public KeyCode _attKey = KeyCode.Space;     
    public GameObject Prefab_AtkObject;         // 공격할 때 스폰할 데미지 오브젝트
    public Transform Transform_AtkSpawnPos;     // 데미지 오브젝트 스폰용 위치

    [Header("Stats Server")]
    [SyncVar] public int _health = 4;

    // HP상태 갱신, 윈도우화면 포커싱 여부 판단, 내 플레이어 여부 판단
    private void Update()
    {
        
    }

    // 서버사이드
    [Command]
    void CommandAtk()   // 서버에 내 플레이어의 공격 요청
    {

    }

    [Command]
    void RpcOnAtk()     // 서버에 내 플레이어의 공격 애니메이션 재생 요청
    {

    }

    // 클라에서 다음 함수가 실행되지 않도록 ServerCallback을 달아줌
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        
    }


}
