using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessCharaAnimEventScript : MonoBehaviour
{
    private PlayerScript playerscript;  //プレイヤーの行動などがあるスクリプト
    [SerializeField]
    private Collider weaponCollider;  //武器のコライダー
    //装備品の親のTransform
    [SerializeField]
    private Transform equip;
    //ジャンプに必要なアニメーション
    [SerializeField]
    private Animator animator;
    //足元にパーティクルを入れるのに必要
    [SerializeField]
    private ParticleSystem footStepParticle;
    //武器の当たり判定を受け付けるフラグ
    [SerializeField]
    private bool isWeponCollision;

    [SerializeField]
    private GameObject swordTrajectory;
    private GameObject emptySwordTrajectory;

    // Start is called before the first frame update
    void Start()
    {
        playerscript = GetComponent<PlayerScript>();
        weaponCollider = equip.GetComponentInChildren<Collider>();
       
    }

    public void AttackStart()
    {
        //武器のコライダーがなかったらコライダーを表示させる処理
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            animator.applyRootMotion = false;
            Debug.Log("攻撃開始");
        }
        emptySwordTrajectory=Instantiate(swordTrajectory, equip.transform.position, equip.transform.rotation);
    }

    public void AttackEnd()
    {
        //攻撃アニメーションが終わったらコライダーを非表示にする処理
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
        Destroy(emptySwordTrajectory);
    }
    //アニメーションが終わったら待機状態に戻る関数
    public void StateEnd()
    {
        playerscript.SetState(PlayerScript.MyState.Normal);
    }

    public void StartWeponCollision()
    {
        isWeponCollision = true;
    }

    public bool IsWeponCollision()
    {
        return isWeponCollision;
    }

    public void EndWeponCollision()
    {
        isWeponCollision = false;
    }

    public void StartDamage()
    {
        animator.applyRootMotion = false;
    }

    public void EndDamage()
    {
        //ダメージアニメーションが終わったら待機状態に戻る関数
        if (playerscript.GetState() != PlayerScript.MyState.Dead)
        {
            playerscript.SetState(PlayerScript.MyState.Normal);
            Debug.Log("プレイヤー食らい終わった");
        }
    }

    public void JumpStop()
    {
        animator.SetFloat("JumpAnimation", 0.0f);
        Debug.Log("硬直中");
    }

    public void JumpStart()
    {
        if (playerscript.GetPosition().y <= 0.1f)
        {
            animator.SetFloat("JumpAnimation", 1.0f);
        }
    }

    public void PlayFootStepEffect()
    {
        footStepParticle.Play();
    }
    //複数の武器のコライダーを格納するための関数
    public void SetCollider(Collider col)
    {
        weaponCollider = col;
       
    }

    // Update is called once per frame
    void Update()
    {
        JumpStart();
        if (playerscript.GetPosition().y >= 0.1f)
        {
            footStepParticle.Pause();
        }
    }
}
