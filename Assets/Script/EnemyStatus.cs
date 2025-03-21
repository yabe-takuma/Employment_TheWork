using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{

    // 敵のMaxHP
    [SerializeField]
    private float maxHp;
    //敵のHP
    [SerializeField]
    private float hp;
    //敵の攻撃力
    [SerializeField]
    private int attackPower;
    private MoveEnemyScript enemyscript;
    //HP表示用UI
    [SerializeField]
    private GameObject HPUI;
    //HP表示用スライダー
    private Slider hpSlider;
    [SerializeField]
    private string filePath = "Assets/Resources/EnemySetting.asset";
    [SerializeField]
    private EnemySetting enemySetting;




    public void SetHp(float hp)
    {
        this.hp = hp;

        //HP表示用UIのアップデート
        UpdateHPValue();

        if(hp<=0)
        {
            //HP表示用UIを非表示にする
            HideStatusUI();
        }

    }

    public float GetHp()
    {
        return hp;
    }

    public float GetMaxHp()
    {
        return maxHp;
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        attackPower = 1;
        enemyscript = GetComponent<MoveEnemyScript>();
        hp = maxHp;
        hpSlider = HPUI.transform.Find("HPBar").GetComponent<Slider>();
        hpSlider.value = 1f;
    }

    //死んだらHPUIを非表示にする
    public void HideStatusUI()
    {
        HPUI.SetActive(false);
    }
    //敵の体力を表すUI
    public void UpdateHPValue()
    {
        hpSlider.value = (float)GetHp() / (float)GetMaxHp();
    }
    //他のスクリプトに参照するための関数
    public int GetAttackPower()
    {
        return attackPower;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
