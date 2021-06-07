using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManagerScript : MonoBehaviour
{
    public bool inMenu, isTurorial;
    public static AudioClip paint, combine, erase, click, win, lose, crowd_panik, countdown, bk0, bk1, bk2, bk3, bk4, bk5, bk6, menu_bk;
    static AudioSource audioSrc;

    static float music_v = 0.1f;
    static float SFX_v = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        paint = Resources.Load<AudioClip>("paint");
        combine = Resources.Load<AudioClip>("combine");
        erase = Resources.Load<AudioClip>("erase");
        click = Resources.Load<AudioClip>("click");
        win = Resources.Load<AudioClip>("win");
        lose = Resources.Load<AudioClip>("lose");
        crowd_panik = Resources.Load<AudioClip>("crowd_panik");
        countdown = Resources.Load<AudioClip>("clockTicking");
        bk0 = Resources.Load<AudioClip>("background0");
        bk1 = Resources.Load<AudioClip>("background1");
        bk2 = Resources.Load<AudioClip>("background2");
        bk3 = Resources.Load<AudioClip>("background3");
        bk4 = Resources.Load<AudioClip>("background4");
        bk5 = Resources.Load<AudioClip>("background5");
        bk6 = Resources.Load<AudioClip>("background6");
        menu_bk = Resources.Load<AudioClip>("MenuBackGround");


        audioSrc = GetComponent<AudioSource>();

        PlayBackground();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBackground()
    {
        if (!inMenu && !isTurorial)
        {
            int rand = 0;
            rand = Random.Range(1, 7);
            PlaySound("background" + rand);
        }
        else if(!isTurorial)
        {
            PlaySound("MenuBackGround");
        }
        else
        {
            PlaySound("background0");
        }

    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "paint":
                audioSrc.PlayOneShot(paint,SFX_v);
                break;
            case "combine":
                audioSrc.PlayOneShot(combine, SFX_v);
                break;
            case "erase":
                audioSrc.PlayOneShot(erase, SFX_v);
                break;
            case "click":
                audioSrc.PlayOneShot(click, SFX_v);
                break;
            case "win":
                audioSrc.PlayOneShot(win, SFX_v) ;
                break;
            case "lose":
                audioSrc.PlayOneShot(lose, SFX_v);
                break;
            case "crowd_panik":
                audioSrc.PlayOneShot(crowd_panik, SFX_v);
                break;
            case "clockTicking":
                audioSrc.PlayOneShot(countdown, SFX_v);
                break;
            case "background0":
                audioSrc.PlayOneShot(bk0, music_v);
                break;
            case "background1":
                     audioSrc.PlayOneShot(bk1, music_v);
                     break;
            case "background2":
                audioSrc.PlayOneShot(bk2, music_v);
                break;
            case "background3":
                audioSrc.PlayOneShot(bk3, music_v);
                break;
            case "background4":
                audioSrc.PlayOneShot(bk4, music_v);
                break;
            case "background5":
                audioSrc.PlayOneShot(bk5, music_v);
                break;
            case "final_level_background":
                audioSrc.PlayOneShot(bk6, music_v);
                break;
            case "MenuBackGround":
                audioSrc.PlayOneShot(menu_bk, music_v);
                break;
        }
    }

    public void setSFX(float v)
    {
        SFX_v = v;
    }

    public void setMusic(float v)
    {
        music_v = v;
    }
}
