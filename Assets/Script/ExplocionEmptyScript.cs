using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplocionEmptyScript : MonoBehaviour
{
    //オブジェクトを徐々に大きくするための変数
    private Vector3 omentimer;
    [SerializeField]
    private PlayerScript playerScript;
    //敵やボスにダメージを与えるのに必要な変数
    [SerializeField]
    private TrollScript trollScript;
    [SerializeField]
    private MeshRenderer meshrenderer;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.Find("Character_Female_Hotel Owner").GetComponent<PlayerScript>();
        trollScript = GameObject.Find("GiantTroll").GetComponent<TrollScript>();
        omentimer = new Vector3(0.4f, 0, 0.4f);
        meshrenderer.material.color = meshrenderer.material.color - new Color32(0, 0, 0, 255);
    }

    // Update is called once per frame
    void Update()
    {
        //特定のサイズになるまで大きくする処理
        if (transform.localScale.z <= 20)
        {
            transform.localScale += omentimer;

        }
        //特定のサイズ以上になったら削除するための処理
        if (transform.localScale.z >= 20)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //爆風が当たったらプレイヤーが倒れる処理
        if (other.tag == "Player" && playerScript.GetState() != PlayerScript.MyState.Damage && playerScript.GetState() != PlayerScript.MyState.Dead &&
          playerScript.GetAvoid() == false && playerScript.GetState() != PlayerScript.MyState.SkillAttack && trollScript.GetState() != TrollScript.TrollState.Dead)
        {
            playerScript.KnockBack(2);
        }
    }
}
