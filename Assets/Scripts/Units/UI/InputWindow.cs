using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InputWindow : BaseManager<InputWindow>
{
    public InputField Inputfield;
    public GameObject target;
    public Button YesButton;
    public Text contentText;
    // Start is called before the first frame update
    void Start()
    {
        target.SetActive(false);
    }

    public void Show(string text,UnityAction action)
    {
        Inputfield.text = "";
        contentText.text = text;
        target.SetActive(true);
        YesButton.onClick.RemoveAllListeners();
        YesButton.onClick.AddListener(action);
        YesButton.onClick.AddListener(Close);
    }
    public void Close()
    {
        target.SetActive(false);
        SoundSystem.Instance.Play2Dsound("Click");
    }
    public string GetText()
    {
        return Inputfield.text;
    }
}
