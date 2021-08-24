using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouteLevel : MonoBehaviour
{
    [SerializeField]
    Image colorImg;
    [SerializeField]
    Image bwImg;
    private const string retrieveUrl = "http://localhost/UnityBackend/retrieveImg.php";
    private const string retrieveCustomUrl = "http://localhost/UnityBackend/customRetrieve.php";

    public static bool isCustom = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart(0.3f));
    }

    IEnumerator LateStart(float waitTime)
    {
        Action<Sprite, bool> getSpriteCallback = (DownloadedImg, bw) =>
        {
            Debug.Log("before bwImg.sprite = DownloadedImg;");
            if (bw)
            {
                bwImg.sprite = DownloadedImg;
               // bwImg.overrideSprite = DownloadedImg;
            }
            else
            {
                colorImg.sprite = DownloadedImg;
               // colorImg.overrideSprite = DownloadedImg;
            }

        };
        yield return new WaitForSeconds(waitTime);
        Debug.Log("before StartCoroutine");
        string uri = retrieveUrl;
        if (isCustom)
        {
            uri = retrieveCustomUrl;
        }
        Debug.Log("uri = " + uri);
        StartCoroutine(Main.instance.web.retrieveImg(uri, Web.level, true, getSpriteCallback));
        StartCoroutine(Main.instance.web.retrieveImg(uri, Web.level, false, getSpriteCallback));

    }

    public Texture2D getColorImg()
    {
        return TextureToTexture2D(colorImg.mainTexture);
    }

    public Texture2D getBWImg()
    {
        return TextureToTexture2D(bwImg.mainTexture);
    }

    private Texture2D TextureToTexture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);
        return texture2D;
    }

    public void setOffCustom()
    {
        isCustom = false;
    }
}
