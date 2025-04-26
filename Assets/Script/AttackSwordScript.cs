using System.Collections;
using System.Collections.Generic;
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
    private GameObject sworddamageUI;
    [SerializeField]
    private GameObject damageEffect;
    private bool isAttack;
    //3段目の攻撃時敵をダウンさせるためにAnimatorを参照
    [SerializeField]
    private Animator animator;
  
    // Start is called before the first frame update
    private void Start()
    {
        myStatus = transform.root.GetComponent<MyStatus>();
        playerscript = transform.root.GetComponent<PlayerScript>();
        animator = transform.root.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //剣が雑魚敵に当たった時の処理
        if(other.tag=="Enemy" && this.gameObject.tag != "FireSword"&&this.gameObject.tag!="WaterSword")
        {
           
            var enemyScript = other.GetComponent<MoveEnemyScript>();
            if (enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead && enemyScript.GetState() == MoveEnemyScript.EnemyState.Chase)
            {
                other.GetComponent<MoveEnemyScript>().TakeDamage(myStatus.GetAttackPower(),other.ClosestPointOnBounds(transform.position));
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y-1.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
                Debug.Log("敵に当たった");
            }
        }

        else if (other.tag == "Enemy"&&this.gameObject.tag=="FireSword")
        {

            var enemyScript = other.GetComponent<MoveEnemyScript>();
            if (enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead&&playerscript.GetState()!=PlayerScript.MyState.SkillAttack)
            {
                other.GetComponent<MoveEnemyScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
                Debug.Log("炎の剣が当たった");
            }
            else if(enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead && playerscript.GetState() == PlayerScript.MyState.SkillAttack)
            {
                other.GetComponent<MoveEnemyScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                other.GetComponent<MoveEnemyScript>().AbnormalCondition();
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
            }
        }

        else if (other.tag == "Enemy" && this.gameObject.tag == "WaterSword")
        {

            var enemyScript = other.GetComponent<MoveEnemyScript>();
            if (enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead && playerscript.GetState() != PlayerScript.MyState.SkillAttack)
            {
                other.GetComponent<MoveEnemyScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
                Debug.Log("水の剣が当たった");
            }
            if (enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead && playerscript.GetState() == PlayerScript.MyState.SkillAttack&&enemyScript.GetIsAbnormal())
            {
                other.GetComponent<MoveEnemyScript>().TakeDamage(myStatus.GetAttackPower()*6, other.ClosestPointOnBounds(transform.position));
                other.GetComponent<MoveEnemyScript>().DestroyFire();
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
                Debug.Log("大ダメージ");
            }
            else if (enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead && playerscript.GetState() == PlayerScript.MyState.SkillAttack && !enemyScript.GetIsAbnormal())
            {
                other.GetComponentInParent<MoveEnemyScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
            }
        }
        //普通の剣がボスに当たった時の処理
        if (other.tag == "Boss" && this.gameObject.tag != "FireSword" && this.gameObject.tag != "WaterSword"&&!isAttack)
        {
            var trollScript = other.GetComponentInParent<TrollScript>();
            if (trollScript.GetState() != TrollScript.TrollState.Dead && isAttack == false)
            {
                other.GetComponentInParent<TrollScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 4.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
                var damageobj = Instantiate(damageEffect, new Vector3(other.bounds.center.x, other.bounds.center.y, other.bounds.center.z), Quaternion.identity);
                damageobj.transform.SetParent(other.transform);
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
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
                Debug.Log("炎の剣がボスに当たった");
            }
            else if (trollScript.GetState() != TrollScript.TrollState.Dead && playerscript.GetState() == PlayerScript.MyState.SkillAttack)
            {
                other.GetComponentInParent<TrollScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                other.GetComponentInParent<TrollScript>().AbnormalCondition();
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
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
                Debug.Log("水の剣がボスに当たった");
            }
            else if (trollScript.GetState() != TrollScript.TrollState.Dead && playerscript.GetState() == PlayerScript.MyState.SkillAttack && trollScript.GetIsAbnormal())
            {
                other.GetComponentInParent<TrollScript>().TakeDamage(myStatus.GetAttackPower() * 6, other.ClosestPointOnBounds(transform.position));
                other.GetComponentInParent<TrollScript>().DestroyFire();
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
            }
            else if (trollScript.GetState() != TrollScript.TrollState.Dead && playerscript.GetState() == PlayerScript.MyState.SkillAttack && !trollScript.GetIsAbnormal())
            {
                other.GetComponentInParent<TrollScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03")&&other.tag=="Enemy")
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
       
    }
    //他のスクリプトに参照できるようにする関数です。
    public bool IsAttack()
    {
        return isAttack;
    }
}
