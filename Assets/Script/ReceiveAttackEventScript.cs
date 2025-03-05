using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveAttackEventScript : MonoBehaviour
{
    //メイスのプレハブ
    [SerializeField]
    private MaceScript mace;
    //衝撃波のプレハブ
    [SerializeField]
    private GameObject shockwavePrefab;
    //衝撃波や爆発などを生成する座標
    [SerializeField]
    private Transform createShockwavePoint;
    //設置物
    [SerializeField]
    private GameObject installationsphere;
    //GiantTrollのスクリプト
    [SerializeField]
    private TrollScript trollScript;

    //爆発
    [SerializeField]
    private GameObject explocion;
    //爆発の範囲を可視化するプレハブ
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

    //波生成時の角度調整
    [SerializeField]
    private Quaternion rotation;
    private bool isWave;
    private bool iscontinuous;

    //攻撃制御
    [SerializeField]
    private bool isAttack;
    // Start is called before the first frame update
    void Start()
    {
        mace = GetComponentInChildren<MaceScript>();
        rotation = new Quaternion(wave.transform.rotation.x, wave.transform.rotation.y + trollScript.GetRotation().y, wave.transform.rotation.z, wave.transform.rotation.w);
        isAttack = true;
    }

    //攻撃開始時
    public void StartAttack()
    {
        //攻撃終了時メイスのコライダーを有効化する処理
        mace.ChangeEnableAttack(true);
        isAttack = true;
        Debug.Log("メイス攻撃開始");
    }
    //攻撃終了時
    public void EndAttack()
    {
        //攻撃終了時メイスのコライダーを無効化する処理
        mace.ChangeEnableAttack(false);
        isAttack = false;
        Debug.Log("メイス攻撃終了");
    }

    public void DuringAttack()
    {
        //爆発攻撃の際にアニメーションを止める処理
        if (trollScript.GetExplocion())
        {
            IsEndStop = true;
            animator.SetFloat("MovingSpeed", 0.0f);
            Instantiate(explocionomen, new Vector3(trollScript.GetPosition().x, 0.3f, trollScript.GetPosition().z), explocionomen.transform.rotation);
            Debug.Log("踏みとどまる");
        }
    }
    //衝撃波や波や設置物などの発生処理
    public void CreateShockwave()
    {
        if (trollScript.GetShockwave())
        {
            Instantiate(shockwavePrefab, createShockwavePoint.position, shockwavePrefab.transform.rotation);
            isWave = false;
            iscontinuous = false;
        }
        else if (trollScript.GetInstallation())
        {
            Instantiate(installationsphere, createShockwavePoint.position, installationsphere.transform.rotation);
            isWave = false;
            iscontinuous = false;
        }
        else if (trollScript.GetExplocion())
        {
            Instantiate(explocion, createShockwavePoint.position, explocion.transform.rotation);
            isWave = false;
            iscontinuous = false;
        }
        else if (trollScript.GetWave())
        {
            Instantiate(wave, createShockwavePoint.position,wave.transform.rotation );
            isWave = true;
        }
        else if (trollScript.GetContinuous())
        {
            Instantiate(wave, createShockwavePoint.position, wave.transform.rotation);
            isWave = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //爆発攻撃時アニメーションを一時止める処理
        if(IsEndStop==true)
        {
            EndStop++;
          
        }
        if (EndStop >= 500)
        {
            animator.SetFloat("MovingSpeed", 1.0f);
            EndStop = 0;
            IsEndStop = false;
        }

    }

    public bool GetIsWave()
    {
        return isWave;
    }

    public bool GetIsAttack()
    {
        return isAttack;
    }
}
