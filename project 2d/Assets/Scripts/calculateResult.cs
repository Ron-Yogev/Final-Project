using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * A class designed to calculate a player's score by color matching percentage
 */

public class calculateResult : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI score_txt = null; // the text we want to change
    [SerializeField] List<Renderer> rend_3d = null; // the 3d objets for compare their color values
    [SerializeField] List<SpriteRenderer> rend_2d = null; // the 2d objets for compare their color values
    [SerializeField] int size = 0;
    [SerializeField] const float MAX_SCORE = 100;
    [SerializeField] int threshold = 80;
    [SerializeField] string nextLevel = null;
    [SerializeField] Text nextLevel_buttonTxt = null;
    [SerializeField] GameObject next = null, menu = null;

    const int allchannels = 768;
    private float score = MAX_SCORE;

    public void Start()
    {
        next.SetActive(false);
        menu.SetActive(false);
    }

    /*
     * A function that goes through all 2 lists so that each member from one list matches the other element in the same index,
     * calculates the differences of the 3-color channels according to the material,
     * summarizes the error percentage and decrease the score accordingly
     */
    public int getScore()
    {
        score = MAX_SCORE;
        // The ratio of the number of parts multiplied by the range of values of the 3 color channels divided by the highest result 
        float decrease = (float)size * allchannels / MAX_SCORE; 
        float R = 0, G = 0, B = 0;
        for (int i = 0; i < size; i++)
        {
            R = Mathf.Abs(rend_3d[i].material.color.r - rend_2d[i].color.r)*256;
            G = Mathf.Abs(rend_3d[i].material.color.g - rend_2d[i].color.g)*256;
            B = Mathf.Abs(rend_3d[i].material.color.b - rend_2d[i].color.b)*256;
            float err = R + G + B; // when the error is bigger than 0 , the score decrease
            score -= err / decrease;
            
        }
        // round the score to integer
        return (int)score;
    }

    /*
     * Function that assign to 'score_txt' variable the whole text with the score
     */
    public void setScore()
    {
        int num = getScore();
        if(num >= threshold)
        {
            SoundManagerScript.PlaySound("win");
            score_txt.color = new Color(60/255f, 179/255f, 113/255f, 1f);
            nextLevel_buttonTxt.text = "Next Level!";
        }
        else
        {
            SoundManagerScript.PlaySound("lose");
            score_txt.color = new Color(220/255f, 20/255f, 60/255f,1f);
            nextLevel_buttonTxt.text = "Try Again";
        }
        score_txt.SetText("Your score is :" + num);
        if (size == 1)
        {
            nextLevel_buttonTxt.text = "Try Again";


        }
        menu.SetActive(true);
        next.SetActive(true);


    }

    public void finishLevel()
    {
        int num = getScore();
        if (num >= threshold)
        {
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void finishToMenu()
    {
        Cursor.visible = true;
        SceneManager.LoadScene("Menu");
    }

}
