using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XYDisplay : MonoBehaviour {

    public GameObject controller;

    GameObject currentX;
    GameObject currentY;
    GameObject currentZ;
    GameObject storedX;
    GameObject storedY;
    GameObject storedZ;

    private void Start()
    {
        currentX = this.transform.GetChild(2).GetChild(1).gameObject;
        currentY = this.transform.GetChild(3).GetChild(1).gameObject;
        currentZ = this.transform.GetChild(5).GetChild(1).gameObject;
        storedX = this.transform.GetChild(0).GetChild(1).gameObject;
        storedY = this.transform.GetChild(1).GetChild(1).gameObject;
        storedZ = this.transform.GetChild(4).GetChild(1).gameObject;
    }

    private void Update()
    {
        currentX.GetComponent<Text>().text = controller.GetComponent<Mouse_Pointer>().mouseX + "";
        currentY.GetComponent<Text>().text = controller.GetComponent<Mouse_Pointer>().mouseY + "";
        currentZ.GetComponent<Text>().text = controller.GetComponent<Mouse_Pointer>().mouseZ + "";
        storedX.GetComponent<Text>().text = controller.GetComponent<Mouse_Pointer>().storedX + "";
        storedY.GetComponent<Text>().text = controller.GetComponent<Mouse_Pointer>().storedY + "";
        storedZ.GetComponent<Text>().text = controller.GetComponent<Mouse_Pointer>().storedZ + "";
    }


}
