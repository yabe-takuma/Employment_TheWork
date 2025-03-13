using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyStatus : MonoBehaviour
{
    //複数の武器を持つための変数
    private GameObject equip;
    [SerializeField]
    private int hp;
    //力
    [SerializeField]
    private int power;
    //武器ごとの攻撃力
    private WeaponStatusScript weaponStatus;

    //LifeGaugeスクリプト
    [SerializeField]
    private LifeGauge lifeGauge;
    //ダメージを受けた時減ったHPを格納するための関数
    public void SetHp(int hp)
    {
        this.hp = hp;
        //体力ゲージに反映
        lifeGauge.SetLifeGauge(hp);
    }
    //他のスクリプトに参照するための関数
    public int GetHp()
    {
        return hp;
    }
    //武器ごとに変数に格納してさらに武器によって攻撃力を変える処理
    public void SetEquip(GameObject weapon)
    {
        equip = weapon;
        weaponStatus = equip.GetComponent<WeaponStatusScript>();
    }
  
   
    //他のスクリプトに自身の力と剣の攻撃力を合わせたダメージ力を返す関数
    public int GetAttackPower()
    {
        return power + weaponStatus.GetAttackPower();
    }
    //他のスクリプトに自身の力と斧の攻撃力を合わせたダメージ力を返す関数
    public int GetAxeAttackPower()
    {
        return power + weaponStatus.GetAxePower();
    }

    // Start is called before the first frame update
    void Start()
    {
        //体力ゲージに反映
        lifeGauge.SetLifeGauge(hp);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
