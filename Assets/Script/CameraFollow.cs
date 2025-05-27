using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform
    public List<Transform> enemies = new List<Transform>(); //敵のリスト
    public float distance = 5.0f;  // プレイヤーとの距離
    public float height = 2.0f; //カメラの高さ
    public float rotationSpeed = 50.0f; //回転速度

    private float angle = 0.0f;
    private bool isLockedOn = false;  // ロックオン状態
    private Transform currentTarget = null;  //現在のターゲット
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ロックオンの切り替え (スペースキー)
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(enemies.Count > 0)
            {
                isLockedOn = !isLockedOn;
                if(isLockedOn)
                {
                    currentTarget = GetClosestEnemy();  //最初のターゲット
                }
            }
        }

        //矢印キーでカメラを回転
        if (!isLockedOn)  //ロックオンしていないときは自由に回転
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                angle -= rotationSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                angle += rotationSpeed * Time.deltaTime;
            }

            // カメラの位置を計算
            Vector3 offset = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * distance, height, Mathf.Cos(angle * Mathf.Deg2Rad) * distance);
            transform.position = player.position + offset;

            //プレイヤーを常に見る
            transform.LookAt(player);
        }
        else // ロックオン時はターゲットの敵を注視
        {
            if (currentTarget != null) 
            {
                transform.LookAt(currentTarget);
            }
        }
    }

    //最も近い敵を取得する関数
    Transform GetClosestEnemy()
    {
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(player.transform.position, enemy.position);
            if(distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

   
}
