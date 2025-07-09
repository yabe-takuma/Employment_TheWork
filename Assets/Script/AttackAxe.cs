using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private List<GameObject> damageEffects;
    private bool isAttack;

    [SerializeField]
    private SphereCollider sphereCollider;  //最初から自動でコライダーのチェックをオンにするため
    [SerializeField]
    private MyItemScript myItemScript;
    [SerializeField]
    private bool isCollision;
    //3段目の攻撃時敵をダウンさせるためにAnimatorを参照
    [SerializeField]
    private Animator animator;
    //プレイヤーが斧を取得したらanimatorにデータを送るために必要な変数
    [SerializeField]
    private bool isItem;

    // Start is called before the first frame update
    private void Start()
    {
        myStatus = transform.root.GetComponent<MyStatus>();
        playerscript = transform.root.GetComponent<PlayerScript>();
        //Itemのタグが付いているものは最初からColliderをオンにして触れるようにする
        if (this.gameObject.tag == "Item")
        {
            sphereCollider.enabled = true;
        }
        isItem = false;  //まだアイテムに触れていないのでfalseにする
        animator = transform.root.GetComponent<Animator>();
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
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("AxeAttack"))
                {
                    var damageEffect = Instantiate<GameObject>(damageEffects[0]);
                    damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                    Destroy(damageEffect, 1f);
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("AxeAttack2")|| animator.GetCurrentAnimatorStateInfo(0).IsName("AxeSkillAttack2"))
                {
                    var damageEffect = Instantiate<GameObject>(damageEffects[1]);
                    damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                    Destroy(damageEffect, 1f);
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("AxeAttack3"))
                {
                    var damageEffect = Instantiate<GameObject>(damageEffects[2]);
                    damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                    Destroy(damageEffect, 1f);
                }
                Debug.Log("敵に当たった");
                isCollision = true;
            }
        }
       
        //ボスに当たった時ダメージを表示するUIやエフェクトなどを表示しています。
        if (other.tag == "Boss"&&!isAttack)
        {
            var trollScript = other.GetComponentInParent<TrollScript>();
            if (trollScript.GetState() != TrollScript.TrollState.Dead)
            {
                other.GetComponentInParent<TrollScript>().TakeDamage(myStatus.GetAxeAttackPower()*2, other.ClosestPointOnBounds(transform.position));
                var weakobj = Instantiate(weakUI, new Vector3(other.bounds.center.x,other.bounds.center.y-2.0f,other.bounds.center.z), Quaternion.identity);
                weakobj.transform.SetParent(other.transform);
                var axedamageobj = Instantiate(axedamageUI, new Vector3(other.bounds.center.x, other.bounds.center.y - 4.0f, other.bounds.center.z), Quaternion.identity);
                axedamageobj.transform.SetParent(other.transform);
                //var damageobj = Instantiate(damageEffects[0], new Vector3(other.bounds.center.x, other.bounds.center.y, other.bounds.center.z), Quaternion.identity);
                //damageobj.transform.SetParent(other.transform);
                var damageEffect = Instantiate<GameObject>(damageEffects[1]);
                damageEffect.transform.position = other.ClosestPointOnBounds(transform.position);
                Destroy(damageEffect, 1f);
                isAttack = true;
                Debug.Log("ボスに当たった");
            }
        }
       
        if(other.tag=="SearchItemArea")
        {
            sphereCollider.enabled = false;
            isItem = true;
            Debug.Log("アイテムが削除");
        }
    }
  

    // Update is called once per frame
    void Update()
    {
        //アイテム取得する前の当たり判定の有無や攻撃の当たり判定の有無の処理
        PlayerAround();
    }

    public void SetMyItem(MyItemScript myitemscript)
    {
        myItemScript = myitemscript;
    }

    

    void PlayerAround()
    {
        if (playerscript != null && playerscript.GetState() != PlayerScript.MyState.Attack)
        {
            isAttack = false;
        }
        if (this.gameObject.tag == "Item" && myItemScript == null && playerscript != null && playerscript.GetState() != PlayerScript.MyState.Attack)
        {
            sphereCollider.enabled = false;
        }
        else if (this.gameObject.tag == "Item" && playerscript != null && playerscript.GetState() != PlayerScript.MyState.Attack)
        {
            sphereCollider.enabled = true;
        }
    }
   
}
