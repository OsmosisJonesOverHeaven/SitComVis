using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manipulator : MonoBehaviour {

    Camera cam;

    //runs once at start of program
    //sets variables
    void Start()
    {
        cam = GameObject.Find("Creator_Camera").GetComponent<Camera>();
        RightMenu();
    }

    //mangles camera to the left of the screen to allow a menu on the right
    public void RightMenu()
    {
        cam.rect = new Rect(0, 0, .5f, 1);
    }

    //mangles camera to the right of the screen to allow a menu on the left
    //unused but functional if ever needed
    public void LeftMenu()
    {
        cam.rect = new Rect(0.5f, 0, 1, 1);
    }

    //makes the camera take up the whole screen
    //use when the menu is hidden to avoid graphical bugs
    public void Revert()
    {
        cam.rect = new Rect(0, 0, 1, 1);
    }
}
