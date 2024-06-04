using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// �ٸ� ��ü�κ��� NetworkServer.Spwan�Ǵ� ������Ʈ
public class NetSpawnedSubObject : NetworkBehaviour
{
    public float _destoryAfter = 2.0f;      // �̺�Ʈ�� �߻��ϰ� �ش� ������Ʈ�� �ڵ� �ı��� �ð�
    public float _force = 1000;             // ������ٵ𿡼� AddForce�� ��

    public Rigidbody RigidBody_SubObj;


    // �������� �ش� ��ü�� Ȱ��ȭ�� �� ȣ��(Unity�� Start�� ����) 
    // ���� ���̵忡���� ȣ��
    public override void OnStartServer()    
    {
        Invoke(nameof(DestroySelf), _destoryAfter); // Ȱ��ȭ �� ���� �ð��� ������ �ڵ� �ı� �ǵ���
    }

    // Ȱ��ȭ �� ���� �̵��Ǵ� ó����
    private void Start()
    {
        RigidBody_SubObj.AddForce(transform.forward * _force);
    }

    [Server]
    private void DestroySelf() // Ư�� ������ �Ǹ� NetworkServer.Destroy�� ������Ʈ ����
    {
        NetworkServer.Destroy(this.gameObject);
    }

    [ServerCallback]

    private void OnTriggerEnter(Collider other) // �������̵忡�� �浹 �� �����ϱ�
    {
        DestroySelf();
    }
}
