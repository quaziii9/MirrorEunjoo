using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

// ������ ������ ������Ʈ
public class NetSpawnedObject : NetworkBehaviour
{
    [Header("Components")]
    public NavMeshAgent NavAgent_Player;        // �÷��̾� ���� �ӵ� �Ǵܿ����� ���
    public Animator Animator_Player;            // �÷��̾� �ִϸ��̼� �����
    public TextMesh TextMesh_HealthBar;         // �÷��̾� HP ǥ��� �ؽ�Ʈ �޽�
    public Transform Transform_Player;          // �÷��̾� ��ġ, ȸ�� �����

    [Header("Movement")]
    public float _rotationSpeed = 100.0f;       // ȸ�� �ӵ� ��

    [Header("Attack")]
    public KeyCode _attKey = KeyCode.Space;     
    public GameObject Prefab_AtkObject;         // ������ �� ������ ������ ������Ʈ
    public Transform Transform_AtkSpawnPos;     // ������ ������Ʈ ������ ��ġ

    [Header("Stats Server")]
    [SyncVar] public int _health = 4;           // �÷��̾��� ü�� , ������ Ŭ���̾�Ʈ ���� ����ȭ


    // HP���� ����, ������ȭ�� ��Ŀ�� ���� �Ǵ�, �� �÷��̾� ���� �Ǵ�
    private void Update()
    {
        SetHealthBarOnUpdate(_health);

        if(CheckIsFocuseOnUpdate() == false)
        {
            return;
        }
        CheckIsLocalPlayerOnUpdate();
    }

    // HP ���� ���ſ� �Լ�
    private void SetHealthBarOnUpdate(int health)
    {
        TextMesh_HealthBar.text = new string('-', health);
    }

    // ������ ȭ�� Ȱ��ȭ(����)������ �Ǵ��ϴ� �Լ�
    private bool CheckIsFocuseOnUpdate()
    {
        return Application.isFocused;
    }

    // �� �÷��̾� ���� ó�� �Լ�
    private void CheckIsLocalPlayerOnUpdate()
    {
        // isLocalPlayer �˻縦 �����ָ� ���� ���� ��� ������Ʈ�� �Ȱ��� ������ �����
        if (isLocalPlayer == false)
            return;

        // ȸ��
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(0, horizontal * _rotationSpeed * Time.deltaTime, 0);

        // �̵�
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

    // �������̵�,
    [Command]   // Ŭ���̾�Ʈ���� ȣ��Ǿ� �������� ����
    void CommandAtk()   // ������ �� �÷��̾��� ���� ��û
    {
        // Ŭ�󿡼� ���� ��û �� �������̵忡�� [Command] �Լ��� CommantAtk����
        // NetworkServer.Spawn() �Լ��� Ư�� ���� ������Ʈ�� ����
        GameObject atkObjectForSpawn = Instantiate(Prefab_AtkObject, Transform_AtkSpawnPos.transform.position, Transform_AtkSpawnPos.transform.rotation);
        NetworkServer.Spawn(atkObjectForSpawn);

        RpcOnAtk();
    }

    // RpcOnAtk()�� �������� ��� Ŭ���̾�Ʈ���� �˷��ִ� ��ε�ĳ������ �Լ��̹Ƿ�[ClientRpc]�� ����
    // �׷��� ��� Ŭ���̾�Ʈ���� �ش� �Լ��� ȣ��ȴ�
    [ClientRpc]         // �������� ȣ��Ǿ Ŭ���̾�Ʈ���� ����
    void RpcOnAtk()     // ������ �� �÷��̾��� ���� �ִϸ��̼� ��� ��û
    {
        Animator_Player.SetTrigger("Atk");
    }

    // Ŭ�󿡼� ���� �Լ��� ������� �ʵ��� ServerCallback�� �޾���

    // Atk�� �ǰݵ� ������Ʈ ���� ó���� ���������� �� �� �ְ� [ServerCallback] ����
    // health�� ���ҵǸ�, [SyncVar]�� ���� �ش� ���� ����ȭ��
    // health�� ���� �÷��̾�� NetworkServer.Destroy�� ������Ʈ ����

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

    // ���� �÷��̾� ȸ��ó��
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
