using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manipulator : MonoBehaviour {

    Camera cam;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        RightMenu();
    }

    public void RightMenu()
    {
        cam.rect = new Rect(-0.5f, 0, 1, 1);
    }
    public void LeftMenu()
    {
        cam.rect = new Rect(0.5f, 0, 1, 1);
    }
    public void Revert()
    {
        cam.rect = new Rect(0, 0, 1, 1);
    }
}
