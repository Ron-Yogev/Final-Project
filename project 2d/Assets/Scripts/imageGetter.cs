using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class imageGetter : MonoBehaviour
{

    Texture2D origImg;
    Texture2D passedImg;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hiiiii");
        passedImg = gameManager.getImage();
        //origImg = new Texture2D(passedImg.width, passedImg.height);
        //origImg.SetPixels(passedImg.GetPixels());
        passedImg.Apply();
        gameObject.GetComponent<Image>().overrideSprite = Sprite.Create(passedImg, new Rect(0.0f, 0.0f, passedImg.width, passedImg.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

}
