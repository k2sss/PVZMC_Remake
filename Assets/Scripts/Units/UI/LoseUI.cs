using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoseUI : Pannel
{
    public bool IsLose;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        EventMgr.Instance.AddEventListener("GameOver", GameLose);
        gameObject.SetActive(false);
    }
    public void GameLose()
    {
        gameObject.SetActive(true);
        InputMgr.Instance.UnableAllInput();
        MonoController.Instance.Invoke(1, () => IsLose = true);
        MusicMgr.Instance.PlayMusic("Lose",0,true);
        MusicMgr.Instance.StopMusicLoop();
    }
    // Update is called once per frame
    void Update()
    {
        if(IsLose == true)
        {
            Time.timeScale = 0.1f;
        }    
    }
    public void ReLoad()
    {
        SceneMgr.Instance.LoadAsync(SceneManager.GetActiveScene().name);

    }

}
