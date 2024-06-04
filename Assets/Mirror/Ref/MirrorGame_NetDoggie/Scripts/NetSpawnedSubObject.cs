using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// 다른 객체로부터 NetworkServer.Spwan되는 오브젝트
public class NetSpawnedSubObject : NetworkBehaviour
{
    public float _destoryAfter = 2.0f;      // 이벤트가 발생하고 해당 오브젝트가 자동 파괴될 시간
    public float _force = 1000;             // 리지드바디에서 AddForce용 값

    public Rigidbody RigidBody_SubObj;


    // 서버에서 해당 객체가 활성화될 때 호출(Unity의 Start와 같음) 
    // 서버 사이드에서만 호출
    public override void OnStartServer()    
    {
        Invoke(nameof(DestroySelf), _destoryAfter); // 활성화 시 일정 시간이 지나면 자동 파괴 되도록
    }

    // 활성화 시 전방 이동되는 처리용
    private void Start()
    {
        RigidBody_SubObj.AddForce(transform.forward * _force);
    }

    [Server]
    private void DestroySelf() // 특정 시점이 되면 NetworkServer.Destroy로 오브젝트 제거
    {
        NetworkServer.Destroy(this.gameObject);
    }

    [ServerCallback]

    private void OnTriggerEnter(Collider other) // 서버사이드에서 충돌 시 제거하기
    {
        DestroySelf();
    }
}
