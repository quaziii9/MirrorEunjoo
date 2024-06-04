using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

// 서버에 스폰될 오브젝트
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
    [SyncVar] public int _health = 4;           // 플레이어의 체력 , 서버와 클라이언트 간에 동기화


    // HP상태 갱신, 윈도우화면 포커싱 여부 판단, 내 플레이어 여부 판단
    private void Update()
    {
        SetHealthBarOnUpdate(_health);

        if(CheckIsFocuseOnUpdate() == false)
        {
            return;
        }
        CheckIsLocalPlayerOnUpdate();
    }

    // HP 상태 갱신용 함수
    private void SetHealthBarOnUpdate(int health)
    {
        TextMesh_HealthBar.text = new string('-', health);
    }

    // 윈도우 화면 활성화(선택)중인지 판단하는 함수
    private bool CheckIsFocuseOnUpdate()
    {
        return Application.isFocused;
    }

    // 내 플레이어 관련 처리 함수
    private void CheckIsLocalPlayerOnUpdate()
    {
        // isLocalPlayer 검사를 안해주면 게임 내의 모든 오브젝트가 똑같은 로직이 적용됨
        if (isLocalPlayer == false)
            return;

        // 회전
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(0, horizontal * _rotationSpeed * Time.deltaTime, 0);

        // 이동
        float vertical = Input.GetAxis("Vertical");
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        NavAgent_Player.velocity = forward * Mathf.Max(vertical, 0) * NavAgent_Player.speed;
        Animator_Player.SetBool("Moving", NavAgent_Player.velocity != Vector3.zero);

        if(Input.GetKeyDown(_attKey))
        {
            CommandAtk();
        }

        RotateLocalPlayer();
    }

    // 서버사이드,
    [Command]   // 클라이언트에서 호출되어 서버에서 수행
    void CommandAtk()   // 서버에 내 플레이어의 공격 요청
    {
        // 클라에서 공격 요청 시 서버사이드에서 [Command] 함수인 CommantAtk에서
        // NetworkServer.Spawn() 함수로 특정 공격 오브젝트를 생성
        GameObject atkObjectForSpawn = Instantiate(Prefab_AtkObject, Transform_AtkSpawnPos.transform.position, Transform_AtkSpawnPos.transform.rotation);
        NetworkServer.Spawn(atkObjectForSpawn);

        RpcOnAtk();
    }

    // RpcOnAtk()은 서버에서 모든 클라이언트에게 알려주는 브로드캐스팅형 함수이므로[ClientRpc]를 붙임
    // 그러면 모든 클라이언트에서 해당 함수가 호출된다
    [ClientRpc]         // 서버에서 호출되어서 클라이언트에서 수행
    void RpcOnAtk()     // 서버에 내 플레이어의 공격 애니메이션 재생 요청
    {
        Animator_Player.SetTrigger("Atk");
    }

    // 클라에서 다음 함수가 실행되지 않도록 ServerCallback을 달아줌

    // Atk에 피격된 오브젝트 관련 처리를 서버에서만 할 수 있게 [ServerCallback] 붙임
    // health가 감소되면, [SyncVar]에 의해 해당 값이 동기화됨
    // health가 없는 플레이어는 NetworkServer.Destroy로 오브젝트 제거

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        var atkGenObject = other.GetComponent<NetSpawnedSubObject>();
        if (atkGenObject != null)
        {
            _health--;
            if(_health ==0)
            {
                NetworkServer.Destroy(this.gameObject);
            }
        }
            

    }

    // 로컬 플레이어 회전처리
    void RotateLocalPlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            Debug.DrawLine(ray.origin, hit.point);
            Vector3 lookRotate = new Vector3(hit.point.x, Transform_Player.position.y, hit.point.z);
            Transform_Player.LookAt(lookRotate);
        }
    }

}
