using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoWindow : BaseManager<InfoWindow>
{
    public Text titleText;
    public Text descriptionText;
    public GameObject tar;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > 3)
        {
            timer = 0;
            gameObject.GetComponent<Animator>().SetBool("Start", false);
            tar.SetActive(false);
        }
    }
    public void Show(string title,string descrption)
    {
        gameObject.GetComponent<Animator>().SetBool("Start", false);
        timer = 0;
        MonoController.Instance.Invoke(0.01f,()=>gameObject.GetComponent<Animator>().SetBool("Start", true));
        MonoController.Instance.Invoke(0.01f, () =>tar.SetActive(true));
        titleText.text = title;
        descriptionText.text = descrption;
    }
    public void Show(string title, string descrption,AudioClip sound)
    {
        gameObject.GetComponent<Animator>().SetBool("Start", false);
        timer = 0;
        MonoController.Instance.Invoke(0.01f, () => gameObject.GetComponent<Animator>().SetBool("Start", true));
        MonoController.Instance.Invoke(0.01f, () => tar.SetActive(true));
        titleText.text = title;
        descriptionText.text = descrption;
        SoundSystem.Instance.Play2Dsound(sound);
    }
}
