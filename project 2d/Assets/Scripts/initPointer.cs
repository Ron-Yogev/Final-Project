using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initPointer : MonoBehaviour
{
    [SerializeField] Texture2D pointerhIcon = null;
    // Start is called before the first frame update
    void Start()
    {
        int h = pointerhIcon.height;
        int w = pointerhIcon.width;
        Vector2 tmp = new Vector2(w * 0.2f, h * 0.2f);
        Cursor.SetCursor(pointerhIcon, tmp, CursorMode.ForceSoftware);
    }

}
