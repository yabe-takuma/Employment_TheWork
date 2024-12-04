using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    [SerializeField]
    private EnemyStatus enemyStatus;

    [SerializeField]
    private PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        enemyStatus = transform.root.GetComponent<EnemyStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Z_Arm") && other.tag == "Player"&&playerScript.GetAvoid()==false&&playerScript.GetState()!=PlayerScript.MyState.Dead)
        {
            Debug.Log("当たり");
            other.GetComponent<PlayerScript>().TakeDamage(transform.root, other.ClosestPoint(transform.position), enemyStatus.GetAttackPower());

        }
       
    }

    public void SetPlayer(PlayerScript player)
    {
        playerScript = player;
        Debug.Log("プレイヤーの情報を渡した。");
    }

}
