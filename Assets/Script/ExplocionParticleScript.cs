using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplocionParticleScript : MonoBehaviour
{
    [SerializeField]
    private PlayerScript playerScript;
    //パーティクルシステム
    private ParticleSystem ps;

    private bool flag;
   
    //敵やボスにダメージを与えるのに必要な変数
    [SerializeField]
    private TrollScript trollScript;
    [SerializeField]
    private MoveEnemyScript moveEnemyScript;
    //--------------------//
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.GetComponent<Renderer>().enabled = false;
        playerScript = GameObject.Find("Character_Female_Hotel Owner").GetComponent<PlayerScript>();
        trollScript = GameObject.Find("GiantTroll").GetComponent<TrollScript>();
        ps.trigger.SetCollider(0, playerScript.transform);
        //MaxParticlesを超えるパーティクルを生成するまでシミュレーションスピードを上げる
        var main = ps.main;
        main.simulationSpeed = 10f;
        //プレハブ化しているため他のスクリプトからデータをもらう処理
        moveEnemyScript = playerScript.GetEnemyScript();
    }

    // Update is called once per frame
    void Update()
    {
        //現在のパーティクル数がMaxParticlesを超えたらパーティクルを移動させる
        if (!flag && ps.particleCount >= ps.main.maxParticles)
        {
            var main = ps.main;
            main.simulationSpeed = 1f;
            flag = true;
            ps.GetComponent<Renderer>().enabled = true;
            var a = ps.velocityOverLifetime;
            a.radial = 2f;
        }
    }

    public void OnParticleCollision(GameObject other)
    {
        //爆発が当たったオブジェクトに応じてダメージを変える処理
        if(other.tag=="Player"&& playerScript.GetState() != PlayerScript.MyState.Damage && playerScript.GetState() != PlayerScript.MyState.Dead &&
           playerScript.GetAvoid() == false && trollScript.GetState() == TrollScript.TrollState.explocion && trollScript.GetState() != TrollScript.TrollState.Dead)
        {
            playerScript.Damage(1);
        }
        if(other.tag=="Boss"&&playerScript.GetState()==PlayerScript.MyState.SkillAttack
            &&trollScript.GetState()!=TrollScript.TrollState.Dead)
        {
            trollScript.TakeDamage(5, other.transform.position);
        }
        if(other.tag=="Enemy" && playerScript.GetState() == PlayerScript.MyState.SkillAttack
            &&moveEnemyScript.GetState()!=MoveEnemyScript.EnemyState.Damage && moveEnemyScript.GetState() != MoveEnemyScript.EnemyState.Dead)
        {
            other.GetComponent<MoveEnemyScript>().TakeDamage(5, other.transform.position);
        }
    }
}
