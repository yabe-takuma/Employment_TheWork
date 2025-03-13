using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionShockwave : MonoBehaviour
{
    //プレイヤーに必要な変数
    [SerializeField]
    private PlayerScript playerScript;
    [SerializeField]
    private CharacterController characterController;
    //------------//
    //トロルの攻撃力を参照するための変数
    [SerializeField]
    private TrollStatus trollStatus;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        characterController = GetComponent<CharacterController>();
        trollStatus = GetComponent<TrollStatus>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //ボスが特定の行動をしたらダメージを当てる処理
        if (hit.collider.tag == "wave" && playerScript.GetState() != PlayerScript.MyState.Damage
            &&playerScript.GetState()!=PlayerScript.MyState.Dead)
        {
            playerScript.Damage(trollStatus.GetAttackPower());
            Physics.IgnoreCollision(characterController, hit.collider, true);
            Debug.Log("接触");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
