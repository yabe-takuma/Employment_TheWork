using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SensorScript : MonoBehaviour
{
    public GameObject nowTarget;  //当たった敵やボスのデータを格納する変数
    [SerializeField]
    private List<GameObject> enemyList;  //複数の敵を格納するための変数
    private Camera cam;  //メインカメラ
    [SerializeField]
    private CameraScript camerascript;  //カメラ動きなどがあるスクリプト
    // Start is called before the first frame update
    void Start()
    {
        nowTarget = null;
        enemyList = new List<GameObject>();
        cam = Camera.main;
        camerascript = cam.GetComponent<CameraScript>();
    }

    void OnTriggerStay(Collider other)
    {
        //当たった時のレイヤーの名前を格納する処理
        string layerName = LayerMask.LayerToName(other.gameObject.layer);
        //当たった時のタグやレイヤーが指定通りだったら当たったオブジェクトを変数に格納する処理
        if (other.tag=="Boss" &&layerName=="Enemy"&& !enemyList.Contains(other.gameObject)|| other.tag == "Enemy" && !enemyList.Contains(other.gameObject))
        {
            enemyList.Add(other.gameObject);
            if (other.tag == null)
            {
                nowTarget = other.gameObject;
                
            }
        }
        camerascript.GetRockonTarget(nowTarget);
        Debug.Log("リストの中に入れた");
    }

    void OnTriggerExit(Collider other)
    {
        //当たっていなかったり指定したタグやレイヤーではなかったら変数を削除する
        if (other.tag=="Boss" && enemyList.Contains(other.gameObject) || other.tag == "Enemy" && enemyList.Contains(other.gameObject))
        {
            if (other.tag == null)
            {
                nowTarget = null;
            }
            enemyList.Remove(other.gameObject);
            Debug.Log("リストの中を削除");
        }
       
        
    }


    // Update is called once per frame
    void Update()
    {
        //Listの中に入っていなかったら変数をnullにする
        if(enemyList.Count==0)
        {
            nowTarget = null;
            return;
        }
        //違っていたら変数に格納する
        else if(enemyList.Count!=0&&nowTarget==null)
        {
            SetNowTarget();
        }
        for(int index =0;index<enemyList.Count;index++) //nullのオブジェクトから消す
        {
            if (enemyList[index]==null)
            {
                enemyList.Remove(enemyList[index]);
            }
        }
    }
    //他のスクリプトに参照するための関数
    public GameObject GetNowTarget()
    {
        return nowTarget;
    }
    //敵が複数いたらその分だけ格納する
    public void SetNowTarget()
    {
        foreach (var enemy in enemyList)
        {
            if(nowTarget == null)
            {
                nowTarget = enemy;
            }
        }
    }
    //別のオブジェクトをロックオンする
    public void OnRockonSwitch(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(enemyList.IndexOf(nowTarget)!=enemyList.Count-1)
            {
                nowTarget = enemyList[enemyList.IndexOf(nowTarget) + 1];
            }
            else
            {
                nowTarget = enemyList[0];
            }
        }
    }
}
