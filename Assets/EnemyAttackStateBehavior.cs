using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackStateBehavior : StateMachineBehaviour
{

    private ProcessEnemyAnimEventScript processEnemyAnimEvent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        processEnemyAnimEvent = animator.transform.GetComponent<ProcessEnemyAnimEventScript>();
        processEnemyAnimEvent.AttackStart();
        Debug.Log("攻撃はじめ");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        processEnemyAnimEvent = animator.transform.GetComponent<ProcessEnemyAnimEventScript>();
        processEnemyAnimEvent.AttackEnd();
        processEnemyAnimEvent.StateEnd();
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);
        Debug.Log("攻撃終了");
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
