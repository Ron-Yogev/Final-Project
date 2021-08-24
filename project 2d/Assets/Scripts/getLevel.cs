using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getLevel : MonoBehaviour
{

    public TMPro.TextMeshProUGUI back_level;
    public TMPro.TextMeshProUGUI front_level;

    // Start is called before the first frame update
    void Start()
    {
        if (calculateScore.isDemo)
        {
            back_level.text = "DEMO";
            front_level.text = "DEMO";
        }
        else if (RouteLevel.isCustom)
        {
            back_level.text = Main.instance.web.getCurrentUser() +" LEVEL";
            front_level.text = Main.instance.web.getCurrentUser() + " LEVEL";
        }
        else
        {
            back_level.text = "LEVEL " + Web.level;
            front_level.text = "LEVEL " + Web.level;
        }
        
    }
}
