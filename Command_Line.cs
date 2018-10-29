using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Command_Line : MonoBehaviour {

    public GameObject history;
    public GameObject textBox;
    public GameObject controller;

    string[] textHistory = new string[5];
    string redoStorage;

    //string[] cmds = { "/save", "/load", "/color", "/move" };

	void Start () {
        if (textHistory[0] == null)
            textHistory[0] = "";
        redoStorage = "";
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

    void ShiftHistory(int dir)
    {
        if (dir > 0)
        {
            string[] tmp = { textHistory[1], textHistory[2], textHistory[3], textHistory[4], null };
            redoStorage = textHistory[0];
            textHistory = tmp;
        }
        else
        {
            if (redoStorage != "")
            {
                string[] tmp = {redoStorage, textHistory[1], textHistory[2], textHistory[3], textHistory[4] };
                textHistory = tmp;
            }
        }
    }

    public void AddtoScript(string line)
    {

    }

}
