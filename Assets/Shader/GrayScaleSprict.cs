using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayScaleSprict : MonoBehaviour
{
    public Shader shader;
    private Material material;
    private Material gaussianBlurMat; //ガウシアンブラー用のマテリアル

   

    private void Awake()
    {
        material = new Material(shader);
        gaussianBlurMat = new Material(shader);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture buf1 = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, source.format);
        RenderTexture buf2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, source.format);
        RenderTexture buf3 = RenderTexture.GetTemporary(source.width / 8, source.height / 8, 0, source.format);
        RenderTexture blurTex = RenderTexture.GetTemporary(source.width, source.height, 0, source.format); //高輝度箇所をぼかしたものを格納するためのテクスチャ

        Graphics.Blit(source, buf1);
        Graphics.Blit(buf1, buf2);
        Graphics.Blit(buf2, buf3);
        Graphics.Blit(buf3, blurTex, gaussianBlurMat);//ガウシアンブラーのマテリアルを適用
        Graphics.Blit(buf3, buf2);
        Graphics.Blit(buf2, buf1);
        Graphics.Blit(buf1, destination);
        Graphics.Blit(source, destination, material);

        RenderTexture.ReleaseTemporary(buf1);  //確保した分は必ずRelease
        RenderTexture.ReleaseTemporary(buf2);
        RenderTexture.ReleaseTemporary(buf3);
        RenderTexture.ReleaseTemporary(blurTex);
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1000, false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
