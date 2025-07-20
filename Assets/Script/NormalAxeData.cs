using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalAxeData",menuName = "GameData/isAxeExplocion")]
public class NormalAxeData : ScriptableObject
{
    public bool isAxeExplocion;

    public void Initialize()
    {
        isAxeExplocion = false;
    }
}
