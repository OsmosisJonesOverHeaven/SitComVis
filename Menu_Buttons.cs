using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Buttons : MonoBehaviour {

    public void OpenEditor()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    public void PlayScene()
    {

    }

    public void OpenSettings()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }

}
