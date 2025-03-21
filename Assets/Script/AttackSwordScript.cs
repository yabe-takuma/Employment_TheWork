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

  
    // Start is called before the first frame update
    private void Start()
    {
        myStatus = transform.root.GetComponent<MyStatus>();
        playerscript = transform.root.GetComponent<PlayerScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //剣が雑魚敵に当たった時の処理
        if(other.tag=="Enemy" && this.gameObject.tag != "FireSword")
        {
           
            var enemyScript = other.GetComponent<MoveEnemyScript>();
            if (enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead)
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
            if (enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead)
            {
                other.GetComponent<MoveEnemyScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                other.GetComponent<MoveEnemyScript>().AbnormalCondition();
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 1.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
                Debug.Log("炎の剣が当たった");
            }
        }
        //剣がボスに当たった時の処理
        if (other.tag=="Boss")
        {
            var trollScript = other.GetComponentInParent<TrollScript>();
            if(trollScript.GetState()!=TrollScript.TrollState.Dead&&isAttack==false)
            {
                other.GetComponentInParent<TrollScript>().TakeDamage(myStatus.GetAttackPower(), other.ClosestPointOnBounds(transform.position));
                var swordobj = Instantiate(sworddamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y-4.0f, other.bounds.center.z), Quaternion.identity);
                swordobj.transform.SetParent(other.transform);
                var damageobj = Instantiate(damageEffect, new Vector3(other.bounds.center.x, other.bounds.center.y, other.bounds.center.z), Quaternion.identity);
                damageobj.transform.SetParent(other.transform);
                isAttack = true;
                Debug.Log("ボスに当たった");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーが攻撃のアニメーションをしていないときはダメージを表示しないフラグ
        if (playerscript!=null&&playerscript.GetState() != PlayerScript.MyState.Attack)
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
