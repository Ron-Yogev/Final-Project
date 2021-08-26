using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    [SerializeField]
    public Image img;
    [SerializeField]
    public InputField url;
    [SerializeField]
    public Text comment;

    bool uploaded = false;

    public static Texture2D pass_img;
    public static Texture2D pass_imgBW;

    // Start is called before the first frame update
    public void buttonClick()
    {
        if (uploaded)
        {
            SceneManager.LoadScene("EdjustTheImage");

        }

        StartCoroutine(GetImageFromWeb(url.text));
    }

    

    public void buttonAgain()
    {
        SceneManager.LoadScene("UploadImage");
    }

    public void buttonBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator GetImageFromWeb(string x)
    {
        UnityEngine.Networking.UnityWebRequest req = UnityWebRequestTexture.GetTexture(x);
        req.SetRequestHeader("access-control-request-headers", "POST");
        
        yield return req.SendWebRequest();

        if(req.isNetworkError || req.isHttpError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Texture2D pic = ((DownloadHandlerTexture)req.downloadHandler).texture;
            if (pic.width > 400 || pic.height > 400)
            {
                comment.gameObject.SetActive(true);
            }
            else
            {
                img.sprite = Sprite.Create(pic, new Rect(0, 0, pic.width, pic.height), Vector2.zero);
                pass_img = new Texture2D(pic.width, pic.height);
                pass_img.SetPixels(pic.GetPixels());
                uploaded = true;
                GameObject.FindGameObjectWithTag("confirmURL").GetComponentInChildren<Text>().text = "Confirm";
                comment.gameObject.SetActive(false);
            }

        }

    }

    public static Texture2D getImage()
    {
        return pass_img;
    }

    public static void setImageBW(Texture2D t)
    {
        pass_imgBW = t;

    }

    public static Texture2D getImageBW()
    {
        return pass_imgBW;
    }

}
