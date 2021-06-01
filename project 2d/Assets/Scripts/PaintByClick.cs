using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PaintByClick : MonoBehaviour
{
    [SerializeField] private Color cur_color = new Color();
    private Color temp_curr = new Color();
    [SerializeField] private Vector3 mouse_pos;
    [SerializeField]
    GameObject[] slider_array;
    bool mixActive;
    Color BucketColor;
    [SerializeField] Texture2D brushIcon = null;
    [SerializeField] Texture2D erasehIcon = null;
    [SerializeField] Texture2D pointerhIcon = null;
    [SerializeField] TextMeshProUGUI clock = null;
    [SerializeField] List<Button> buttons = null;
    [SerializeField] bool tutorial = false;
    private bool isRunning = false;
    private bool onErase = false;
    private bool onPointer = false;
    GameObject image_curr_color;
    Texture2D img;


    // Start is called before the first frame update
    void Start()
    {
        int h = brushIcon.height;
        int w = brushIcon.width;
        Vector2 tmp = new Vector2(w * 0.2f, h * 0.8f);
        Cursor.SetCursor(brushIcon, tmp, CursorMode.ForceSoftware);
        isRunning = false;
        image_curr_color = GameObject.FindGameObjectWithTag("currColor");
        BucketColor = new Color();
        mixActive = false;
        slider_array = GameObject.FindGameObjectsWithTag("slider");
        // pass over all sliders and hide them
        for (int i = 0; i < slider_array.Length; i++)
        {
            slider_array[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!tutorial)
        {
            isRunning = clock.GetComponent<CountBackTime>().isRunning();
        }
        if (isRunning || tutorial)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Input.mousePosition;
                //get the position of the click
                RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(pos)); ;

                if (true)
                {
                    //when the player click on one of the color spheres - the current color is update
                    if (hit.collider != null && hit.transform.tag == "color" && !onErase && !onPointer)
                    {
                        cur_color = hit.collider.gameObject.GetComponent<SpriteRenderer>().color;//.GetColor("_Color");
                        temp_curr = cur_color;
                        image_curr_color.GetComponent<Image>().color = cur_color;
                    }

                    // when the player click on one of the peaces on the canvas - the peace is colored
                    else if (!onPointer)
                    {
                        if (onErase)
                        {
                            if (checkBorders(pos))
                            {
                                /*img =*/ GameObject.FindGameObjectWithTag("paintable").GetComponent<FloodFill>().getCorrectPixelMouseClick(pos, Color.white);
                            }

                        }
                        else
                        {

                            if (checkBorders(pos))
                            {
                                /*img =*/ GameObject.FindGameObjectWithTag("paintable").GetComponent<FloodFill>().getCorrectPixelMouseClick(Input.mousePosition,cur_color);
                            }
                            
                        }
                    }

                    // when the player click on one of the uncolor spheres - the spehre is colored
                    if (hit.collider != null && hit.transform.tag == "uncolor")
                    {
                        if (mixActive)
                        {
                            //SoundManagerScript.PlaySound("combine");
                            BucketColor = hit.collider.gameObject.GetComponent<newMixedColor>().getCurrColor();
                            cur_color = BucketColor;
                            temp_curr = cur_color;
                            image_curr_color.GetComponent<Image>().color = cur_color;
                            hit.collider.gameObject.GetComponent<SpriteRenderer>().color = BucketColor;

                        }
                        else
                        {
                            cur_color = hit.collider.gameObject.GetComponent<SpriteRenderer>().color;//.material.GetColor("_Color");
                            temp_curr = cur_color;
                            image_curr_color.GetComponent<Image>().color = cur_color;
                        }
                    }
                }
            }
        }
        //the game is over
        else
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].interactable = false;
            }

        }
    }

    bool checkBorders(Vector2 dat)
    {
        Vector2 localCursor;
        var rect1 = GameObject.FindGameObjectWithTag("paintable").GetComponent<RectTransform>();
        var pos1 = dat;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect1, pos1,
            null, out localCursor))
        {
            Debug.Log("its nullll in local cursor");
            return false;
        }

        int xpos = (int)(localCursor.x);
        int ypos = (int)(localCursor.y);

        if (xpos < 0) xpos = xpos + (int)rect1.rect.width / 2;
        else xpos += (int)rect1.rect.width / 2;

        if (ypos > 0) ypos = ypos + (int)rect1.rect.height / 2;
        else ypos += (int)rect1.rect.height / 2;

        if (xpos < 0 || ypos < 0 || xpos > rect1.sizeDelta.x || ypos > rect1.sizeDelta.y)
        {
            Debug.Log("its nullll in border pos");
            return false;
        }
        return true;
    }

    /*
     * Functino that assign true to the onErease field 
     */
    public void setOnErase()
    {
        //SoundManagerScript.PlaySound("click");
        onErase = true;
    }
    /*
     * Functino that assign false to the onErease field
     */
    public void setOfErase()
    {
        onErase = false;
    }
    /*
     * Functino that assign true to the onPointer field
     */
    public void setOnPointer()
    {
        //SoundManagerScript.PlaySound("click");
        onPointer = true;

    }
    /*
     * Functino that assign false to the onPointer field
     */
    public void setOfPointer()
    {
        onPointer = false;
    }

    /*
     * Functino that assign true to the isRunning field
     */
    public void setNoRunning()
    {
        isRunning = false;
    }
    /*
     * Function that oncover all the sliders
     */
    public void openSliders()
    {
        for (int i = 0; i < slider_array.Length; i++)
        {
            slider_array[i].SetActive(true);
        }
        mixActive = true;

    }

    /*
     * Function that hide all the sliders
     */
    public void closeSliders()
    {
        //SoundManagerScript.PlaySound("click");
        for (int i = 0; i < slider_array.Length; i++)
        {
            slider_array[i].SetActive(false);
        }
        mixActive = false;
    }

    public void setEraseIcon()
    {
        int h = erasehIcon.height;
        int w = erasehIcon.width;
        Vector2 tmp = new Vector2(w * 0.2f, h * 0.8f);
        Cursor.SetCursor(erasehIcon, tmp, CursorMode.ForceSoftware);
    }

    public void setBrushIcon()
    {
        int h = brushIcon.height;
        int w = brushIcon.width;
        Vector2 tmp = new Vector2(w * 0.2f, h * 0.8f);
        Cursor.SetCursor(brushIcon, tmp, CursorMode.ForceSoftware);
    }

    public void setPointerIcon()
    {
        int h = pointerhIcon.height;
        int w = pointerhIcon.width;
        Vector2 tmp = new Vector2(w * 0.2f, h * 0.2f);
        Cursor.SetCursor(pointerhIcon, tmp, CursorMode.ForceSoftware);
    }

    public bool isTutorial()
    {
        return tutorial;
    }
}