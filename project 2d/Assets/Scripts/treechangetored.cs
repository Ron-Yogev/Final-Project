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
        floodFill4Stack(new Color(100 / 255f, 102 / 255f, 171 / 255f, 255 / 255f), x, y);

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

   /* void floodFillScanlineStack(Color targetColor, int x, int y)
    {
        if (!CheckValidity(new Vector2(x,y), targetColor)) return;

        int x1;
        bool spanAbove, spanBelow;

        //HashSet<Vector2> hs = new HashSet<Vector2>();
        // hs.Add(new Vector2(x, y));

        Stack<Vector2> st = new Stack<Vector2>();
        st.Push(new Vector2(x, y));

        while (st.Count > 0 && !this.flag)
        {
            st.Pop();
            x1 = x;
            while (CheckValidity(new Vector2(x1, y), targetColor)screenBuffer[y * w + x1] == oldColor) x1--;
            x1++;
            spanAbove = false;
            spanBelow = false;
            while (x1 < readble.width && CheckValidity(new Vector2(x1, y), targetColor))
            {
                //screenBuffer[y * w + x1] = newColor;
                readble.SetPixel(x1, y, targetColor);
                if (!spanAbove && y > 0 && CheckValidity(new Vector2(x1, y -1), targetColor))
                {
                    st.Push(new Vector2(x1, y-1));
                    //hs.Add(new Vector2(x1, y-1));
                    //push(stack, x1, y - 1);
                    spanAbove = true;
                }
                else if (spanAbove && y > 0 && !CheckValidity(new Vector2(x1, y - 1), targetColor))
                {
                    spanAbove = false;
                }
                if (!spanBelow && y < readble.height - 1 && CheckValidity(new Vector2(x1, y + 1), targetColor))
                {
                    st.Push(new Vector2(x1, y + 1));
                    //push(stack, x1, y + 1);
                    // hs.Add(new Vector2(x1, y + 1));
                    spanBelow = true;
                }
                else if (spanBelow && y < readble.height - 1 && !CheckValidity(new Vector2(x1, y + 1), targetColor))
                {
                    spanBelow = false;
                }
                x1++;
            }
        }
        if (this.flag)
        {
            readble.SetPixels(helper.GetPixels());
        }
    }*/

   /* void MyFill(Color targetColor, int x, int y)
    {
        bool[,] array = new bool[readble.width, readble.height];
        if (!array[y, x] && CheckValidity(new Vector2(y,x ), targetColor)) _MyFill(targetColor, array, x, y);
    }*/

   /*void _MyFill(Color targetColor, bool[,] array, int x, int y)
    {
        Debug.Log("_MyFill");
        
        // at this point, we know array[y,x] is clear, and we want to move as far as possible to the upper-left. moving
        // up is much more important than moving left, so we could try to make this smarter by sometimes moving to
        // the right if doing so would allow us to move further up, but it doesn't seem worth the complexity
        while (true)
        {
            int ox = x, oy = y;
            while (y != 0 && !array[y - 1, x] && CheckValidity(new Vector2(y-1, x), targetColor)) y--;
            while (x != 0 && !array[y, x - 1] && CheckValidity(new Vector2(y , x-1), targetColor)) x--;
            if (x == ox && y == oy) break;
        }

        MyFillCore(targetColor, array, x, y);
    }*/

   /*void MyFillCore(Color targetColor, bool [,] array, int x, int y)
    {

        Debug.Log("MyFillCore");
        Debug.Log("point: (" + y + "," + x + ")");
        // at this point, we know that array[y,x] is clear, and array[y-1,x] and array[y,x-1] are set.
        // we'll begin scanning down and to the right, attempting to fill an entire rectangular block
        int lastRowLength = 0; // the number of cells that were clear in the last row we scanned

        
        do
        {
            if (!CheckValidity(new Vector2(y, x), targetColor))
            {
                return;
            }
            Debug.Log("MyFillCore after validity");
            int rowLength = 0, sx = x; // keep track of how long this row is. sx is the starting x for the main scan below
                                       // now we want to handle a case like |***|, where we fill 3 cells in the first row and then after we move to
                                       // the second row we find the first  | **| cell is filled, ending our rectangular scan. rather than handling
                                       // this via the recursion below, we'll increase the starting value of 'x' and reduce the last row length to
                                       // match. then we'll continue trying to set the narrower rectangular block
            if (lastRowLength != 0 && array[y, x]) // if this is not the first row and the leftmost cell is filled...
            {
                do
                {
                    if (--lastRowLength == 0) return; // shorten the row. if it's full, we're done
                } while (array[y, ++x]); // otherwise, update the starting point of the main scan to match
                sx = x;
            }
            // we also want to handle the opposite case, | **|, where we begin scanning a 2-wide rectangular block and
            // then find on the next row that it has     |***| gotten wider on the left. again, we could handle this
            // with recursion but we'd prefer to adjust x and lastRowLength instead
            else
            {
                for (; x != 0 && !array[y, x - 1]; rowLength++, lastRowLength++)
                {
                    
                    array[y, --x] = true; // to avoid scanning the cells twice, we'll fill them and update rowLength here
                                          // if there's something above the new starting point, handle that recursively. this deals with cases
                                          // like |* **| when we begin filling from (2,0), move down to (2,1), and then move left to (0,1).
                                          // the  |****| main scan assumes the portion of the previous row from x to x+lastRowLength has already
                                          // been filled. adjusting x and lastRowLength breaks that assumption in this case, so we must fix it
                    readble.SetPixel(y, x, targetColor);
                    if (y != 0 && !array[y - 1, x]) _MyFill(targetColor, array, x, y - 1); // use _Fill since there may be more up and left
                }
            }

            // now at this point we can begin to scan the current row in the rectangular block. the span of the previous
            // row from x (inclusive) to x+lastRowLength (exclusive) has already been filled, so we don't need to
            // check it. so scan across to the right in the current row
            for (; sx < readble.width && !array[y, sx]; rowLength++, sx++)
            {
                Debug.Log("point: (" + y + "," + x + ")");
                array[y, sx] = true;
                readble.SetPixel(y, sx, targetColor);
            }
            // now we've scanned this row. if the block is rectangular, then the previous row has already been scanned,
            // so we don't need to look upwards and we're going to scan the next row in the next iteration so we don't
            // need to look downwards. however, if the block is not rectangular, we may need to look upwards or rightwards
            // for some portion of the row. if this row was shorter than the last row, we may need to look rightwards near
            // the end, as in the case of |*****|, where the first row is 5 cells long and the second row is 3 cells long.
            // we must look to the right  |*** *| of the single cell at the end of the second row, i.e. at (4,1)
            if (rowLength < lastRowLength)
            {
                for (int end = x + lastRowLength; ++sx < end;) // 'end' is the end of the previous row, so scan the current row to
                {                                          // there. any clear cells would have been connected to the previous
                    if (!array[y, sx])
                        MyFillCore(targetColor, array, sx, y); // row. the cells up and left must be set so use FillCore
                }
            }
            // alternately, if this row is longer than the previous row, as in the case |*** *| then we must look above
            // the end of the row, i.e at (4,0)                                         |*****|
            else if (rowLength > lastRowLength && y != 0) // if this row is longer and we're not already at the top...
            {
                for (int ux = x + lastRowLength; ++ux < sx;) // sx is the end of the current row
                {
                    if (!array[y - 1, ux]) _MyFill(targetColor, array, ux, y - 1); // since there may be clear cells up and left, use _Fill
                }
            }
            lastRowLength = rowLength; // record the new row length
        } while (lastRowLength != 0 && ++y < readble.height && !this.flag); // if we get to a full row or to the bottom, we're done
        if (this.flag)
        {
            readble.SetPixels(helper.GetPixels());
        }
    }*/

    /*void ScanlineFTFill(Color targetColor, int x, int y)
    {
        int height = readble.width;
        int width = readble.height;

        int iterations = 0;

        bool[,] array = new bool[readble.width, readble.height];

        // we'll maintain a stack of points representing horizontal line segments that need to be filled.
        // for each point, we'll fill left and right until we find the boundaries
        Stack<Vector2> points = new Stack<Vector2>();
        points.Push(new Vector2(x, y)); // add the initial point
        do
        {
            Vector2 pt = points.Pop(); // pop a line segment from the stack
                                     // we'll keep track of the transitions between set and clear cells both above and below the line segment that
                                     // we're filling. on a transition from a filled cell to a clear cell, we'll push that point as a new segment
            bool setAbove = true, setBelow = true; // initially consider them set so that a clear cell is immediately pushed
            for (x = (int)pt.x; x < width && !array[(int)pt.y, x]; x++) // scan to the right
            {
                array[(int)pt.y, x] = true;
                if ((int)pt.y > 0 && array[(int)(pt.y - 1), x] != setAbove && CheckValidity(new Vector2((int)pt.y, x), targetColor)) // if there's a transition in the cell above...
                {
                    setAbove = !setAbove;
                    if (!setAbove )
                        readble.SetPixel((int)pt.y, x, targetColor);
                        points.Push(new Vector2(x, pt.y - 1)); // push the new point if it transitioned to clear
                }
                if (pt.y < height - 1 && array[(int)(pt.y + 1), x] != setBelow) // if there's a transition in the cell below...
                {
                    setBelow = !setBelow;
                    if (!setBelow)
                        readble.SetPixel((int)pt.y, x, targetColor);
                        points.Push(new Vector2(x, pt.y + 1));
                }

                iterations++;
            }

            if (pt.x > 0) // now we'll scan to the left, if there's anything to the left
            {
                // this time, we want to initialize the flags based on the actual cell values so that we don't add the line
                // segments twice. (e.g. if it's clear above, it needs to transition to set and then back to clear.)
                setAbove = pt.y > 0 && array[(int)(pt.y - 1), (int)pt.x];
                setBelow = pt.y < height - 1 && array[(int)(pt.y + 1), (int)pt.x];
                for (x = (int)(pt.x - 1); x >= 0 && !array[(int)pt.y, x]; x--) // scan to the left
                {
                    array[(int)pt.y, x] = true;
                    if (pt.y > 0 && array[(int)(pt.y - 1), x] != setAbove && CheckValidity(new Vector2((int)pt.y, x), targetColor)) // if there's a transition in the cell above...
                    {
                        setAbove = !setAbove;
                        if (!setAbove)
                            readble.SetPixel((int)pt.y, x, targetColor);
                            points.Push(new Vector2(x, pt.y - 1)); // push the new point if it transitioned to clear
                    }
                    if (pt.y < height - 1 && array[(int)(pt.y + 1), x] != setBelow && CheckValidity(new Vector2((int)pt.y, x), targetColor)) // if there's a transition in the cell below...
                    {
                        setBelow = !setBelow;
                        if (!setBelow)
                            readble.SetPixel((int)pt.y, x, targetColor);
                            points.Push(new Vector2(x, pt.y + 1));
                    }
                    iterations++;
                }
            }
        } while (points.Count != 0 && !this.flag);
        if (this.flag)
        {
            readble.SetPixels(helper.GetPixels());
        }
        Debug.Log("iterations: " + iterations);
    }*/

    /*public void FloodFill( Color targetColor, int x, int y)
    {
        Debug.Log("target color: " + targetColor);
        Debug.Log("readble width: " + readble.width + " height: " + readble.height);
        //bool[,] flag = new bool[readble.width, readble.height];
        //var q = new Queue<Vector2>(readble.width * readble.height);
        HashSet<Vector2> hs = new HashSet<Vector2>();
        hs.Add(new Vector2(x, y));
       // q.Enqueue(new Vector2(x, y));
        int iterations = 0;

        while (hs.Count > 0 && !this.flag)
        {
            //Vector2 point = q.Dequeue();
            Vector2 point = hs.ElementAt(hs.Count - 1);
            hs.Remove(point);
            int x1 = (int)point.x;
            int y1 = (int)point.y;
            Debug.Log("current point:(" +x1+","+y1+")");

            readble.SetPixel(x1, y1, targetColor);
            

            var newPoint = new Vector2(x1 + 1, y1);
            if (CheckValidity(newPoint, targetColor)) //&& flag[(int)x1, (int)y1] == false
                hs.Add(newPoint);
                //q.Enqueue(newPoint);

            newPoint = new Vector2(x1 - 1, y1);
            if (CheckValidity(newPoint, targetColor) )
                hs.Add(newPoint);
            //q.Enqueue(newPoint);

            newPoint = new Vector2(x1, y1 + 1);
            if (CheckValidity(newPoint, targetColor) )
                hs.Add(newPoint);
            // q.Enqueue(newPoint);

            newPoint = new Vector2(x1, y1 - 1);
            if (CheckValidity(newPoint, targetColor) )
                hs.Add(newPoint);
            // q.Enqueue(newPoint);
            //flag[x1, y1] = true;
            iterations++;
            
        }

        if (this.flag)
        {
            readble.SetPixels(helper.GetPixels());
        }

        Debug.Log("iterations: " + iterations);
    }*/

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
