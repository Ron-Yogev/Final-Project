using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    private void Start()
    {

        StartCoroutine(Main.instance.web.getLevelVars("http://localhost/UnityBackend/retrieveVars.php"));
        StartCoroutine(Main.instance.web.getCustomLevel("http://localhost/UnityBackend/getCustomLevel.php", false));

    }
    public void playGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void newGame()
    {
        SceneManager.LoadScene("UploadImage");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void CustomChallenge()
    {
        RouteLevel.isCustomChallenge = true;
        SceneManager.LoadScene("Game");
    }
}
