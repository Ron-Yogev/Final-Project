using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A class designed to test color changes
 */
public class setColor : MonoBehaviour
{
    [SerializeField] 
    [Range(0f,256f)]
    float R = 255f;
    [SerializeField]
    [Range(0f, 256f)]
    float G = 255f;
    [SerializeField]
    [Range(0f, 256f)]
    float B = 255f;
    SpriteRenderer cubeRenderer;
    // Start is called before the first frame update
    void Start()
    {
        //Get the Renderer component from the new cube
        cubeRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {


        //Call SetColor using the shader property name "_Color" and setting the color to red
        cubeRenderer.material.SetColor("_Color", new Color(R/255f,G / 255f, B / 255f));
    }
}
