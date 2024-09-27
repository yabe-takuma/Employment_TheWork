using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaknessUIScript : MonoBehaviour
{
    [SerializeField]
    private Text weakText;
   

    //フェードアウトするスピード
    private float fadeOutSpeed = 1f;
    //移動値
    [SerializeField]
    private float moveSpeed = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        weakText = GetComponentInChildren<Text>();
    }

    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        weakText.color = Color.Lerp(weakText.color, new Color(1f, 0f, 0f, 0f), fadeOutSpeed * Time.deltaTime);
        
        if(weakText.color.a<=0.1f)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
