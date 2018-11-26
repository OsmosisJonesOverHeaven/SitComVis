using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objects_and_Commands : MonoBehaviour {

    public bool Objects; //if false, defaults to command block	
    GameObject room_holder;
    Camera_Switcher cs;

    //called at start
    //gets the spawned object holder
    private void Start()
    {
        room_holder = GameObject.Find("Room_Holder");
        cs = this.gameObject.GetComponent<Camera_Switcher>();
    }

    public int numObjs = 0;
    public int id = 0;

    //called every frame
    //updates the object list if the number of items in the room doesnt match numObjs
    private void Update()
    {
        if (Objects)
        {
            if(numObjs != room_holder.transform.childCount)
            {
                UpdateList();
                numObjs = room_holder.transform.childCount;
            }
        }
    }

    //actually updates the list
    public void UpdateList()
    {
        string tmp = "";
        foreach(Transform child in room_holder.transform)
        {
            id += 1;
            if (!child.name.Contains("|")) {
                //Debug.Log("AddID?");
                child.name = child.name.Insert(child.name.Length, "|" + id);
                child.name = child.name.Replace("(Clone)", "");
            }
            if (child.name.Contains("Camera"))
            {
                //Debug.Log("Eureka!");
                cs.SaveCamPos(child);
            }
            //Debug.Log("ID: " + id + " Child Name: " + child.name);
            //Debug.Log("Spawned: " + child.name); 
            tmp += child.name + ", "; //when they get stored to lists, have a number associated and add that number
            //Debug.Log(tmp);
        }
        tmp = tmp.Substring(0, tmp.Length - 2);
        transform.GetChild(0).GetComponent<Text>().text = tmp;
    }
}
