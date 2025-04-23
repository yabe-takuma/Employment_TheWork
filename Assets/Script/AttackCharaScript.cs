using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
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
    [SerializeField]
    private ReceiveAttackEventScript receiveAttackEventScript;
    [SerializeField]
    private int cooltime;

    // Start is called before the first frame update
    void Start()
    {
        trollScript = GetComponentInParent<TrollScript>();
        trollAnimator = trollScript.GetComponent<Animator>();
        receiveAttackEventScript = GetComponentInParent<ReceiveAttackEventScript>();
        caunter = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        //攻撃状態でない時に攻撃(アニメーションが攻撃状態でない時も条件に含める)
        if (other.tag == "Player"
            && trollScript.GetState() != TrollScript.TrollState.attack
            && !trollAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
            && trollScript.GetState() != TrollScript.TrollState.shockwaveAttack
            && !trollAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShockwaveAttack")
            && trollScript.GetState() != TrollScript.TrollState.installation
            && !trollScript.GetInstallation()
            && trollScript.GetState() != TrollScript.TrollState.wave
            && !trollAnimator.GetCurrentAnimatorStateInfo(0).IsName("WaveAttack")
            && trollScript.GetState() != TrollScript.TrollState.continuous
            && trollScript.GetState() != TrollScript.TrollState.explocion
            && !trollScript.GetExplocion()
            && trollScript.GetState() !=TrollScript.TrollState.Dead)
            
        {
            TrollAttackState(other.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        hp = trollstatus.GetHp();
    }

    void TrollAttackState(GameObject other)
    {
        cooltime++;
        //ボスの攻撃を順番に振り分ける
        if (caunter == 0 && cooltime >= 100)
        {
            trollScript.SetState(TrollScript.TrollState.shockwaveAttack, other.transform);
            trollScript.SetState(TrollScript.TrollState.chase, other.transform);
            Debug.Log("攻撃2");
            caunter = 1;
            cooltime = 0;
        }
        else if (caunter == 1 && cooltime >= 100 && trollstatus.GetHp() >= trollstatus.GetMaxHp() / 2)
        {
            trollScript.SetState(TrollScript.TrollState.wave, other.transform);
            trollScript.SetState(TrollScript.TrollState.chase, other.transform);
            Debug.Log("足踏み攻撃");
            caunter = 0;
            cooltime = 0;
        }
        else if (caunter == 1 && cooltime >= 100 && trollstatus.GetHp() <= trollstatus.GetMaxHp() / 2)
        {
            trollScript.SetState(TrollScript.TrollState.installation, other.transform);
            trollScript.SetState(TrollScript.TrollState.chase, other.transform);
            Debug.Log("攻撃3");
            caunter = 2;
            cooltime = 0;
        }

        else if (caunter == 2 && cooltime >= 100 && trollstatus.GetHp() <= trollstatus.GetMaxHp() / 2)
        {
            trollScript.SetState(TrollScript.TrollState.explocion, other.transform);
            trollScript.SetState(TrollScript.TrollState.chase, other.transform);
            caunter = 3;
            cooltime = 0;
        }

        else if (caunter == 3 && cooltime >= 100 && trollstatus.GetHp() <= trollstatus.GetMaxHp() / 2)
        {
            trollScript.SetState(TrollScript.TrollState.continuous, other.transform);
            trollScript.SetState(TrollScript.TrollState.chase, other.transform);
            Debug.Log("攻撃3");
            caunter = 0;
            cooltime = 0;
        }
    }

}
