using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackStateBehaviour : StateMachineBehaviour
{
    //プレイヤーがアニメーション中に割り込むための変数
    [SerializeField]
    private ProcessCharaAnimEventScript processCharaAnimEvent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //アニメーションする前にデータを格納したり攻撃状態を解除する処理
        processCharaAnimEvent = animator.transform.GetComponent<ProcessCharaAnimEventScript>();
        animator.ResetTrigger("Attack");
       
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //攻撃する時アニメーション中だったら次の攻撃に行く処理
        if(Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("Attack", true);
        }
        //攻撃しているときジャンプすると攻撃をキャンセルする処理
        else if(animator.GetBool("Jump")==true)
        {
            animator.SetBool("Jump", true);
            animator.ResetTrigger("Attack");
        }
        //攻撃用のコライダーを表示するための処理
        processCharaAnimEvent.AttackStart();
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //全ての連続攻撃をしたら攻撃アニメーションを解除する処理
        if(stateInfo.IsName("Attack3"))
        {
            animator.ResetTrigger("Attack");
           
        }
       
        //アタック状態を抜け出す前に武器のコライダーも無効化する
        processCharaAnimEvent.AttackEnd();
        processCharaAnimEvent.StateEnd();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
