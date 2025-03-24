using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAxe : MonoBehaviour
{
    // Start is called before the first frame update
    // 斧の攻撃力や特定の敵に当たった時だけダメージが変わるUI
    // や通常時のダメージUIやエフェクトなどの変数を用意しています。
    private MyStatus myStatus;
    private PlayerScript playerscript;
    [SerializeField]
    private GameObject weakUI;
    [SerializeField]
    private GameObject axedamageUI;
    [SerializeField]
    private GameObject axenormaldamageUI;
    [SerializeField]
    private GameObject damageEffect;
    private bool isAttack;

    [SerializeField]
    private SphereCollider sphereCollider;  //最初から自動でコライダーのチェックをオンにするため
    [SerializeField]
    private MyItemScript myItemScript;
    [SerializeField]
    private bool isCollision;

    // Start is called before the first frame update
    private void Start()
    {
        myStatus = transform.root.GetComponent<MyStatus>();
        playerscript = transform.root.GetComponent<PlayerScript>();
        if (this.gameObject.tag == "Item")
        {
            sphereCollider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //敵に当たった時ダメージを表示するUIやエフェクトなどを表示しています。
        if (other.tag == "Enemy"&&this.gameObject.tag!="Item"&&playerscript!=null)
        {

            var enemyScript = other.GetComponent<MoveEnemyScript>();
            if (enemyScript.GetState() != MoveEnemyScript.EnemyState.Dead)
            {
                other.GetComponent<MoveEnemyScript>().TakeDamage(myStatus.GetAxeAttackPower(), other.ClosestPointOnBounds(transform.position));
                var axedamageobj = Instantiate(axenormaldamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y, other.bounds.center.z), Quaternion.identity);
                axedamageobj.transform.SetParent(other.transform);
                Debug.Log("敵に当たった");
                isCollision = true;
            }
        }
       
        //ボスに当たった時ダメージを表示するUIやエフェクトなどを表示しています。
        if (other.tag == "Boss")
        {
            var trollScript = other.GetComponentInParent<TrollScript>();
            if (trollScript.GetState() != TrollScript.TrollState.Dead)
            {
                other.GetComponentInParent<TrollScript>().TakeDamage(myStatus.GetAxeAttackPower()*2, other.ClosestPointOnBounds(transform.position));
                var weakobj = Instantiate(weakUI, new Vector3(other.bounds.center.x,other.bounds.center.y-2.0f,other.bounds.center.z), Quaternion.identity);
                weakobj.transform.SetParent(other.transform);
                var axedamageobj = Instantiate(axedamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 4.0f, other.bounds.center.z), Quaternion.identity);
                axedamageobj.transform.SetParent(other.transform);
                var damageobj = Instantiate(damageEffect, new Vector3(other.bounds.center.x, other.bounds.center.y, other.bounds.center.z), Quaternion.identity);
                damageobj.transform.SetParent(other.transform);
                isAttack = true;
                Debug.Log("ボスに当たった");
            }
        }
       
        if(other.tag=="SearchItemArea")
        {
            sphereCollider.enabled = false;
            Debug.Log("アイテムが削除");
        }
    }
  

    // Update is called once per frame
    void Update()
    {
        if (playerscript!=null&&playerscript.GetState() != PlayerScript.MyState.Attack)
        {
            isAttack = false;
        }
        if (this.gameObject.tag == "Item" && myItemScript ==null&& playerscript != null && playerscript.GetState() != PlayerScript.MyState.Attack)
        {
            sphereCollider.enabled = false;
        }
        else if(this.gameObject.tag == "Item" && playerscript !=null&&playerscript.GetState() != PlayerScript.MyState.Attack)
        {
            sphereCollider.enabled = true;
        }
       
    }

    public void SetMyItem(MyItemScript myitemscript)
    {
        myItemScript = myitemscript;
    }

   
}
