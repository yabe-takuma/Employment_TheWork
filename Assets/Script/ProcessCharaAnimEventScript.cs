using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessCharaAnimEventScript : MonoBehaviour
{
    private PlayerScript playerscript;
    [SerializeField]
    private Collider weaponCollider;
    //装備品の親のTransform
    [SerializeField]
    private Transform equip;

    // Start is called before the first frame update
    void Start()
    {
        playerscript = GetComponent<PlayerScript>();
        weaponCollider = equip.GetComponentInChildren<Collider>();
       
    }

    public void AttackStart()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            //if(equip.GetChild(0).CompareTag("Sword"))
            //{
            //    audioSorce.PlayOueShot(attackSound);
            //}
            Debug.Log("攻撃開始");
        }
       
    }

    public void AttackEnd()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
        
    }

    public void StateEnd()
    {
        playerscript.SetState(PlayerScript.MyState.Normal);
    }

    public void EndDamage()
    {
        if (playerscript.GetState() != PlayerScript.MyState.Dead)
        {
            playerscript.SetState(PlayerScript.MyState.Normal);
            Debug.Log("プレイヤー食らい終わった");
        }
    }

    public void SetCollider(Collider col)
    {
        weaponCollider = col;
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
