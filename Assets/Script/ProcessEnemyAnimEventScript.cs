using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessEnemyAnimEventScript : MonoBehaviour
{
    private MoveEnemyScript enemy;
    [SerializeField]
    private SphereCollider sphereCollider;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<MoveEnemyScript>();
    }

     public void AttackStart()
    {
        sphereCollider.enabled = true;
        Debug.Log("攻撃開始");
    }

    public void AttackEnd()
    {
        sphereCollider.enabled = false;
        Debug.Log("攻撃終了");
    }

    public void StateEnd()
    {
        enemy.SetState(MoveEnemyScript.EnemyState.Freeze);
        Debug.Log("固まる");
    }

    public void EndDamage()
    {
        enemy.SetState(MoveEnemyScript.EnemyState.Walk);
        Debug.Log("食らい終わった");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
