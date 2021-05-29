using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Web : MonoBehaviour
{

    public static int level;
    public static string user;
    void Start()
    {
        // A correct website page.
        //StartCoroutine(GetDate("http://localhost/UnityBackend/GetDate.php"));

        //StartCoroutine(GetUsers("http://localhost/UnityBackend/GetUsers.php"));

       // StartCoroutine(Login("http://localhost/UnityBackend/Login.php","yogedv100", "1233456" ));

       // StartCoroutine(RegisterUser("http://localhost/UnityBackend/RegisterUser.php", "newnew", "111111"));

        // A non-existing page.
        //StartCoroutine(GetRequest("https://error.html"));
    }

    public IEnumerator GetDate(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }


    public IEnumerator GetUsers(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    public IEnumerator Login(string uri, string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username); // post form in php
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string data = www.downloadHandler.text;
                if (data != "Wrong Credentials." && data != "Username dosent exist.")
                {

                    int level = Int32.Parse(data);
                    Debug.Log("level = " + level);
                    //mainmenu.setLevel(level,username);

                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                }
            }
        }
    }

    public IEnumerator RegisterUser(string uri, string username, string password)
    {
        
        WWWForm form = new WWWForm();
        form.AddField("registerUser", username); // post form in php
        form.AddField("registerPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                if(www.downloadHandler.text == "Username dosent exist.")
                {
                    Toast.Instance.Show(www.downloadHandler.text, 3f, Toast.ToastColor.Red);
                }
                else if (www.downloadHandler.text == "Wrong Credentials.")
                {
                    Toast.Instance.Show(www.downloadHandler.text, 3f, Toast.ToastColor.Red);
                }
                else 
                {
                    Toast.Instance.Show(www.downloadHandler.text, 3f, Toast.ToastColor.Green);
                }
            }
        }
    }

    public IEnumerator uploadImage(string uri, Texture2D img, Texture2D BWimg, string username, int threshold, int numPixels)
    {
        WWWForm form = new WWWForm();
        Texture2D imgTexture = GetTextureCopy(img);
        byte[] texBytes = imgTexture.EncodeToPNG();

        Debug.Log("bytes in upload=" + texBytes.Length);

        Texture2D BWimgTexture = GetTextureCopy(BWimg);
        byte[] BWtexBytes = BWimgTexture.EncodeToPNG();

        form.AddField("username", username); // post form in php
        form.AddField("threshold", threshold);
        form.AddField("numPixels", numPixels);

        form.AddBinaryData("myimage", texBytes, "imagefronUnity.png", "image/png");

        form.AddBinaryData("mybwimage", BWtexBytes, "bwimagefronUnity.png", "image/png");


        WWW w = new WWW(uri, form);

        yield return w;

        if(w.error != null)
        {
            Debug.Log("error:" + w.error);
        }
        else
        {
            Debug.Log(w.text);

        }
        w.Dispose();

    }

    Texture2D GetTextureCopy(Texture2D source)
    {
        RenderTexture rt = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(source, rt);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D readbleTexture = new Texture2D(source.width, source.height);
        readbleTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readbleTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return readbleTexture;
    }

    public IEnumerator retrieveImg(string uri, int level, bool isBW, System.Action<Sprite,bool> callback)
    {
        Debug.Log("in retrieveImg");

        WWWForm form = new WWWForm();
        int bw;
        if (isBW) bw = 1;
        else bw = 0;

        form.AddField("level", level); // post form in php
        form.AddField("isBW", bw);
        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("in Web class - its good,  isBW = " + isBW);

                byte[] bytes = www.downloadHandler.data;
               
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);
                Debug.Log("syze bytes = " + bytes.Length);

                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                callback(sprite,isBW);


                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public static int getLevel()
    {
        return level;
    }

    public static string getUser()
    {
        return user;
    }

    public void setLevel(int level)
    {
        Web.level = level;
    }
}
