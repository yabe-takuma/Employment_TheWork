using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleTrollScript : MonoBehaviour
{
    [SerializeField]
    private TitleCameraScript titleCameraScript;
    private Animator animator;
    [SerializeField]
    private int animationCooltime;
    private AnimatorStateInfo stateInfo;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);


    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool isIdleBreakPlaying = stateInfo.IsName("idle_break") && stateInfo.normalizedTime < 1.0f;
        bool hasIdleBreakFinished = stateInfo.IsName("idle_break") && stateInfo.normalizedTime >= 1.0f;

        // アニメーション中でなければクールタイム加算
        if (!isIdleBreakPlaying && titleCameraScript.IsAtDistination())
        {
            animationCooltime++;
        }

        // クールタイムが一定値を超えたらアニメーション再生
        if (animationCooltime > 200)
        {
            animator.SetBool("idle_break", true);
            animationCooltime = 0;
        }

        // アニメーションが終了したらフラグリセット
        if (hasIdleBreakFinished)
        {
            animator.SetBool("idle_break", false);
        }


    }
}
