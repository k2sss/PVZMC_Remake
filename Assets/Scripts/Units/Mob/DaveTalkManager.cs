using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaveTalkManager : BaseManager<DaveTalkManager>
{
    private bool Initialized;
    private GameObject TextWindow;
    private GameObject DavePicture;
    private GameObject DaveRecorder;
    private TextPrinter printer;
    private Animator DaveAnimator;
    private Animator DavePannelAnimator;
    public AudioClip[] CrazyDaveAudio_short;
    public AudioClip[] CrazyDaveAudio_long;
    public AudioClip[] CrazyDaveAudio_extremelyLong;
    private float ClickTimer;
    protected override void Awake()
    {
        base.Awake();
        Init();
        DavePicture.SetActive(false);
        TextWindow.SetActive(false);
    }


    private void Update()
    {
        ClickTimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0)&&ClickTimer > 0.2f)
        {
            if (!printer.isFinish())
            {
                if (printer.isPrint == true)
                {

                    printer.FinishSpeak();
                    ClickTimer = 0;
                }
                else
                {
                    printer.StartPrintNextSentence();
                    //²¥·ÅDave¶¯»­ºÍÓïÒô£º
                    DaveAnimationAndAudio(printer.nowSentence);

                }
                   

            }
            else
            {
                
                MonoController.Instance.Invoke(1, 
                    ()=> {
                        
                        ExitDaveTalk();
                    });
            }
        }
    }
    private void DaveAnimationAndAudio(string sentence)
    {
        if (DaveRecorder == null)
        {
            DaveRecorder = Instantiate(FileLoadSystem.ResourcesLoad<GameObject>("DaveRecorder"));
            DaveAnimator = DaveRecorder.transform.GetChild(0).GetComponent<Animator>();
        }
        int len = sentence.Length;
        if (len <= 5)
        {
            DaveAnimator.SetInteger("Talk", Random.Range(1, 3));
            SoundSystem.Instance.PlayRandom2Dsound(CrazyDaveAudio_short);
        }
        else if(len <15)
        {
            DaveAnimator.SetInteger("Talk", Random.Range(3, 5));
            SoundSystem.Instance.PlayRandom2Dsound(CrazyDaveAudio_long);
        }
        else
        {
            DaveAnimator.SetInteger("Talk", 5);
            SoundSystem.Instance.PlayRandom2Dsound(CrazyDaveAudio_extremelyLong);
        }



    }
    public void Init()
    {
        if (!Initialized)
        {
            Initialized = true;
            TextWindow = transform.Find("TalkWindow").gameObject;
            printer = TextWindow.transform.GetChild(0).GetComponent<TextPrinter>();
            DavePicture = transform.Find("Dave").gameObject;
            DavePannelAnimator = DavePicture.GetComponent<Animator>();
            DavePicture.SetActive(false);
        }
    }
    public void ShowTalkWindow(string[] texts)
    {
        Init();
        TextWindow.SetActive(true);
        printer.Sentences = texts;
        printer.TextIndexReset();
        printer.StartPrintNextSentence();
        DaveAnimationAndAudio(printer.nowSentence);
       
    }

    public void ShowDave()
    {
        Init();
        if (DaveRecorder == null)
        {
            DaveRecorder = Instantiate(FileLoadSystem.ResourcesLoad<GameObject>("DaveRecorder"));
            DaveAnimator = DaveRecorder.transform.GetChild(0).GetComponent<Animator>();
        }
        DavePannelAnimator.SetBool("Exit", false);
        DavePicture.SetActive(true);
    }
    public void DaveExit()
    {
        DavePannelAnimator.SetBool("Exit", true);
        MonoController.Instance.Invoke(0.7f, () => DavePicture.SetActive(false));
    }

    public void CloseTalkWindow()
    {
        TextWindow.SetActive(false);
    }

    public void ExitDaveTalk()
    {
        DaveExit();
        CloseTalkWindow();
    }
}
