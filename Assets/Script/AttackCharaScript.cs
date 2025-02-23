using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TrollScript;

public class AttackCharaScript : MonoBehaviour
{
    [SerializeField]
    private TrollScript trollScript;
    [SerializeField]
    private Animator trollAnimator;

    [SerializeField]
    private float caunter;
    [SerializeField]
    private Vector3 target;
    [SerializeField]
    private ChaseCharaScript chaseScript;
    [SerializeField]
    private float distance;

    [SerializeField]
    private TrollStatus trollstatus;

    [SerializeField]
    private int hp;
    //ボスの攻撃を終了を検知
    [SerializeField]
    private bool isAttack;
    [SerializeField]
    private ReceiveAttackEventScript receiveAttackEventScript;

    // Start is called before the first frame update
    void Start()
    {
        trollScript = GetComponentInParent<TrollScript>();
        trollAnimator = trollScript.GetComponent<Animator>();
        receiveAttackEventScript = GetComponentInParent<ReceiveAttackEventScript>();
    }

    private void OnTriggerStay(Collider other)
    {
        //target = chaseScript.GetTarget().transform.position;
        //攻撃状態でない時に攻撃(アニメーションが攻撃状態でない時も条件に含める)
        if (other.tag == "Player"
            && trollScript.GetState() != TrollScript.TrollState.attack
            && !trollAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
            && trollScript.GetState() != TrollScript.TrollState.shockwaveAttack
            && !trollAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShockwaveAttack")
            && trollScript.GetState() != TrollScript.TrollState.installation
            && trollScript.GetState() != TrollScript.TrollState.wave
            && trollScript.GetState() != TrollScript.TrollState.explocion
            && trollScript.GetState() != TrollScript.TrollState.continuous
            && trollScript.GetState() != TrollScript.TrollState.charge
            && !trollAnimator.GetCurrentAnimatorStateInfo(0).IsName("Charge")
            && trollScript.GetState()!=TrollScript.TrollState.Dead)
            
        {
            caunter = Random.value;

            distance = Vector3.Distance(trollScript.transform.position, chaseScript.GetTarget().transform.position);
            //ランダムに攻撃を振り分ける
            if (distance > 9.0f && distance < 10.0f && isAttack==false&& trollstatus.GetHp()>=trollstatus.GetMaxHp()/2)
            {
                trollScript.SetState(TrollScript.TrollState.attack, other.transform);
                trollScript.SetState(TrollScript.TrollState.chase, other.transform);
                Debug.Log("攻撃1");
                isAttack = true;
            }
            else if (distance > 0.0f && distance < 6.0f && isAttack == false && trollstatus.GetHp() >= trollstatus.GetMaxHp() / 2)
            {
                trollScript.SetState(TrollScript.TrollState.shockwaveAttack, other.transform);
                trollScript.SetState(TrollScript.TrollState.chase, other.transform);
                Debug.Log("攻撃2");
                isAttack = true;
            }
            else if (distance > 6.0f && distance < 9.0f && isAttack == false && trollstatus.GetHp() >= trollstatus.GetMaxHp() / 2)
            {
                trollScript.SetState(TrollScript.TrollState.wave, other.transform);
                trollScript.SetState(TrollScript.TrollState.chase, other.transform);
                Debug.Log("攻撃2");
                isAttack = true;
            }
            else if(distance>3.0f&&distance<5.0f && isAttack == false && trollAnimator.GetBool("Explocion") == false && trollstatus.GetHp() <= trollstatus.GetMaxHp() / 2)
            {
                trollScript.SetState(TrollScript.TrollState.installation, other.transform);
                trollScript.SetState(TrollScript.TrollState.chase, other.transform);
                Debug.Log("攻撃3");
                isAttack = true;
            }

            else if (distance > 5.0f && distance < 8.0f && isAttack == false && trollstatus.GetHp() <= trollstatus.GetMaxHp() / 2)
            {
                trollScript.SetState(TrollScript.TrollState.explocion, other.transform);
                trollScript.SetState(TrollScript.TrollState.chase, other.transform);
                isAttack = true;
            }

            else if (distance > 8.0f && distance < 15.0f && isAttack == false && trollAnimator.GetBool("Explocion") == false && trollstatus.GetHp() <= trollstatus.GetMaxHp() / 2)
            {
                trollScript.SetState(TrollScript.TrollState.continuous, other.transform);
                trollScript.SetState(TrollScript.TrollState.chase, other.transform);
                Debug.Log("攻撃3");
                isAttack = true;
            }
            if(receiveAttackEventScript.GetIsAttack()==false)
            {
                isAttack = false;
            }
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        hp = trollstatus.GetHp();
    }
}
