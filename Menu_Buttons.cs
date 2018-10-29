using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Buttons : MonoBehaviour {

    GameObject shadows;
    private void Start()
    {
        shadows = transform.GetChild(0).gameObject;
    }

    float scaleSpeed = 10f;

    private void Update()
    {
        shadows.transform.position = new Vector3((-Input.mousePosition.x / scaleSpeed) + (Screen.width / 2), -Input.mousePosition.y / scaleSpeed + Screen.height/2, 0);
    }

    public void OpenEditor()
    {
        Cursor.visible = true;
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    public void PlayScene()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }

}
