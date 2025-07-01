using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayerScript : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;
    private Vector3 velocity;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private TitleCameraScript titleCameraScript;
    [SerializeField]
    private int animationCooltime;
    private AnimatorStateInfo stateInfo;

    private bool isInput;
    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0, -1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
        //プレイヤーが指定した座標まで自動で向かう処理
        if (characterController.isGrounded)
        {
            if (transform.position.z < 192)
            {
                velocity.z = 1.0f;
                animator.SetFloat("Speed", velocity.magnitude);
            }
            else
            {
                velocity.z = 0.0f;
                animator.SetFloat("Speed", 0f);
            }
        }
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool isIdleBreakPlaying = stateInfo.IsName("Action") && stateInfo.normalizedTime < 1.0f;
        bool hasIdleBreakFinished = stateInfo.IsName("Action") && stateInfo.normalizedTime >= 1.0f;

        // アニメーション中でなければクールタイム加算
        if (!isIdleBreakPlaying && titleCameraScript.IsAtDistination())
        {
            animationCooltime++;
        }

        // クールタイムが一定値を超えたらアニメーション再生
        if (animationCooltime > 300)
        {
            animator.SetBool("Action", true);
            animationCooltime = 0;
        }

        // アニメーションが終了したらフラグリセット
        if (hasIdleBreakFinished)
        {
            animator.SetBool("Action", false);
        }
        characterController.Move(velocity * Time.deltaTime);
    }
}
