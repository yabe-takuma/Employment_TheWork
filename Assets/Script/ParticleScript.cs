using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    //当たったらプレイヤーにダメージを受けるための変数
    [SerializeField]
    private PlayerScript playerScript;
    //パーティクルシステム
    private ParticleSystem ps;
    //現在のパーティクル数がMaxParticlesを超えたらパーティクルを移動させるのに必要な変数
    private bool flag;
    //当たり判定に必要な変数
    [SerializeField]
    int numEnter;
    [SerializeField]
    int numInside;
    //-----------//
    //当たり判定に必要
    [SerializeField]
    private TrollScript trollScript;

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
    }

    // Update is called once per frame
    void Update()
    {
        //現在のパーティクル数がMaxParticlesを超えたらパーティクルを移動させる
        if(!flag&&ps.particleCount>=ps.main.maxParticles)
        {
            var main = ps.main;
            main.simulationSpeed = 10f;
            flag = true;
            ps.GetComponent<Renderer>().enabled = true;
            var a = ps.velocityOverLifetime;
            a.radial = 2f;
        }

        
    }

    public void OnParticleTrigger()
    {
        if(ps != null&&flag)
        {

            //複数のパーティクルを格納するための処理
            List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
            List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();

            //当たり判定で使う処理
             numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
             numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
            //当たったらプレイヤーがダメージを受ける処理
            if(numEnter!=0||numInside !=0)
            {
                Debug.Log("接触");
                if(playerScript.GetState()!=PlayerScript.MyState.Damage&&playerScript.GetState()!=PlayerScript.MyState.Dead&&
                    playerScript.GetAvoid()==false&&trollScript.GetState()!=TrollScript.TrollState.Dead)
                {
                    playerScript.Damage(1);
                }
            }

            for(int i=0;i<numEnter;i++)
            {
                ParticleSystem.Particle p = enter[i];
                p.startColor = new Color32(255, 0,0, 255);
                inside[i] = p;
            }

            ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
            ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

        }
    }

}
