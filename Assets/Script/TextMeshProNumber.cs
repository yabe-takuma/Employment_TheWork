using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMeshProNumber : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMeshPro;
    [SerializeField]
    private int _deadCaunter = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int DeadCaunter
    {
        get => _deadCaunter;
        set{
            _deadCaunter = value;
            OnUpdateDeadCaunter(value);
        }
    }

    //表示する数値の更新
    private void OnUpdateDeadCaunter(int deadCaunter)
    {
        if (textMeshPro == null) return;

        //GC.Allocを発生させずに数値を指定可能
        //ただし、UnitorEditor上だと発生します
        textMeshPro.SetText("{0}", deadCaunter);
    }

    public void SubtractDeadCaunter(int deadCaunter)
    {
        _deadCaunter -= deadCaunter;
        if (_deadCaunter >= 0)
        {
            OnUpdateDeadCaunter(_deadCaunter);
        }
    }

#if UNITY_EDITOR
    //インスペクター等から編集された時に表示更新(エディタ専用)
    private void OnValidate()
    {
        OnUpdateDeadCaunter(_deadCaunter);
    }
#endif
    //public void UpdateTextMeshProText()
    //{
    //    //数値を文字列に変換
    //    string numberString = deadCaunter.ToString();

    //    // TextMeshProのtextプロパティに代入
    //    textMeshPro.text = numberString;
    //}

    //public void IncrementNumber()
    //{
    //    deadCaunter--;
    //    UpdateTextMeshProText();
    //}
}
