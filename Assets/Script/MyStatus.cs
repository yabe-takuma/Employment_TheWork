using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyStatus : MonoBehaviour
{

   
    private GameObject equip;
    [SerializeField]
    private int hp;
    //力
    [SerializeField]
    private int power;
    private WeaponStatusScript weaponStatus;

    //LifeGaugeスクリプト
    [SerializeField]
    private LifeGauge lifeGauge;

    //BrackLifeスクリプト
    [SerializeField]
    private BrackLife bracklife;

    public void SetHp(int hp)
    {
        this.hp = hp;
        //体力ゲージに反映
        lifeGauge.SetLifeGauge(hp);
        //bracklife.SetLifeGauge();
    }

    public int GetHp()
    {
        return hp;
    }

    public void SetEquip(GameObject weapon)
    {
        equip = weapon;
        weaponStatus = equip.GetComponent<WeaponStatusScript>();
    }

    public GameObject GetEquip()
    {
        return equip;
    }

    public WeaponStatusScript GetWeaponStatus()
    {
        return weaponStatus;
    }

    //自身の力と武器の攻撃力を合わせたダメージ力を返す
    public int GetAttackPower()
    {
        return power + weaponStatus.GetAttackPower();
    }

    public int GetAxeAttackPower()
    {
        return power + weaponStatus.GetAxePower();
    }

    // Start is called before the first frame update
    void Start()
    {
        //体力ゲージに反映
        bracklife.SetLifeGauge();
        lifeGauge.SetLifeGauge(hp);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
