using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{

    // 敵のMaxHP
    [SerializeField]
    private int maxHp = 100;
    //敵のHP
    [SerializeField]
    private int hp;
    //敵の攻撃力
    [SerializeField]
    private int attackPower = 1;
    private MoveEnemyScript enemyscript;
    //HP表示用UI
    [SerializeField]
    private GameObject HPUI;
    //HP表示用スライダー
    private Slider hpSlider;

    public void SetHp(int hp)
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

    public int GetHp()
    {
        return hp;
    }

    public int GetMaxHp()
    {
        return maxHp;
    }

    // Start is called before the first frame update
    void Start()
    {
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

    public void UpdateHPValue()
    {
        hpSlider.value = (float)GetHp() / (float)GetMaxHp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
