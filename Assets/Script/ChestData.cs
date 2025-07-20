using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChestData",menuName = "GameData/ChestCounter")]
public class ChestData : ScriptableObject
{
    public int chestCounter;

    public void Initialize()
    {
        chestCounter = 0;
    }
}
