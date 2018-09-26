using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Controls : MonoBehaviour {

    public float speed = 250;
    public Obj_Lister ol;

    float scroll = 0;
    void Update () {
        scroll = Input.GetAxis("Mouse ScrollWheel") + Input.GetAxis("Vertical");
        if ((ol.canScrollUp && scroll > 0) || (ol.canScrollDown && scroll < 0))
            transform.Translate(0, (scroll) * speed, 0);
	}
}
