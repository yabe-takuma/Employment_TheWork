using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class AttackSwordScript : MonoBehaviour
{
    //敵の攻撃力やプレイヤーにアニメーションをするためやUIやエフェクトなどの変数です。
    [SerializeField]
    private MyStatus myStatus;
    [SerializeField]
    private PlayerScript playerscript;
    [SerializeField]
    private ProcessCharaAnimEventScript processCharaAnimEvent;
    [SerializeField]
    private GameObject sworddamageUI;
    [SerializeField]
    private List<GameObject> damageEffects;
    private bool isAttack;
    //3段目の攻撃時敵をダウンさせるためにAnimatorを参照
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private bool isCollision;
    [SerializeField]
    private ParticleSystem swordTrailParticle;
    [SerializeField]
    private ParticleSystem fireSpark;
    [SerializeField]
    private ParticleSystem[] weaponParticles;
    [SerializeField]
    private ParticleSystem subSwordTrailParticle;


    // Start is called before the first frame update
    private void Start()
    {
        myStatus = transform.root.GetComponent<MyStatus>();
        playerscript = transform.root.GetComponent<PlayerScript>();
        animator = transform.root.GetComponent<Animator>();
        processCharaAnimEvent = transform.root.GetComponent<ProcessCharaAnimEventScript>();
        Transform moonSword= transform.Find("MoonSword Variant");
        weaponParticles = GetComponentsInChildren<ParticleSystem>();

        CacheParticles();
        //swordTrailParticle.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        //剣が雑魚敵に当たった時の処理
        if(other.tag=="Enemy" && this.gameObject.tag != "FireSword"&&this.gameObject.tag!="WaterSword")
        {
            var enemyScript = other.GetComponent<MoveEnemyScript>();
            // ここでアニメーションの時間をチェックし、特定の範囲のみヒットを許可
            float animTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //敵が死亡状態ではないとき敵に攻撃を与える処理
            if (enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead /*&&  animTime > 0.2f && animTime < 0.4f*/)
            {
                isCollision = true;
                other.GetComponent<MoveEnemyScript>().TakeDamage(myStatus.GetAttackPower(),other.ClosestPointOnBounds(transform.position));
                if (!enemyScript.IsDead())
                {
                    var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                    swordobj.transform.SetParent(other.transform);
                    if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                    {
                        var damageEffect = Instantiate<GameObject>(damageEffects[0]);
                        damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                        Destroy(damageEffect, 1f);
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
                    {
                        var damageEffect = Instantiate<GameObject>(damageEffects[1]);
                        damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                        Destroy(damageEffect, 1f);
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                    {
                        var damageEffect = Instantiate<GameObject>(damageEffects[2]);
                        damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                        Destroy(damageEffect, 1f);
                    }
                  
                }
                
                Debug.Log("敵に当たった");
            }
        }
        //炎の剣での敵の攻撃処理
        else if (other.tag == "Enemy"&&this.gameObject.tag=="FireSword")
        {

            var enemyScript = other.GetComponent<MoveEnemyScript>();
            // ここでアニメーションの時間をチェックし、特定の範囲のみヒットを許可
            float animTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //敵が死亡状態ではなくスキル攻撃ではないとき敵に攻撃を与える処理
            if (enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead/*&& animTime > 0.2f && animTime < 0.4f*/&&playerscript.GetState()!=PlayerScript.MyState.SkillAttack)
            {
                other.GetComponent<MoveEnemyScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                if (!enemyScript.IsDead())
                {
                    //ダメージのUIを出現させる処理です。当たった敵に親子関係を入れることで当たった敵のそばにUIが見えるようにしています。
                    var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                    swordobj.transform.SetParent(other.transform);
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                    {
                        var damageEffect = Instantiate<GameObject>(damageEffects[0]);
                        damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                        Destroy(damageEffect, 1f);
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
                    {
                        var damageEffect = Instantiate<GameObject>(damageEffects[1]);
                        damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                        Destroy(damageEffect, 1f);
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                    {
                        var damageEffect = Instantiate<GameObject>(damageEffects[2]);
                        damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                        Destroy(damageEffect, 1f);
                    }
                }
                Debug.Log("炎の剣が当たった");
            }
            else if(enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead && playerscript.GetState() == PlayerScript.MyState.SkillAttack)
            {
                other.GetComponent<MoveEnemyScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                other.GetComponent<MoveEnemyScript>().AbnormalCondition();
                if (enemyScript.IsDead())
                {
                    //ダメージのUIを出現させる処理です。当たった敵に親子関係を入れることで当たった敵のそばにUIが見えるようにしています。
                    var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                    swordobj.transform.SetParent(other.transform);
                    var damageEffect = Instantiate<GameObject>(damageEffects[1]);
                    damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                    Destroy(damageEffect, 1f);
                }
            }
        }
        //水の剣での敵の攻撃処理
        else if (other.tag == "Enemy" && this.gameObject.tag == "WaterSword")
        {

            var enemyScript = other.GetComponent<MoveEnemyScript>();
            // ここでアニメーションの時間をチェックし、特定の範囲のみヒットを許可
            float animTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //敵が死亡状態ではなくスキル攻撃ではないとき敵に攻撃を与える処理
            if (enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead /*&& animTime > 0.2f && animTime < 0.4f*/ && playerscript.GetState() != PlayerScript.MyState.SkillAttack)
            {
                other.GetComponent<MoveEnemyScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                if (!enemyScript.IsDead())
                {
                    //ダメージのUIを出現させる処理です。当たった敵に親子関係を入れることで当たった敵のそばにUIが見えるようにしています。
                    var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                    swordobj.transform.SetParent(other.transform);
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                    {
                        var damageEffect = Instantiate<GameObject>(damageEffects[0]);
                        damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                        Destroy(damageEffect, 1f);
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
                    {
                        var damageEffect = Instantiate<GameObject>(damageEffects[1]);
                        damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                        Destroy(damageEffect, 1f);
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                    {
                        var damageEffect = Instantiate<GameObject>(damageEffects[2]);
                        damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                        Destroy(damageEffect, 1f);
                    }
                }
                Debug.Log("水の剣が当たった");
            }
            if (enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead && playerscript.GetState() == PlayerScript.MyState.SkillAttack&&enemyScript.GetIsAbnormal())
            {
                other.GetComponent<MoveEnemyScript>().TakeDamage(myStatus.GetAttackPower()*6, other.ClosestPointOnBounds(transform.position));
                other.GetComponent<MoveEnemyScript>().DestroyFire();
                if (!enemyScript.IsDead())
                {
                    //ダメージのUIを出現させる処理です。当たった敵に親子関係を入れることで当たった敵のそばにUIが見えるようにしています。
                    var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                    swordobj.transform.SetParent(other.transform);
                    var damageEffect = Instantiate<GameObject>(damageEffects[3]);
                    damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                    Destroy(damageEffect, 1f);
                }
                Debug.Log("大ダメージ");
            }
            else if (enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead && playerscript.GetState() == PlayerScript.MyState.SkillAttack && !enemyScript.GetIsAbnormal())
            {
                other.GetComponentInParent<MoveEnemyScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                if (!enemyScript.IsDead())
                {
                    //ダメージのUIを出現させる処理です。当たった敵に親子関係を入れることで当たった敵のそばにUIが見えるようにしています。
                    var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                    swordobj.transform.SetParent(other.transform);
                    var damageEffect = Instantiate<GameObject>(damageEffects[3]);
                    damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                    Destroy(damageEffect, 1f);
                }
            }
        }
        //普通の剣がボスに当たった時の処理
        if (other.tag == "Boss" && this.gameObject.tag != "FireSword" && this.gameObject.tag != "WaterSword"&&!isAttack)
        {
            var trollScript = other.GetComponentInParent<TrollScript>();
            if (trollScript.GetState() != TrollScript.TrollState.Dead && isAttack == false)
            {
                other.GetComponentInParent<TrollScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                //ダメージのUIを出現させる処理です。当たった敵に親子関係を入れることで当たった敵のそばにUIが見えるようにしています。
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 4.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
                //ダメージエフェクトを出現させる処理です。当たった敵に親子関係を入れることで当たった敵のそばにエフェクトが見えるようにしています。
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                {
                    var damageEffect = Instantiate<GameObject>(damageEffects[0]);
                    damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                    Destroy(damageEffect, 1f);
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
                {
                    var damageEffect = Instantiate<GameObject>(damageEffects[1]);
                    damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                    Destroy(damageEffect, 1f);
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                {
                    var damageEffect = Instantiate<GameObject>(damageEffects[2]);
                    damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                    Destroy(damageEffect, 1f);
                }
                isAttack = true;
                Debug.Log("ボスに当たった");
            }
        }
        //炎の剣がボスに当たった時の処理
        else if (other.tag == "Boss" && this.gameObject.tag == "FireSword" && !isAttack)
        {

            var trollScript = other.GetComponentInParent<TrollScript>();
            if (trollScript.GetState() != TrollScript.TrollState.Dead && playerscript.GetState() != PlayerScript.MyState.SkillAttack)
            {
                other.GetComponentInParent<TrollScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                //ダメージのUIを出現させる処理です。当たった敵に親子関係を入れることで当たった敵のそばにUIが見えるようにしています。
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                {
                    var damageEffect = Instantiate<GameObject>(damageEffects[0]);
                    damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                    Destroy(damageEffect, 1f);
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
                {
                    var damageEffect = Instantiate<GameObject>(damageEffects[1]);
                    damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                    Destroy(damageEffect, 1f);
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                {
                    var damageEffect = Instantiate<GameObject>(damageEffects[2]);
                    damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                    Destroy(damageEffect, 1f);
                }
                Debug.Log("炎の剣がボスに当たった");
            }
            else if (trollScript.GetState() != TrollScript.TrollState.Dead && playerscript.GetState() == PlayerScript.MyState.SkillAttack)
            {
                other.GetComponentInParent<TrollScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                other.GetComponentInParent<TrollScript>().AbnormalCondition();
                //ダメージのUIを出現させる処理です。当たった敵に親子関係を入れることで当たった敵のそばにUIが見えるようにしています。
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
            }
        }
        //水の剣がボスに当たった時の処理
        else if (other.tag == "Boss" && this.gameObject.tag == "WaterSword" && !isAttack)
        {
            var trollScript = other.GetComponentInParent<TrollScript>();
            if (trollScript.GetState() != TrollScript.TrollState.Dead && playerscript.GetState() != PlayerScript.MyState.SkillAttack)
            {
                other.GetComponentInParent<TrollScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                //ダメージのUIを出現させる処理です。当たった敵に親子関係を入れることで当たった敵のそばにUIが見えるようにしています。
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                {
                    var damageEffect = Instantiate<GameObject>(damageEffects[0]);
                    damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                    Destroy(damageEffect, 1f);
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
                {
                    var damageEffect = Instantiate<GameObject>(damageEffects[1]);
                    damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                    Destroy(damageEffect, 1f);
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                {
                    var damageEffect = Instantiate<GameObject>(damageEffects[2]);
                    damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                    Destroy(damageEffect, 1f);
                }
                Debug.Log("水の剣がボスに当たった");
            }
            else if (trollScript.GetState() != TrollScript.TrollState.Dead && playerscript.GetState() == PlayerScript.MyState.SkillAttack && trollScript.GetIsAbnormal())
            {
                other.GetComponentInParent<TrollScript>().TakeDamage(myStatus.GetAttackPower() * 6, other.ClosestPointOnBounds(transform.position));
                other.GetComponentInParent<TrollScript>().DestroyFire();
                //ダメージのUIを出現させる処理です。当たった敵に親子関係を入れることで当たった敵のそばにUIが見えるようにしています。
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
            }
            else if (trollScript.GetState() != TrollScript.TrollState.Dead && playerscript.GetState() == PlayerScript.MyState.SkillAttack && !trollScript.GetIsAbnormal())
            {
                other.GetComponentInParent<TrollScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                //ダメージのUIを出現させる処理です。当たった敵に親子関係を入れることで当たった敵のそばにUIが見えるようにしています。
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
            }
        }
        //プレイヤーの連続攻撃の3段目を当てると敵が倒れるようになる処理です。
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03") && other.tag == "Enemy")
        {
            var enemyScript = other.GetComponent<MoveEnemyScript>();
            other.GetComponentInParent<MoveEnemyScript>().KnockBackDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
        }
    }

  

    // Update is called once per frame
    void Update()
    {
        //プレイヤーが攻撃のアニメーションをしていないときはダメージを表示しないフラグ
        if (playerscript!=null&&playerscript.GetState() != PlayerScript.MyState.Attack && playerscript.GetState() != PlayerScript.MyState.SkillAttack)
        {
            isAttack = false;
        }
        if (!processCharaAnimEvent.IsWeponCollision())
        {
            isCollision = false;
        }
    }
    //他のスクリプトに参照できるようにする関数です。
    public bool IsAttack()
    {
        return isAttack;
    }

    public bool IsCollision()
    {
        return isCollision;
    }

    public void offIsCollision()
    {
        isCollision = false;
    }

    void CacheParticles()
    {
        ParticleSystem[] allParticles = GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in allParticles)
        {
            if (ps.name == "SwordTrailParticle")
            {
                swordTrailParticle = ps;
            }
            else if (ps.name == "SubSwordTrailParticle2")
            {
                subSwordTrailParticle = ps;
            }
        }
    }



    public void PlaySwordTrail()
    {
        Debug.Log("Play() 呼ばれました");

        if (swordTrailParticle != null)
        {
            Debug.Log($"SwordTrailParticle Play! Delay: {swordTrailParticle.startDelay}, Duration: {swordTrailParticle.duration}");
            //swordTrailParticle.Clear();
            //swordTrailParticle.Play();
        }



        if (subSwordTrailParticle != null)
        {
            Debug.Log("SubSwordTrailParticle Target: " + subSwordTrailParticle.transform.parent.name);

           
            subSwordTrailParticle.Play(); 



        }




    }


}
