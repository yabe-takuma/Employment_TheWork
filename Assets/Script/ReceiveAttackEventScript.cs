using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveAttackEventScript : MonoBehaviour
{
    [SerializeField]
    private MaceScript mace;
    [SerializeField]
    private GameObject shockwavePrefab;
    [SerializeField]
    private Transform createShockwavePoint;
    //設置物
    [SerializeField]
    private GameObject installationsphere;
    [SerializeField]
    private TrollScript trollScript;

    //爆発
    [SerializeField]
    private GameObject explocion;
    [SerializeField]
    private GameObject explocionomen;

    //波
    [SerializeField]
    private GameObject wave;

    //アニメーションの一時停止
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private int EndStop;
    [SerializeField]
    private bool IsEndStop;

    [SerializeField]
    private Quaternion rotation;

    private bool isWave;

    private bool iscontinuous;

    // Start is called before the first frame update
    void Start()
    {
        mace = GetComponentInChildren<MaceScript>();
        rotation = new Quaternion(wave.transform.rotation.x, wave.transform.rotation.y + trollScript.GetRotation().y, wave.transform.rotation.z, wave.transform.rotation.w);
    }

    //攻撃開始時
    public void StartAttack()
    {
        mace.ChangeEnableAttack(true);
        Debug.Log("メイス攻撃開始");
    }
    //攻撃終了時
    public void EndAttack()
    {
        mace.ChangeEnableAttack(false);
        Debug.Log("メイス攻撃終了");
    }

    public void DuringAttack()
    {
        if (trollScript.GetExplocion())
        {
            IsEndStop = true;
            animator.SetFloat("MovingSpeed", 0.0f);
            Instantiate(explocionomen, new Vector3(createShockwavePoint.position.x, 0.3f, createShockwavePoint.position.z), explocionomen.transform.rotation);
        }
    }
    //衝撃波発生
    public void CreateShockwave()
    {
        if (trollScript.GetShockwave())
        {
            Instantiate(shockwavePrefab, createShockwavePoint.position, shockwavePrefab.transform.rotation);
            isWave = false;
            iscontinuous = false;
            Debug.Log("衝撃波発動");
        }
        else if (trollScript.GetInstallation())
        {
            Instantiate(installationsphere, createShockwavePoint.position, installationsphere.transform.rotation);
            isWave = false;
            iscontinuous = false;
            Debug.Log("設置物配置完了");
        }
        else if (trollScript.GetExplocion())
        {
            Instantiate(explocion, createShockwavePoint.position, explocion.transform.rotation);
            isWave = false;
            iscontinuous = false;
            Debug.Log("爆発完了");
        }
        else if (trollScript.GetWave())
        {
            Instantiate(wave, createShockwavePoint.position,wave.transform.rotation );
            isWave = true;
            Debug.Log("波完了");
        }
        else if (trollScript.GetContinuous())
        {
            Instantiate(wave, createShockwavePoint.position, wave.transform.rotation);
            isWave = true;
            Debug.Log("波完了");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(IsEndStop==true)
        {
            EndStop++;
          
        }

        if (EndStop >= 500)
        {
            animator.SetFloat("MovingSpeed", 1.0f);
            EndStop = 0;
            IsEndStop = false;
            //Destroy(explocionomen);
        }
    }

    public bool GetIsWave()
    {
        return isWave;
    }
}
