using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static TrollScript;

public class AttackCharaScript : MonoBehaviour
{
    [SerializeField]
    private TrollScript trollScript;  //トロルの移動や攻撃全般が書かれているスクリプト
    [SerializeField]
    private Animator trollAnimator;  //トロルのアニメーション全般

    [SerializeField]
    private float caunter;  //攻撃一つ一つを判断するための変数

    [SerializeField]
    private TrollStatus trollstatus;  //このスクリプトの中にトロルのHPがありHPによって攻撃を追加したいため

    [SerializeField]
    private int cooltime;  //すぐに攻撃しないように少しクールタイムを入れる

    // Start is called before the first frame update
    void Start()
    {
        trollScript = GetComponentInParent<TrollScript>();
        trollAnimator = trollScript.GetComponent<Animator>();
        caunter = 0; //攻撃を最初から行ってほしいので0にします。
    }

    private void OnTriggerStay(Collider other)
    {
        //攻撃状態でない時に攻撃(アニメーションが攻撃状態でない時も条件に含める)
        //それぞれ攻撃と衝撃波攻撃と爆発攻撃と足踏み攻撃の時に処理されるようになっています。
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
       
    }

    void TrollAttackState(GameObject other)
    {
        cooltime++;
        //ボスの攻撃を順番に振り分ける
        if (caunter == 0 && cooltime >= 100)  //衝撃波攻撃
        {
            trollScript.SetState(TrollScript.TrollState.shockwaveAttack, other.transform);
            trollScript.SetState(TrollScript.TrollState.chase, other.transform);
            Debug.Log("攻撃2");
            caunter = 1;
            cooltime = 0;
        }
        else if (caunter == 1 && cooltime >= 100 && trollstatus.GetHp() >= trollstatus.GetMaxHp() / 2)  //足踏み攻撃
        {
            trollScript.SetState(TrollScript.TrollState.wave, other.transform);
            trollScript.SetState(TrollScript.TrollState.chase, other.transform);
            Debug.Log("足踏み攻撃");
            caunter = 0;
            cooltime = 0;
        }
        else if (caunter == 1 && cooltime >= 100 && trollstatus.GetHp() <= trollstatus.GetMaxHp() / 2)  //連続衝撃波攻撃
        {
            trollScript.SetState(TrollScript.TrollState.installation, other.transform);
            trollScript.SetState(TrollScript.TrollState.chase, other.transform);
            Debug.Log("攻撃3");
            caunter = 2;
            cooltime = 0;
        }

        else if (caunter == 2 && cooltime >= 100 && trollstatus.GetHp() <= trollstatus.GetMaxHp() / 2)  //爆発攻撃
        {
            trollScript.SetState(TrollScript.TrollState.explocion, other.transform);
            trollScript.SetState(TrollScript.TrollState.chase, other.transform);
            caunter = 3;
            cooltime = 0;
        }

        else if (caunter == 3 && cooltime >= 100 && trollstatus.GetHp() <= trollstatus.GetMaxHp() / 2)  //足踏み攻撃(HPが半分の時も行動パターンに入れたいため)
        {
            trollScript.SetState(TrollScript.TrollState.wave, other.transform);
            trollScript.SetState(TrollScript.TrollState.chase, other.transform);
            Debug.Log("攻撃3");
            caunter = 0;
            cooltime = 0;
        }
    }

}
