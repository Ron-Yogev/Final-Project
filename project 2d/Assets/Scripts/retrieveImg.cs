using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class retrieveImg : MonoBehaviour
{
    public bool isBW;
    public Image img;
    public string imgName;
    private int level;
    public Button retrieveBtn;
    private const string retrieveUrl = "http://localhost/UnityBackend/retrieveImg.php";

    // Start is called before the first frame update
    void Start()
    {

        level = 22;
        if (isBW)
        {
            imgName = "uncolor";
        }
        else
        {
            imgName = "color";
        }
        System.Action<Sprite> getSpriteCallback = (DownloadedImg) =>
        {
            //gameObject.GetComponent<Image>().overrideSprite
            Debug.Log("retrieveee to unityyyyyyyyyy,  isBW = " + isBW);
            GameObject.FindGameObjectWithTag(imgName).GetComponent<Image>().overrideSprite = DownloadedImg;
            //gameObject.GetComponent<Image>().overrideSprite = DownloadedImg;
           // gameObject.transform.Find(imgName).GetComponent<Image>().sprite = DownloadedImg;
        };

        retrieveBtn.onClick.AddListener(() =>
        {
            //StartCoroutine(Main.instance.web.retrieveImg(retrieveUrl, level, isBW, getSpriteCallback));
        });

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
