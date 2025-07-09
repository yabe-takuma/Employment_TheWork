using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeponEnemyScript : MonoBehaviour
{

    [SerializeField]
    private GameObject[] wepons;

    private WeponName weponsName;

    public enum WeponName
    {
        Sword,
        Axe
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void SetWeponCaunter(WeponName weponname)
    {
        weponsName = weponname;
    }
}
