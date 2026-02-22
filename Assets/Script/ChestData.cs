using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChestData",menuName = "GameData/ChestCounter")]
public class ChestData : ScriptableObject
{
    public int chestCounter;  //何個の宝箱を開けたかを計測する
    //ゲームの最初は0にして初期化する
    public void Initialize()
    {
        chestCounter = 0;
    }
}
