using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

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
    [SyncVar] public int _health = 4;

    // HP���� ����, ������ȭ�� ��Ŀ�� ���� �Ǵ�, �� �÷��̾� ���� �Ǵ�
    private void Update()
    {
        
    }

    // �������̵�
    [Command]
    void CommandAtk()   // ������ �� �÷��̾��� ���� ��û
    {

    }

    [Command]
    void RpcOnAtk()     // ������ �� �÷��̾��� ���� �ִϸ��̼� ��� ��û
    {

    }

    // Ŭ�󿡼� ���� �Լ��� ������� �ʵ��� ServerCallback�� �޾���
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        
    }


}
