using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Web : MonoBehaviour
{
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
                Debug.Log(www.downloadHandler.text);
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
}
