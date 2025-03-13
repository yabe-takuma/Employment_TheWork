using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵のステータスを1箇所で作ることが出来る処理
[Serializable]
public class EnemyData
{
    public string Id;
    public int MaxHp;
    public int Hp;
    public int Attack;
}
[CreateAssetMenu(menuName = "ScriptableObject/Enemy Setting", fileName =
    "EnemySetting")]
public class EnemySetting : ScriptableObject 
{
    public List<EnemyData> DataList;
}


