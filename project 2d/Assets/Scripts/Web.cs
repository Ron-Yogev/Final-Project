using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Web : MonoBehaviour
{

    public static int level = 1;
    public static int threshold = 80;
    public static int numPixels = 100000;
    public static int timeInSec = 120;
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

    public string getCurrentUser()
    {
        return user;
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
                    user = username;
                    Debug.Log(data);
                    level = Int32.Parse(data);
                    Debug.Log("level = " + level);

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

    public IEnumerator uploadImage(string uri, Texture2D img, Texture2D BWimg, string username, int threshold, int numPixels, int timeInSec)
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
        form.AddField("time", timeInSec);

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
                Debug.Log("before error");
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("before downloadHandler");
                byte[] bytes = www.downloadHandler.data;
                Debug.Log("before texture");
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);
                Debug.Log("before sprite");
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                Debug.Log("before callback");
                callback(sprite,isBW);
            }
        }
    }

    public IEnumerator getLevelVars(string uri)
    {
        Debug.Log("in getLevelVars");
        WWWForm form = new WWWForm();
        form.AddField("level", level); // post form in php

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            Debug.Log("in getLevelVars before yield");
            yield return www.SendWebRequest();
            Debug.Log("in getLevelVars after yield");
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("in getLevelVars error");
                Debug.Log(www.error);
            }
            else
            {
                string data = www.downloadHandler.text;
                Debug.Log("data = " + data);
                int[] vars = parseVars(data);
                threshold = vars[0];
                Debug.Log("vars[0] =  " + vars[0]);
                numPixels = vars[1];
                Debug.Log("vars[1] = " + vars[1]);
                timeInSec = vars[2];
                Debug.Log("vars[2] = " + vars[2]);
                Debug.Log("vars = " + vars);
            }
        }
    }

    public IEnumerator updateLevel(string uri)
    {
        Debug.Log("in update level");
        WWWForm form = new WWWForm();
        form.AddField("user", user); // post form in php

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            Debug.Log("before yield");
            yield return www.SendWebRequest();
            Debug.Log("after yield");

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("lo baini");
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("text rom php = "+www.downloadHandler.text);
                Debug.Log("update baini");
            }
        }

       StartCoroutine(Main.instance.web.getLevelVars("http://localhost/UnityBackend/retrieveVars.php"));

    }

    public int[] parseVars(string data)
    {
        string[] vars = data.Split(',');
        int[] ans = new int[3];
        for(int i=0;i<vars.Length; i++)
        {
            ans[i] = Int32.Parse(vars[i]);
        }
        return ans;
    }

    public static string getUser()
    {
        return user;
    }

    public static void setLevel(int level)
    {
        Web.level = level;
    }
}
