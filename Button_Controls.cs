using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Controls : MonoBehaviour {

    public float speed = 100;
    public Obj_Lister ol;
    float scroll = 0;

    //calls every frame
    //moves the menu items up and down under certain circumstances
    void Update () {
        scroll = Input.GetAxis("Mouse ScrollWheel") + Input.GetAxis("Vertical");
        if (ol.canScroll && (ol.canScrollUp && scroll > 0) || (ol.canScrollDown && scroll < 0))
            transform.Translate(0, (scroll) * speed, 0);
	}
}
