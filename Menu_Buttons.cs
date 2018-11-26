using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Buttons : MonoBehaviour {

    GameObject shadows;

    //runs at first frame
    //finds the object for the shadows on the main menu
    private void Start()
    {
        shadows = transform.GetChild(0).gameObject;
    }

    float scaleSpeed = 10f;

    //runs every frame
    //moves the shadow around on the main menu
    private void Update()
    {
        shadows.transform.position = new Vector3((-Input.mousePosition.x / scaleSpeed) + (Screen.width / 2), -Input.mousePosition.y / scaleSpeed + Screen.height/2, 0);
    }

    //opens the SceneEditor
    public void OpenEditor()
    {
        Cursor.visible = true;
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    //opens the Main Menu
    public void OpenMenu()
    {
        Cursor.visible = false;
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

    //opens the ScenePlayer
    public void PlayScene()
    {
        Cursor.visible = true;
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
    }

    //closes the application
    public void Exit()
    {
        Application.Quit();
    }

}
