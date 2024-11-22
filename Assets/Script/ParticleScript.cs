using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    [SerializeField]
    private PlayerScript playerScript;
    //パーティクルシステム
    private ParticleSystem ps;

    private bool flag;
    //経過時間
    private float elapsedTime;
    [SerializeField]
    int numEnter;
    [SerializeField]
    int numInside;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.GetComponent<Renderer>().enabled = false;
        playerScript = GameObject.Find("Character_Female_Hotel Owner").GetComponent<PlayerScript>();
        ps.trigger.SetCollider(0, playerScript.transform);
        //MaxParticlesを超えるパーティクルを生成するまでシミュレーションスピードを上げる
        var main = ps.main;
        main.simulationSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        //現在のパーティクル数がMaxParticlesを超えたらパーティクルを移動させる
        if(!flag&&ps.particleCount>=ps.main.maxParticles)
        {
            var main = ps.main;
            main.simulationSpeed = 1f;
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

            //particles
            List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
            List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();

            //get
             numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
             numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

            if(numEnter!=0||numInside !=0)
            {
                Debug.Log("接触");
                if(playerScript.GetState()!=PlayerScript.MyState.Damage&&playerScript.GetAvoid()==false)
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
