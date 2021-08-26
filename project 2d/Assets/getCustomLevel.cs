using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class getCustomLevel : MonoBehaviour
{
    public Button customBtn;
    private const string customLevelUrl = "http://localhost/UnityBackend/getCustomLevel.php";

    // Start is called before the first frame update
    void Start()
    {
        customBtn.onClick.AddListener(() =>
        {
            StartCoroutine(Main.instance.web.getCustomLevel(customLevelUrl, true));
        });

    }
}

