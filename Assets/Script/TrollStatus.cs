using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrollStatus : MonoBehaviour
{

    //敵のMaxHP
    [SerializeField]
    private int maxHp = 300;

    //敵のHP
    [SerializeField]
    private int hp;
    //敵の攻撃力
    [SerializeField]
    private int attackPower = 1;
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

    public int GetAttackPower()
    {
        return attackPower;
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

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        hpSlider = HPUI.transform.Find("HPBar").GetComponent<Slider>();
        hpSlider.value = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
