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
    public float barrierDistance = 600f; // ボスからの距離

    public float horizontalBarrierDistance = 4f; // 左右のバリア距離（短くしたい）
    public float verticalBarrierDistance = 6f;   // 前後のバリア距離（そのまま or 広く）

    private List<GameObject> barriers = new List<GameObject>();

    private bool isBossBattleActive;

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
        ////キャラクターが範囲外に出たらidle状態にする
        //if(other.tag == "Player"
        //    && trollScript.GetState() == TrollScript.TrollState.chase)
        //{
        //    trollScript.SetState(TrollScript.TrollState.idle);
        //    HPUI.SetActive(false);
        //    target = null;
        //    EndBossBattle();
        //}
       
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
        if (barriers.Count > 0) return;

        float yOffset = 0.5f;
        float halfSize = barrierDistance;

        // 四隅を基準に位置を計算（Box型で囲む）
        Vector3 bossPos = bossTransform.position;

        Vector3 topLeft = bossPos + new Vector3(-horizontalBarrierDistance, 0, verticalBarrierDistance);
        Vector3 topRight = bossPos + new Vector3(horizontalBarrierDistance, 0, verticalBarrierDistance);
        Vector3 bottomLeft = bossPos + new Vector3(-horizontalBarrierDistance, 0, -verticalBarrierDistance);
        Vector3 bottomRight = bossPos + new Vector3(horizontalBarrierDistance, 0, -verticalBarrierDistance);

        // 壁を4辺に展開（壁の長さに応じてforループや spacing を変えてもOK）
        CreateBarrierBetween(topLeft, topRight, yOffset);    // 上辺
        CreateBarrierBetween(bottomLeft, bottomRight, yOffset); // 下辺
        CreateBarrierBetween(topLeft, bottomLeft, yOffset);  // 左辺
        CreateBarrierBetween(topRight, bottomRight, yOffset); // 右辺


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

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }


    }

    void EndBossBattle()
    {
        RemoveBossBarrier();  // バリア解除
    }

    void CreateBarrierBetween(Vector3 start, Vector3 end, float yOffset)
    {
        int segmentCount = 6; // 必要な分割数（距離に応じて）
        for (int i = 0; i <= segmentCount; i++)
        {
            float t = (float)i / segmentCount;
            Vector3 pos = Vector3.Lerp(start, end, t) + Vector3.up * yOffset;
            Quaternion rot = Quaternion.LookRotation(end - start);
            GameObject barrier = Instantiate(barrierPrefab, pos, rot);
            barriers.Add(barrier);
        }
    }

    public bool IsBossBattleActive()
    {
        return isBossBattleActive;
    }

}
