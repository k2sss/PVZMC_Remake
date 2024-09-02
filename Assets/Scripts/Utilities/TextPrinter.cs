using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
[RequireComponent(typeof(Text))]
public class TextPrinter : MonoBehaviour
{
    private Text text;
    private int SentenceIndex;
    [Multiline]
    public string[] Sentences;
    public string nowSentence { get; private set; }
    private float TimeDelay;
    public bool isPrint { get; private set; }
    private bool isPrint2;
    public float timeDelay = 0.2f;
    public Action[] TalkOverActions;
    public string soundType = "KeyBoard";
    public bool Auto = true;
    private Coroutine courtine;

    //规定格式
    private void Awake()
    {
        TalkOverActions = new Action[Sentences.Length];
        text = GetComponent<Text>();
    }
    private void Update()
    {
        if (Auto == true)
        {
            if (isPrint == false && SentenceIndex != 0)
            {
                StartPrintNextSentence();
            }
        }
    }
    public void TextIndexReset()
    {
        if (text == null)
            text = GetComponent<Text>();
        text.text = "";
        SentenceIndex = 0;
        if (courtine != null)
            StopCoroutine(courtine);
    }
    [ContextMenu("Start")]
    public void StartPrintNextSentence()//开始打印下一句
    {
        if (SentenceIndex < Sentences.Length)
        {
            nowSentence = Sentences[SentenceIndex];
            courtine = StartCoroutine(ShowText());
            SentenceIndex++;
        }

    }
    public bool isFinish()
    {
        if (SentenceIndex < Sentences.Length)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void FinishSpeak()
    {
        if (isPrint2 == true)
        {
            StopCoroutine(courtine);
            text.text = nowSentence;
            isPrint = false;
            isPrint2 = false;
            TalkOverActions[SentenceIndex]?.Invoke();
        }

    }
    IEnumerator ShowText()
    {
        isPrint2 = true;
        isPrint = true;
        text.text = "";
        int a = SentenceIndex;

        for (int i = 0; i < nowSentence.Length + 1; i++)
        {

            i += JudgeTime(nowSentence, i);//跳过特定句子
            yield return new WaitForSeconds(TimeDelay);
            text.text += nowSentence[i];
            SoundSystem.Instance.Play2Dsound(soundType);
            if (i == nowSentence.Length - 1)
            {
                TalkOverActions[a]?.Invoke();
                isPrint2 = false;
                MonoController.Instance.Invoke(1, () => isPrint = false);
            }

        }

    }
    private int JudgeTime(string s, int index)//如果不需要停顿
    {

        try
        {
            if (s.Substring(index, "/s".Length) == "/s")//如果截取的部分为/s
            {
                TimeDelay = timeDelay * 2f;
                return "/s".Length;
            }
            else if (s.Substring(index, "/ls".Length) == "/ls")//如果截取的部分为/ls
            {
                TimeDelay = timeDelay * 5f;
                return "/ls".Length;
            }
            else
            {
                TimeDelay = timeDelay;
            }
        }
        catch { }



        return 0;

    }

}
