using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoSprict : MonoBehaviour
{
    public Shader highLumiShader; //高輝度箇所を抽出するHighLumiShaderを割り当て。
    public Shader blurShader;     //ブラーをかけるBlurShaderを割り当て
    public Shader compoShader;    //テクスチャ合成用のシェーダ
    private Material highLumiMat;
    private Material blurMat;
    private Material compoMat;

    private void Awake()
    {
        highLumiMat = new Material(highLumiShader);
        blurMat = new Material(blurShader);
        compoMat = new Material(compoShader);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture highLumiTex = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
        RenderTexture blurTex0 = RenderTexture.GetTemporary(source.width, source.height,0, source.format);
        RenderTexture blurTex1 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
        RenderTexture buffTex = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);

        Graphics.Blit(source, highLumiTex, highLumiMat);
        blurMat.SetFloat("_AngleDeg", 45);  //角度調整
        Graphics.Blit(highLumiTex, blurTex0, blurMat);

        blurMat.SetFloat("_AngleDeg", 135);
        Graphics.Blit(highLumiTex, blurTex1, blurMat);

        compoMat.SetTexture("_BlurTex", blurTex0);
        Graphics.Blit(source, buffTex, compoMat);
        compoMat.SetTexture("_BlurTex", blurTex1);
        Graphics.Blit(buffTex, destination, compoMat);

        RenderTexture.ReleaseTemporary(highLumiTex);
        RenderTexture.ReleaseTemporary(blurTex0);
        RenderTexture.ReleaseTemporary(blurTex1);
        RenderTexture.ReleaseTemporary(buffTex);

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
