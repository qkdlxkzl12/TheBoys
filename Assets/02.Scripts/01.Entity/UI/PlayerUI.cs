using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 0414

public class PlayerUI : MonoBehaviour
{

    //public GameObject PlayerStatue;
    public GameObject GamePause;
    public GameObject GameLose;
    public GameObject GameClear;

    bool TurnPause = false;
    bool TurnStop = false;

    void Awake()
    {
        GamePause.SetActive(false);
        GameLose.SetActive(false);
        GameClear.SetActive(false);
    }

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if (TurnPause == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
        }
    }

    public void DeadEvent()
    {
        GameObject.Find("PauseUI").transform.parent = null;
        GameLose.SetActive(true);
        TurnStop = true;
        Time.timeScale = 0;
        Cursor.visible = true;
    }

    public void ClearEvent()
    {
        TurnStop = true;
        Cursor.visible = true;
        GameClear.SetActive(true);
    }

    public void Pause()
    {
        if(TurnPause == false)
        {
            Cursor.visible = true;
            TurnPause = true;
            GamePause.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Cursor.visible = false;
            TurnPause = false;
            GamePause.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void GoToMain()
    {
        SceneManager.LoadScene("00.MainMenu");
    }

   public void Exit()
    {
        Application.Quit();
    }
    
}
