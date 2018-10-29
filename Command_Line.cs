using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Command_Line : MonoBehaviour {

    public GameObject history;
    public GameObject textBox;
    public GameObject controller;

    //string[] cmds = { "/save", "/load", "/color", "/move" };

	void Start () {
		
	}
	

	void Update () {
		
	}

    public void ParseCommand()
    {
        string cmd = textBox.GetComponent<Text>().text;
        string[] parsed = cmd.Split(' ');
        int index = 0;
        foreach(string s in parsed)
        {
            if (s == "/save")
            {
                if (parsed[index + 1].StartsWith("'") && parsed[index + 1].EndsWith("'"))
                    controller.GetComponent<Data_Storage>().WriteScript(parsed[index + 1].Substring(1, parsed[index + 1].Length - 2));
                else if(parsed[index + 1].StartsWith("/"))
                {
                    //controller.GetComponent<Data_Storage>().WriteScript()
                    //Save setting
                    //Save other stuff
                }

            }
        }
    }

}
