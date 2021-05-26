using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * A class designed to combine the colors by inserting the methods into the sliders
 */
public class newMixedColor : MonoBehaviour
{
    private Color curr;
    private float C, M, Y;
    [SerializeField] Image CI = null, MI = null, YI = null;
    [SerializeField] Image CO = null, MO = null, YO = null;

    // Start is called before the first frame update
    void Start()
    {
        //initializing the class fields
        C = 0f;
        M = 0f;
        Y = 0f;
        curr = CMYKtoRGB(C, M, Y);
    }

    private void Update()
    {
    }

    /*
     * A function that is inserted into the red slider and changes the red color channel in the current color 
     */
    public void addCyanColor(float add)
    {
        C = -1f * (add/255f);
        CI.color = CMYKtoRGB(C, 0, 0);
        CO.color = CMYKtoRGB(C, 0, 0);
        curr = CMYKtoRGB(C, M, Y);
    }

    /*
     * A function that is inserted into the green slider and changes the red color channel in the current color 
     */
    public void addMagentaColor(float add)
    {
        M = -1f * (add / 255f);
        MI.color = CMYKtoRGB(0, M, 0);
        MO.color = CMYKtoRGB(0, M, 0);
        curr = CMYKtoRGB(C, M, Y);
    }

    /*
     * A function that is inserted into the blue slider and changes the red color channel in the current color 
     */
    public void addYellowColor(float add)
    {
        Y = -1f * (add / 255f);
        YI.color = CMYKtoRGB(0, 0, Y);
        YO.color = CMYKtoRGB(0, 0, Y);
        curr = CMYKtoRGB(C, M, Y);
    }

    /*
     * Function that returns the current color
     */
    public Color getCurrColor()
    {
        return curr;
    }


    /*
 * Function that turns CMYK color into RGB color
 */
    public Color CMYKtoRGB(float C, float M, float Y)
    {
        return new Color(1f + C, 1f + M, 1f + Y);
    }
}
