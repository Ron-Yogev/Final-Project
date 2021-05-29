using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uploadImg : MonoBehaviour
{
    public Button uploadBtn;
    public Image BWimg;
    public Image img;
    private int threshold;
    private int numPixels;
    private string username;
    private const string uploadUrl = "http://localhost/UnityBackend/uploaderImg.php";


    // Start is called before the first frame update
    void Start()
    {
        username = "ma ta mdaber";
        threshold = 85;
        numPixels = 100;
        uploadBtn.onClick.AddListener(() =>
        {
            StartCoroutine(Main.instance.web.uploadImage(uploadUrl, TextureToTexture2D(img.mainTexture), TextureToTexture2D(BWimg.mainTexture),
                username, threshold, numPixels));
        });

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
}
