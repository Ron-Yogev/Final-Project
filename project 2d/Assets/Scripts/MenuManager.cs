using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(Main.instance.web.getLevelVars("http://localhost/UnityBackend/retrieveVars.php"));
    }
    public void playGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void newGame()
    {
        SceneManager.LoadScene("UploadImage");
    }
}
