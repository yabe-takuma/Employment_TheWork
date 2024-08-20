using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCharaScript : MonoBehaviour
{
    [SerializeField]
    private TrollScript trollScript;
    [SerializeField]
    private Animator trollAnimator;

    // Start is called before the first frame update
    void Start()
    {
        trollScript = GetComponentInParent<TrollScript>();
        trollAnimator = trollScript.GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        //攻撃状態でない時に攻撃(アニメーションが攻撃状態でない時も条件に含める)
        if(other.tag == "Player"
            && trollScript.GetState() != TrollScript.TrollState.attack
            && !trollAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
            && trollScript.GetState() != TrollScript.TrollState.shockwaveAttack
            && !trollAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShockwaveAttack"))
        {
            //ランダムに攻撃を振り分ける
            if(Random.value<=0.5f)
            {
                trollScript.SetState(TrollScript.TrollState.attack, other.transform);
                trollScript.SetState(TrollScript.TrollState.chase, other.transform);
                Debug.Log("攻撃1");
            }
            else
            {
                trollScript.SetState(TrollScript.TrollState.shockwaveAttack, other.transform);
                trollScript.SetState(TrollScript.TrollState.chase, other.transform);
                Debug.Log("攻撃2");
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
