using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objects_and_Commands : MonoBehaviour {

    public bool Objects; //if false, defaults to command block	
    GameObject room_holder;
    GameObject controller;

    private void Start()
    {
        room_holder = GameObject.Find("Room_Holder");
        controller = GameObject.Find("Controller");
    }

    //obj
    int numObjs = 0;
    public int id = 0;
    //cmd

    private void Update()
    {
        //obj
        if (Objects)
        {
            if(numObjs != room_holder.transform.childCount)
            {
                UpdateList();
                numObjs = room_holder.transform.childCount;
            }
        }
    }

    //Object list stuff
    public void UpdateList()
    {
        string tmp = "";
        id = 0;
        foreach(Transform child in room_holder.transform)
        {
            id += 1;
            child.name = child.name.Replace("(Clone)", "_" + id);
            tmp += child.name.Replace("(Clone)", "_" + id) + ", "; //when they get stored to lists, have a number associated and add that number
        }
        tmp = tmp.Substring(0, tmp.Length - 2);
        transform.GetChild(0).GetComponent<Text>().text = tmp;
    }

    //Command line stuff

}
