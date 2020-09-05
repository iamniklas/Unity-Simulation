using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] GameObject mPausePanel = null;
    [SerializeField] GameObject mFinishPanel = null;

    [SerializeField] KeyCode mPauseKey = KeyCode.Escape;

    bool mPaused = false;

    int mMainMenuIndex = 0;

    bool mGameIsOver = false;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(mPauseKey))
        {
            TogglePause();
        }

        Cursor.visible = mPaused;
        Cursor.lockState = mPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void TogglePause()
    {
        if (!mGameIsOver)
        {
            mPaused = !mPaused;

            mPausePanel.SetActive(!mPausePanel.activeSelf);

            if(mPaused)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            Time.timeScale = mPaused ? 0.0f : 1.0f;
        }
    }

    public void ShowFinalPanel()
    {
        if(!mGameIsOver)
        {
            mGameIsOver = true;
            mPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0.0f;
            mFinishPanel.SetActive(true);
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(mMainMenuIndex);
    }
}
