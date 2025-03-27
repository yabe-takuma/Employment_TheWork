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
            && trollScript.GetState()!= TrollScript.TrollState.Dead
            &&!trollScript.GetWave())
        {
            trollScript.SetState(TrollScript.TrollState.chase, other.transform);
            HPUI.SetActive(true);
            target = other.gameObject;
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
}
