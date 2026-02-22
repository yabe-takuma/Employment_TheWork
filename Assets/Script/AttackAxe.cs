using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackAxe : MonoBehaviour
{
    // Start is called before the first frame update
    // 斧の攻撃力や特定の敵に当たった時だけダメージが変わるUI
    // や通常時のダメージUIやエフェクトなどの変数を用意しています。
    [Header("プレイヤーのステータスについてのスクリプト(ただ勝手に取得しているので気にしなくてもいい)")]
    private MyStatus myStatus;
    [Header("プレイヤースクリプト(ただ勝手に取得しているので気にしなくてもいい)")]
    private PlayerScript playerscript;
    [Header("弱点を表示するためのUI")]
    [SerializeField]
    private GameObject weakUI;
    [Header("斧の弱点ダメージを表示するためのUI")]
    [SerializeField]
    private GameObject axedamageUI;
    [Header("斧の通常ダメージを表示するためのUI")]
    [SerializeField]
    private GameObject axenormaldamageUI;
    [Header("ダメージのパーティクル")]
    [SerializeField]
    private List<GameObject> damageEffects;
    private bool isAttack;

    [Header("剣のコライダー")]
    [SerializeField]
    private SphereCollider sphereCollider;  //最初から自動でコライダーのチェックをオンにするため
    [Header("アイテムを取ることを記述しているスクリプト")]
    [SerializeField]
    private MyItemScript myItemScript;
    
    //3段目の攻撃時敵をダウンさせるためにAnimatorを参照
    [SerializeField]
    private Animator animator;
  
    [SerializeField]
    private ParticleSystem[] weaponParticles;

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
      
        animator = transform.root.GetComponent<Animator>();
        Transform axe = transform.Find("axe Variant");
        weaponParticles = GetComponentsInChildren<ParticleSystem>();
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

    public void PlaySwordTrail()
    {
        Debug.Log("Play() 呼ばれました");
        weaponParticles[0].Play();
        weaponParticles[0].Play();
    }

}
