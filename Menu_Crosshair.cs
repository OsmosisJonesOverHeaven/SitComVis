using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Crosshair : MonoBehaviour {

    GameObject y;
    GameObject x;

    //runs at the start
    //gets crosshair objects
	void Start () {
        Cursor.visible = false;
        y = transform.GetChild(0).gameObject;
        x = transform.GetChild(1).gameObject;
    }

    //moves crosshair objects every frame
	void Update () {
        y.transform.position = new Vector3(Input.mousePosition.x + 1, y.transform.position.y, y.transform.position.z);
        x.transform.position = new Vector3(x.transform.position.x, Input.mousePosition.y + 1, x.transform.position.z);
    }
}
