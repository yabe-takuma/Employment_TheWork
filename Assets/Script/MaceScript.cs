using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceScript : MonoBehaviour
{

    //攻撃を有効にするかどうか
    private bool enableAttack;
    //メイスのコライダ群
    private Collider[] maceColliders;
    //攻撃相手のCharacterController
    [SerializeField]
    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        //メイスに使っているコライダを全取得
        maceColliders = GetComponents<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //攻撃が有効な範囲のアニメーションでなければ何もしない
        if(!enableAttack)
        {
            return;
        }
        if(collision.gameObject.tag=="Player")
        {
            var playerChara = collision.gameObject.GetComponent<PlayerScript>();
            //キャラがダメージ状態でなければダメージを与える
            if(playerChara.GetState() != PlayerScript.MyState.Damage)
            {
                playerChara.Damage();
                //キャラが攻撃を受けた時にメイスとの衝突を無効にする
                IgnoreCollision(true);
            }
        }
    }
    //攻撃の有効・向こうの切り替えメソッド
    public void ChangeEnableAttack(bool flag)
    {
        //攻撃開始時にはキャラとメイスを有効にしておく
        if(flag)
        {
            IgnoreCollision(false);
        }
        enableAttack = flag;
    }
    //ダメージ時のキャラとメイスの衝突を切り替えるメソッド
    public void IgnoreCollision(bool flag)
    {
        foreach(var item in maceColliders)
        {
            Physics.IgnoreCollision(item, characterController, flag);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
