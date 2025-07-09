using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamegeUIScript : MonoBehaviour
{
    //ダメージを表示するために必要な変数
    [SerializeField]
    private TextMeshProUGUI damageText;

    //フェードアウトするスピード
    private float fadeOutSpeed = 1f;
    //移動値
    [SerializeField]
    private float moveSpeed = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        damageText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        UIhandling();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UIhandling()
    {
        //UIが徐々に上に行く処理
        transform.rotation = Camera.main.transform.rotation;
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        damageText.color = Color.Lerp(damageText.color, new Color(1f, 0f, 0f, 0f), fadeOutSpeed * Time.deltaTime);
        //UIが透明になったら消滅する処理
        if (damageText.color.a <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
