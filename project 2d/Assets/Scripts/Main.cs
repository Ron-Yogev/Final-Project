using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main instance;
    public Web web;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        web = GetComponent<Web>();
    }

}
