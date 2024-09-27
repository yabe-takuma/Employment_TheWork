using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class WeaponStatusScript : MonoBehaviour
{

    public enum WeaponType
    {
        Sword,
        Gun,
        Other,
        Axe
    }

    [SerializeField]
    private int attackPower;
    [SerializeField]
    private int shotPower;
    [SerializeField]
    private int axePower;
    [SerializeField]
    private WeaponType weaponType;
    [SerializeField]
    private float weaponRange;

    public int GetAttackPower()
    {
        return attackPower;
    }

    public int GetShotPower()
    {
        return shotPower;
    }

    public int GetAxePower()
    {
        return axePower;
    }

    public WeaponType GetWeaponType()
    {
        return weaponType;
    }

    public float GetWeaponRange()
    {
        return weaponRange;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
