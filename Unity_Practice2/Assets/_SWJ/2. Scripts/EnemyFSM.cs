using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 유한상태머신
public class EnemyFSM : MonoBehaviour
{
    GameObject target;

    //몬스터 상태 이넘문
    enum EnemyState
    {
        Idle, Move, Attack, Return, Damaged, Die
    }
    EnemyState state;//몬스터 상태 변수
    EnemyState preState;

    float distance;     //플레이어와 몬스터의 거리
    float moveSpeed=5f;
    int enemyHp = 5;
                        /// 유용한 기능
    #region "Idle 상태에 필요한 변수들"

    #endregion

    #region "Move 상태에 필요한 변수들"

    #endregion

    #region "Attack 상태에 필요한 변수들"

    #endregion

    #region "Return 상태에 필요한 변수들"
    Vector3 preLocate;
    float backDistance;
    #endregion

    #region "Damage 상태에 필요한 변수들"

    #endregion

    #region "Die 상태에 필요한 변수들"

    #endregion


    void Start()
    {
        //몬스터 상태 초기화
        state = EnemyState.Idle;
        //플레이어 설정
        target = GameObject.Find("Player").gameObject;
    }

    void Update()
    {
        distance = Vector3.Distance(target.transform.position, transform.position);
        backDistance = Vector3.Distance(preLocate, transform.position);
        if (backDistance > 30)
        {
            state = EnemyState.Return;
        }
        Debug.Log(state);
        //상태에 따른 행동처리
        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                Damage();
                break;
            case EnemyState.Die:
                Die();
                break;
            default:
                break;
        }

    }//end of void Update()
    //기본상태
    private void Idle()
    {
        
        
        
        if (distance <20f)
        {
            preLocate = this.transform.position;
            Debug.Log("Move");
            state = EnemyState.Move;
        }
        //1. 플레이어와 일정범위가 되면 이동상태로 변경(탐지범위)
        //- 플레이어 찾기 (GameObject.Find("Player")
        //- 일정거리 20미터 (거리비교 : Distance, magnitude 아무거나) 
        //- 상태변경
        //- 상태전환 출력
    }
    //이동상태
    private void Move()
    {

        //1. 플레이어를 향해 이동 후 공격범위 안에 들어오면 공격상태로 변경
        //2. 플레이어를 추격하더라도 처음위치에서 일정범위를 넘어가면 리턴상태로 변경 
        //- 플레이어 처럼 캐릭터컨트롤러를 이용하기
        //- 공격범위 1미터
        //- 상태변경
        //- 상태전환 출력
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();
        dir.y = 0;
        transform.forward= Vector3.Slerp(transform.forward, dir, Time.deltaTime * moveSpeed);
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        
        if(distance<2)
        {
            Debug.Log("Attack");
            state = EnemyState.Attack;
        }
        

    }
    //공격상태
    private void Attack()
    {

        //1. 플레이어를 향해 이동 후 공격범위 안에 들어오면 일정한 시간 간격으로 플레이어 공격
        //2. 플레이어가 공격범위를 벗어나면 이동상태(재추격)
        //- 공격범위 1미터
        //- 상태변경
        //- 상태전환 출력
        if(distance>4)
        {
            state = EnemyState.Move;
        }

    }
    //복귀상태
    private void Return()
    {
        //1. 몬스터가 플레이어를 추격하더라도 처음 위치에서 일정 범위를 벗어나면 다시 돌아옴
        //- 처음위치에서 일정범위 30미터
        //- 상태변경
        //- 상태전환 출력
        
        Vector3 dir = preLocate - transform.position;
        dir.Normalize();
        dir.y = 0;
        transform.forward = Vector3.Slerp(transform.forward, dir, Time.deltaTime * moveSpeed);
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        if (backDistance<1)
        {
            state = EnemyState.Idle;
        }

    }
    //피격상태 (Any State)
    private void Damage()
    {
        
        //코루틴을 사용하자
        //1. 몬스터 체력이 1이상
        //2. 다시 이전상태로 변경
        //- 상태변경
        //- 상태전환 출력
    }
    //죽음상태 (Any State)
    private void Die()
    {
        //코루틴을 사용하자
        //1. 체력이 0이하
        //2. 몬스터 오브젝트 삭제
        //- 상태변경
        //- 상태전환 출력 (죽었다)
    }
    public void Hit()
    {
        enemyHp--;
        if (enemyHp>=1)
        {
            //preState = state;
            state = EnemyState.Damaged;
            StartCoroutine(hitDameage());
        }
        else if (enemyHp <= 0)
        {

            StartCoroutine(enemyDie());
        }
    }
    IEnumerator hitDameage()
    {
        Debug.Log("Damage");
        
        Debug.Log(enemyHp);
        
        yield return new WaitForSeconds(2f);
        state = EnemyState.Idle;
        
    }
    IEnumerator enemyDie()
    {
        Debug.Log("Die");
        state = EnemyState.Die;
        yield return null;
    }
}
