using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class calculateScore : MonoBehaviour
{
    float score = 0;
    int numPixels = 73810; //db;
    Texture2D coloredImg, BWImg;
    Dictionary<int, Vector2> areasCoord;
    const int allchannels = 765;
    const int MAX_SCORE = 100;
    [SerializeField]
    int threshold = 80;
    [SerializeField]
    Button nextLevelBtn;
    [SerializeField]
    Button menuBtn;
    [SerializeField]
    bool tutorial;
    string username;


    private void Start()
    {
        if (tutorial)
        {
            numPixels = 12985;
        }
        else
        {

        }
        //username = .....
        ////numPixels = .....
    }

    public void setVariables(Texture2D color_img, Texture2D bw_img, Dictionary<int, Vector2> dic)
    {
        Debug.Log("set varbiales in calculate score");

        coloredImg = new Texture2D(color_img.width, color_img.height);
        coloredImg.SetPixels(color_img.GetPixels());
        coloredImg.Apply();
        Debug.Log("color_img: " + color_img);
        Debug.Log("bw_img: " + bw_img);
        BWImg = new Texture2D(bw_img.width, bw_img.height);
        BWImg.SetPixels(bw_img.GetPixels());
        BWImg.Apply();

        areasCoord = dic;

        Calculate();


    }

    public void Calculate()
    {
        // The ratio of the number of parts multiplied by the range of values of the 3 color channels divided by the highest result 
        //float decrease = (float)size * allchannels / MAX_SCORE;
        float R = 0, G = 0, B = 0;
        foreach (KeyValuePair<int, Vector2> item in areasCoord)
        {
            Debug.Log("item:" + item);
            Vector2 coord = item.Value;
            Color colorOrig = coloredImg.GetPixel((int)coord.x, (int)coord.y);
            Color colorUser = BWImg.GetPixel((int)coord.x, (int)coord.y);
            Debug.Log("orig" + colorOrig + ",user" + colorUser);
            R = Mathf.Abs(colorOrig.r - colorUser.r) * 255;
            G = Mathf.Abs(colorOrig.g - colorUser.g) * 255;
            B = Mathf.Abs(colorOrig.b - colorUser.b) * 255;
            float ratio = (allchannels - (R + G + B)) / allchannels; // when the error is bigger than 0 , the score decrease
            score += (item.Key / (float)numPixels) * 100 * ratio;

        }
        // round the score to integer

        textToUser();


    }

    public void textToUser()
    {
        if (score >= threshold)
        {
            //SoundManagerScript.PlaySound("win");
            gameObject.GetComponent<TextMeshProUGUI>().color = new Color(60 / 255f, 179 / 255f, 113 / 255f, 1f);
            nextLevelBtn.GetComponentInChildren<Text>().text = "Next Level!";
        }
        else
        {
            //SoundManagerScript.PlaySound("lose");
            gameObject.GetComponent<TextMeshProUGUI>().color = new Color(220 / 255f, 20 / 255f, 60 / 255f, 1f);
            nextLevelBtn.GetComponentInChildren<Text>().text = "Try Again";
        }
        gameObject.GetComponent<TextMeshProUGUI>().text = "SCORE : " + (int)score;
        if (tutorial)
        {
            nextLevelBtn.GetComponentInChildren<Text>().text = "Try Again";
        }

        menuBtn.gameObject.SetActive(true);
        nextLevelBtn.gameObject.SetActive(true);




    }

    public void finishLevel()
    {
        if (tutorial)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (score >= threshold)
        {
            
            // INITIAL NEW LEVEL
            //SceneManager.LoadScene(nextLevel); // get the user current level from db and update
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void finishToMenu()
    {
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }
}
