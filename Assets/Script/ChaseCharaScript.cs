using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCharaScript : MonoBehaviour
{
    //トロルの行動などがあるスクリプト
    private TrollScript trollScript;
    //HPUI
    [SerializeField]
    private GameObject HPUI;

    //プレイヤーが来るのを検知する変数
    private GameObject target;

    public Transform bossTransform;  // ボスの位置
    public GameObject barrierPrefab; // バリアのプレハブ
    public float barrierDistance = 5f; // ボスからの距離

    private List<GameObject> barriers = new List<GameObject>();



    // Start is called before the first frame update
    void Start()
    {
        trollScript = GetComponentInParent<TrollScript>();
    }

    private void OnTriggerStay(Collider other)
    {
        //キャラクターが範囲内に来たら追いかける
        if(other.tag == "Player"
            && trollScript.GetState()!= TrollScript.TrollState.chase
            && trollScript.GetState()!= TrollScript.TrollState.attack
            && trollScript.GetState()!= TrollScript.TrollState.shockwaveAttack
            && trollScript.GetState()!= TrollScript.TrollState.explocion
            && trollScript.GetState()!= TrollScript.TrollState.wave
            && trollScript.GetState()!= TrollScript.TrollState.continuous
            && trollScript.GetState()!= TrollScript.TrollState.Dead)
        {
            trollScript.SetState(TrollScript.TrollState.chase, other.transform);
            HPUI.SetActive(true);
            target = other.gameObject;
            StartBossBattle();
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        //キャラクターが範囲外に出たらidle状態にする
        if(other.tag == "Player"
            && trollScript.GetState() == TrollScript.TrollState.chase)
        {
            trollScript.SetState(TrollScript.TrollState.idle);
            HPUI.SetActive(false);
            target = null;
            EndBossBattle();
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //他のスクリプトに参照するための変数
    public GameObject GetTarget()
    {
        return this.target;
    }

    void CreateBossBarrier()
    {
        Vector3[] barrierPositions = new Vector3[]
        {
        bossTransform.position + Vector3.forward * barrierDistance,  // 前側
        bossTransform.position - Vector3.forward * barrierDistance,  // 後側
        bossTransform.position + Vector3.right * barrierDistance,    // 右側
        bossTransform.position - Vector3.right * barrierDistance     // 左側
        };

        foreach (Vector3 pos in barrierPositions)
        {
            GameObject barrier = Instantiate(barrierPrefab, pos, Quaternion.identity);
            barriers.Add(barrier);
        }
    }

    void RemoveBossBarrier()
    {
        foreach (GameObject barrier in barriers)
        {
            Destroy(barrier); // バリア削除
        }
        barriers.Clear();
    }

    void StartBossBattle()
    {
        CreateBossBarrier();  // バリア設置
    }

    void EndBossBattle()
    {
        RemoveBossBarrier();  // バリア解除
    }



}
