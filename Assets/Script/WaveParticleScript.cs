using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveParticleScript : MonoBehaviour
{
    [SerializeField]
    private PlayerScript playerScript;
    //パーティクルシステム
    [SerializeField]
    private ParticleSystem ps;
    [SerializeField]
    private bool flag;
    //経過時間
    private float elapsedTime;
    [SerializeField]
    int numEnter;
    [SerializeField]
    int numInside;

    [SerializeField]
    private TrollScript trollScript;

    [SerializeField]
    private ReceiveAttackEventScript receveattackevent;
    [SerializeField]
    private int timer;

    private Quaternion fixedRotation;



    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.GetComponent<Renderer>().enabled = false;
        playerScript = GameObject.Find("Character_Female_Hotel Owner").GetComponent<PlayerScript>();
        trollScript = GameObject.Find("GiantTroll").GetComponent<TrollScript>();
        receveattackevent = GameObject.Find("GiantTroll").GetComponent<ReceiveAttackEventScript>();
        ps.trigger.SetCollider(0, playerScript.transform);
      
        // ボスのY軸の回転だけを取り出して、回転を明示的に設定
        float yAngle = trollScript.GetRotation().eulerAngles.y;
        fixedRotation = Quaternion.Euler(90f, yAngle, 0f);
        transform.rotation = fixedRotation;

        //MaxParticlesを超えるパーティクルを生成するまでシミュレーションスピードを上げる
        var main = ps.main;
        main.simulationSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = fixedRotation; // 常に固定された角度を使う
        WaveParticleUpdate();


    }

    public void OnParticleTrigger()
    {
        if (ps != null && flag)
        {
            
            //particles
            List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
            List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();

            //get
            numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
            numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

            if (numEnter != 0 || numInside != 0)
            {
                Debug.Log("接触");
                if (playerScript.GetState() != PlayerScript.MyState.Damage && playerScript.GetState() != PlayerScript.MyState.Dead &&
                    playerScript.GetAvoid() == false && trollScript.GetState() != TrollScript.TrollState.Dead)
                {
                    playerScript.Damage(1);
                }
            }

            for (int i = 0; i < numEnter; i++)
            {
                ParticleSystem.Particle p = enter[i];
                p.startColor = new Color32(255, 0, 0, 255);
                inside[i] = p;
            }

            ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
            ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

        }
    }

    void WaveParticleUpdate()
    {
        elapsedTime += Time.deltaTime;

        //現在のパーティクル数がMaxParticlesを超えたらパーティクルを移動させる
        if (!flag && ps.particleCount >= ps.main.maxParticles)
        {
            var main = ps.main;
            main.simulationSpeed = 1f;
            flag = true;
            ps.GetComponent<Renderer>().enabled = true;

            // TrollのYだけを使った正確な向きにする
            float trollY = trollScript.GetRotation().eulerAngles.y;
            Quaternion yOnlyRotation = Quaternion.Euler(0f, trollY, 0f);
            transform.rotation = yOnlyRotation;

            var a = ps.velocityOverLifetime;
            a.enabled = true;
            a.space = ParticleSystemSimulationSpace.World;

            // 「forward方向」に進ませたいとき（真上ではなく地面方向）
            Vector3 forward = transform.forward.normalized;

            // 上方向へ飛ばさないようにY成分をゼロにし、水平方向にのみ移動させる
            a.x = new ParticleSystem.MinMaxCurve(forward.x * 5f); // X方向に速度（適宜調整）
            a.y = new ParticleSystem.MinMaxCurve(0f); // Y方向はゼロで水平に
            a.z = new ParticleSystem.MinMaxCurve(forward.z * 5f); // Z方向にも速度（適宜調整）


        }
       
        if (receveattackevent.GetIsWave())
        {
            timer++;
        }
    }
}
