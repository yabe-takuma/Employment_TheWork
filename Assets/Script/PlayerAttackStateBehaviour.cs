using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackStateBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private ProcessCharaAnimEventScript processCharaAnimEvent;

   

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        processCharaAnimEvent = animator.transform.GetComponent<ProcessCharaAnimEventScript>();
        
        animator.ResetTrigger("Attack");
       
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Input.GetButtonDown("Fire3"))
        {
            animator.SetBool("Attack", true);
        }
        processCharaAnimEvent.AttackStart();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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
