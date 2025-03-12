using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutScript : MonoBehaviour
{
    [SerializeField]
    private Image panelImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        panelImage.color = Color.Lerp(panelImage.color, new Color(0, 0, 0, 0), 0.5f * Time.deltaTime);
    }
}
