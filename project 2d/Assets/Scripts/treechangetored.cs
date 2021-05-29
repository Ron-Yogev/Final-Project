using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class treechangetored : MonoBehaviour
{
    [SerializeField] Texture2D pic = null;
    Texture2D readble;
    Texture2D helper;
    bool flag;

    // Start is called before the first frame update
    void Start()
    {
        duplicateTexture(pic);

    }

    void duplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        readble = new Texture2D(source.width, source.height);
        readble.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readble.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.flag = false;
            getCorrectPixelMouseClick(Input.mousePosition);
        }
    }

    void getCorrectPixelMouseClick(Vector2 dat)
    {
        Vector2 localCursor;
        var rect1 = GetComponent<RectTransform>();
        var pos1 = dat;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect1, pos1,
            null, out localCursor))
            return;

        int xpos = (int)(localCursor.x);
        int ypos = (int)(localCursor.y);

        if (xpos < 0) xpos = xpos + (int)rect1.rect.width / 2;
        else xpos += (int)rect1.rect.width / 2;

        if (ypos > 0) ypos = ypos + (int)rect1.rect.height / 2;
        else ypos += (int)rect1.rect.height / 2;

        if (xpos < 0 || ypos < 0 || xpos > rect1.sizeDelta.x || ypos > rect1.sizeDelta.y) return;
        int x = (int)(readble.width * (xpos / rect1.sizeDelta.x));
        int y = (int)(readble.height * (ypos / rect1.sizeDelta.y));
        //Debug.Log("x: " + x + " y:" + y);
        Debug.Log("color " + readble.GetPixel(x, y));
        if (readble.GetPixel(x, y) == new Color(0f, 0f, 0f, 1f)) return;
        helper = new Texture2D(readble.width, readble.height);
        helper.SetPixels(readble.GetPixels());
        helper.Apply();
        float r = 100 / 255f;
        float g = 102 / 255f;
        float b = 171 / 255f;
        Color c = new Color(r, g, b, 255 / 255f);
        floodFill4Stack(c, x, y);

        readble.Apply();
        gameObject.GetComponent<Image>().overrideSprite = Sprite.Create(readble, new Rect(0.0f, 0.0f, readble.width, readble.height), new Vector2(0.5f, 0.5f), 100.0f);


    }

    public static readonly int[] dx = { 0, 1, 0, -1 }; // relative neighbor x coordinates
    public static readonly int[] dy = { -1, 0, 1, 0 }; // relative neighbor y coordinates

    //4-way floodfill using our own stack routines
    void floodFill4Stack(Color targetColor, int x, int y)
    {
        if (!CheckValidity(x, y, targetColor)) return; //avoid infinite loop

        int iteration = 0;
        //Stack<Vector2> st = new Stack<Vector2>();
        //st.Push(new Vector2(x, y));
       
        HashSet<Vector2> hs = new HashSet<Vector2>();
        hs.Add(new Vector2(x, y));
        while (hs.Count > 0 && !this.flag)
        {

            Vector2 point = hs.ElementAt(hs.Count - 1);
            hs.Remove(point);
            //st.Pop();
            readble.SetPixel((int)point.x, (int)point.y, targetColor);
            //screenBuffer[y * w + x] = newColor;
            for (int i = 0; i < 4; i++)
            {
                int nx = (int)point.x + dx[i];
                int ny = (int)point.y + dy[i];
                if (CheckValidity(nx, ny, targetColor))
                {
                    hs.Add(new Vector2(nx, ny)); 
                }
                //iteration++;
            }
            iteration++;
        }
        if (this.flag)
        {
            readble.SetPixels(helper.GetPixels());
        }
        Debug.Log("iterations: " + iteration);
    }

    bool CheckValidity(int x , int y, Color TargetColor)
    {   
        if(readble.GetPixel(x, y) == TargetColor)
        {
            return false;
        }
        if(readble.GetPixel(x, y) == new Color(0f, 0f, 0f, 1f))
        {
            return false;
        }
        if (x <= 0 || x >= readble.width-1 || y <= 0 || y >= readble.height-1)
        {
            this.flag = true;
            return false;
        }

        return true;
    }

}
