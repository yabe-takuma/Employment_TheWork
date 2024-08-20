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

    // Start is called before the first frame update
    void Start()
    {
        mace = GetComponentInChildren<MaceScript>();
    }

    //攻撃開始時
    public void StartAttack()
    {
        mace.ChangeEnableAttack(true);
    }
    //攻撃終了時
    public void EndAttack()
    {
        mace.ChangeEnableAttack(false);
    }
    //衝撃波発生
    public void CreateShockwave()
    {
        Instantiate(shockwavePrefab, createShockwavePoint.position, shockwavePrefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
