using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manipulator : MonoBehaviour {

    Camera cam;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public void LeftMenu()
    {
        cam.rect.Set(.5f, 0f, 1, 1);
    }
    public void RightMenu()
    {
        cam.rect.Set(0f, 0f, .5f, 1);
    }
}
