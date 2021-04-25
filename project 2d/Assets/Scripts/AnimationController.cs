using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{

    private bool anim1 = true;
    private bool anim2 = false;
    private bool anim3 = false;
    private bool anim4 = false;
    private bool anim5 = false;
    private bool anim6 = false;
    private bool anim7 = false;
    private bool anim8 = false;
    private bool anim9 = false;
    private bool anim10 = false;

    float cyan = 0;
    float magenta = 0;
    float yellow = 0;
    [SerializeField] List<GameObject> animations;
    [SerializeField] Button erase_button;
    [SerializeField] Button combine_button;
    [SerializeField] Button paint_button;
    [SerializeField] Button finish_button;
    [SerializeField] Button home_button;

    // Start is called before the first frame update
    void Start()
    {
        erase_button.interactable = false;
        combine_button.interactable = false;
        paint_button.interactable = false;
        finish_button.interactable = false;
        home_button.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //get the position of the click
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.transform.tag == "color" && anim1)
                {
                    anim1 = false;
                    animations[0].SetActive(false);
                    anim2 = true;
                    animations[1].SetActive(true);
                }

                if (hit.collider != null && hit.transform.tag == "paintable" && anim2)
                {
                    anim2 = false;
                    animations[1].SetActive(false);
                    anim3 = true;
                    erase_button.interactable = true;
                    animations[2].SetActive(true);
                }


                if (hit.collider != null && hit.transform.tag == "paintable" && anim4)
                {
                    combine_button.interactable = true;
                    anim4 = false;
                    animations[3].SetActive(false);
                    anim5 = true;
                    animations[4].SetActive(true);
                }

                if (hit.collider != null && hit.transform.tag == "uncolor" && anim7)
                {
                    paint_button.interactable = true;
                    anim7 = false;
                    animations[6].SetActive(false);
                    anim8 = true;
                    animations[7].SetActive(true);

                }

                if (hit.collider != null && hit.transform.tag == "paintable" && anim9)
                {
                    finish_button.interactable = true;
                    anim9 = false;
                    animations[8].SetActive(false);
                    anim10 = true;
                    animations[9].SetActive(true);
                }
            }
        }
    }

    public void erase3Arrow()
    {
        if (anim3)
        {
            anim3 = false;
            erase_button.interactable = false;
            animations[2].SetActive(false);
            anim4 = true;
            animations[3].SetActive(true);
        }
    }

    public void combine5Arrow()
    {
        if (anim5)
        {
            combine_button.interactable = false;
            anim5 = false;
            animations[4].SetActive(false);
            anim6 = true;
            animations[5].SetActive(true);
        }
    }

    public void setCyanColor(float add)
    {
        cyan = add;
        checkSliderValue();
    }

    public void setMagentaColor(float add)
    {
        magenta = add;
        checkSliderValue();
    }

    public void setYellowColor(float add)
    {
        yellow = add;
        checkSliderValue();
    }

    public void checkSliderValue()
    {
        if (anim6 && cyan==0f && yellow==1f && magenta==1f)
        {
            anim6 = false;
            animations[5].SetActive(false);
            anim7 = true;
            animations[6].SetActive(true);
        }
    }

    public void brush8Arrow()
    {
        if (anim8)
        {
            paint_button.interactable = false;
            anim8 = false;
            animations[7].SetActive(false);
            anim9 = true;
            animations[8].SetActive(true);
        }
    }

    public void finishButton()
    {
        if (anim10)
        {
            animations[9].SetActive(false);
            finish_button.interactable = false;
            home_button.interactable = false;
            Cursor.visible = true;
        }
    }

    public void backToManu()
    {
        SceneManager.LoadScene("Menu");
    }
}
