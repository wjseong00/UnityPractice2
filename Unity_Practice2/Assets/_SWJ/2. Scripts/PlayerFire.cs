using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
   
    public GameObject bullet;
    public GameObject grenades;
    public GameObject firePoint;
    public float throwPower = 10.0f;
    //EnemyFSM ef;
    private void Start()
    {
        //ef = GameObject.Find("Enemy").GetComponent<EnemyFSM>();
    }
    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    private void Fire()
    {
        //마우스왼쪽버튼 클릭시 레이캐스트로 총알발사
        if (Input.GetMouseButtonDown(0))
        {
            //RaycastHit hitInfo;
            //Ray ray = new Ray(this.transform.position, transform.forward);
            //if (Physics.Raycast(ray, out hitInfo))
            //{
            //    
            //    Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hitInfo.normal);
            //    GameObject spark = Instantiate(bullet);
            //    spark.transform.position = hitInfo.point;
            //    spark.transform.rotation = rot;
            //    Destroy(spark, 1f);
            //}
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo;
            //레이랑 충돌했냐?
            int layer = gameObject.layer;
            layer = 1 << 8 | 1 << 9 | 1 << 12;
            if (Physics.Raycast(ray, out hitInfo,100f,layer))
            {
                print("충돌오브젝트 : " + hitInfo.collider.name);

                //내총알에 충돌했으니 몬스터 체력을 깎기
                EnemyFSM enemy = hitInfo.collider.GetComponent<EnemyFSM>();
                enemy.hitDamage(10);
                //hitInfo.collider.gameObject.GetComponent<EnemyFSM>().hitDamage(10);
                //hitInfo.transform.GetComponent<EnemyFSM>().hitDamage(10);

                if(hitInfo.collider.name =="Enemy")
                {
                    //ef.Hit();
                }
                
                //충돌 자리에 오브젝트 생성
                //충돌파편 이펙트 생성
                GameObject spark = Instantiate(bullet,hitInfo.transform);
                //부딪힌 지점 hitINfo 안에 정보들이 담겨 있다
                spark.transform.position = hitInfo.point;
                //파편이펙트
                //파편이 부딪힌 지점이 향하는 방향으로 튀게 해줘야 한다.
                spark.transform.forward = hitInfo.normal;
            }
            //레이어 마스크 사용 충돌처리
            //유니티 내부적으로 속도향상을 위해 비트연산 처리가 된다
            //총 32비트를 사용하기때문에 레이어도 32개까지 추라 가능함
           
            //if(Physics.Raycast(ray, out hitInfo,100,layer)) //layer만 충돌
            //if (Physics.Raycast(ray, out hitInfo, 100, ~layer)) //layer만 충돌제외
            {
                //if(플레이어라면 충돌)
                //if(에너미라면 충돌)
                //if(보스라면 충돌)
                //이런식이면 if문이 많이 들어가면
                //성능이 조금이라도 떨어질 수밖에 없다
                //비트연산은 성능 최적화에 도움이 된다.
            }
        }
        //마우스우측버튼 클릭시 수류탄투척 하기
        if (Input.GetMouseButtonDown(1))
        {
            //폭탄생성
            GameObject bomb = Instantiate(grenades);
            bomb.transform.position = firePoint.transform.position;
            //폭탄은 플레이어가 던지기 때문에
            //폭탄의 리지드바디를 이용해서 던지면 된다
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            //전방으로 물리적인 힘을 가한다.
            //rb.AddForce(Camera.main.transform.forward * throwPower,ForceMode.Impulse);
            //ForceMode.Impulse => 순간적인 힘을 가한다(질량의 영향을 받음)
            //ForceMode.Force   => 연속적인 힘을 가한다 ( 질량의 영향을 받음)
            //ForceMode.Acceleration    => 연속적인 힘을 가한다(질량 영향 안받음)
            //ForceMode.VelocityChange => 순간적인 힘을 가한다(질량 영향안받음

            //45도 각도로 발사
            //각도를 주려면 어떻게 해야 할까(벡터의 덧셈)
            Vector3 dir = Camera.main.transform.forward + Camera.main.transform.up;
            dir.Normalize();
            rb.AddForce(dir * throwPower, ForceMode.Impulse);
        }
    }
}
