using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessEnemyAnimEventScript : MonoBehaviour
{
    private MoveEnemyScript enemy;  //敵の行動などがあるスクリプト
    [SerializeField]
    private SphereCollider sphereCollider;  //攻撃用のコライダー
    [SerializeField]
    private Transform equipment;
    [SerializeField]
    private GameObject weapon;

    private GameObject wepon;

    private bool isWeapon;

    private int equipments;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<MoveEnemyScript>();
       
    }

     public void AttackStart()
     {
        //攻撃の時コライダーを表示する処理
        sphereCollider.enabled = true;
        Debug.Log("攻撃開始");
     }

    public void AttackEnd()
    {
        //攻撃終了時コライダーを非表示にする処理
        sphereCollider.enabled = false;
        Debug.Log("攻撃終了");
    }

    public void StateEnd()
    {
        //アニメーションが終わったら待機状態に戻る処理
        enemy.SetState(MoveEnemyScript.EnemyState.Freeze);
        Debug.Log("固まる");
    }

    public void EndDamage()
    {
        //ダメージアニメーションが終わったら歩くようにする処理
        enemy.SetState(MoveEnemyScript.EnemyState.Walk);
        Debug.Log("食らい終わった");
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy.GetState()==MoveEnemyScript.EnemyState.Dead)
        {
            sphereCollider.enabled = false;
        }
       
    }

    public void SetWeapon(GameObject weapons)
    {
        weapon = weapons;
    }

    public int GetWeaponCaunter()
    {
        return equipments;
    }
}
