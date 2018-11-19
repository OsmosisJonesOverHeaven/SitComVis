using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Command_Line : MonoBehaviour {

    /*Conventions:
     * /save (setting/script) -
     * saves the scene with an optional name
     * 
     * /color object red blue green -
     * changes the color of an object
     * 
     * /move object (x y z/point) -
     * moves an object to xyz or to the marked point
     * 
     * /scale object (x y z/factor) -
     * changes size of an object
     * 
     * /rotate object (x y z) -
     * changes rotation of an object
     * 
     * /anim object name (speed/on/off)
     * animates a character
     * 
     * /animMove object (x y z/point)
     * moves a character over time to a point
     * 
     * /delete string -
     * deletes the last row or every occurrance of a command
     * 
     * /replace
     * replaces a line
     * 
     * /sub x text -
     * displays a subtitle for x seconds
     * 
     * /speed x -
     * sets the delay between lines, default is 1 second
     * 
     * /view x -
     * switches camera to camera x
     * 
     * /addPos object x y z
     * adds values to the position
     * 
     * /pause x
     * pauses for x seconds
     */

    public GameObject history;
    public GameObject bigHistory;
    public GameObject textBox;
    public GameObject controller;

    string fullText = "";
    string redoStorage = "";
    string setting;
    string lastCommand = "";

	void Start () {
        
	}
	

	void Update () {
        bigHistory.GetComponent<Text>().text = fullText;
        history.GetComponent<Text>().text = fullText;
    }

    void AddHistory() {
        fullText += textBox.GetComponent<Text>().text + "\n";
    }
    void ReplaceHistory(string target, string replacement)
    {
        fullText.Replace(target, replacement);
    }

    public void ParseCommand()
    {
        string cmd = this.GetComponent<InputField>().text;
        string[] parsed = cmd.Split(' ');
        if (parsed[0] == "/save")
        {
            try
            {
                if(parsed[1] == "setting")
                    controller.GetComponent<Data_Storage>().SaveSetting();
                else
                    controller.GetComponent<Data_Storage>().SaveActions(fullText);
            }
            catch
            {
                addError();
            }
        }
        else if (parsed[0] == "/color")
        {
            try
            {
                GameObject target = GameObject.Find(parsed[1]);
                if (target.GetComponent<Renderer>())
                {
                    Material[] materials = target.GetComponent<Renderer>().materials;
                    foreach (Material m in materials)
                    {
                        m.color = new Color(float.Parse(parsed[2]), float.Parse(parsed[3]), float.Parse(parsed[4]));
                    }
                }
                else
                {
                    foreach (Transform c in target.transform)
                    {
                        if(c.GetComponent<Renderer>())
                            c.GetComponent<Renderer>().material.color = new Color(float.Parse(parsed[2]), float.Parse(parsed[3]), float.Parse(parsed[4]));
                    }
                }
                AddHistory();
            }
            catch
            {
                addError();
            }
        }
        else if (parsed[0] == "/move")
        {
            try
            {
                GameObject tmp = GameObject.Find("Pointer 1(Clone)");
                GameObject target = GameObject.Find(parsed[1]);
                float x, y, z;
                if (parsed[2] == "p")
                    x = tmp.transform.position.x;
                else
                    x = float.Parse(parsed[2]);
                if (parsed[3] == "p")
                    y = tmp.transform.position.y;
                else
                    y = float.Parse(parsed[3]);
                if (parsed[4] == "p")
                    z = tmp.transform.position.z;
                else
                    z = float.Parse(parsed[4]);

                if (parsed[2] != "point")
                    target.transform.position = new Vector3(x, y, z);
                else
                    target.transform.position = tmp.transform.position;
                textBox.GetComponent<Text>().text = "/move " + parsed[1] + " " + x + " " + y + " " + z;
                AddHistory();
            }
            catch
            {
                addError();
            }
        }
        else if (parsed[0] == "/addPos")
        {
            try
            {
                GameObject target = GameObject.Find(parsed[1]);
                target.transform.Translate(float.Parse(parsed[2]), float.Parse(parsed[3]), float.Parse(parsed[4]));
                AddHistory();
            }
            catch
            {
                addError();
            }
        }
        else if (parsed[0] == "/scale")
        {
            try
            {
                GameObject target = GameObject.Find(parsed[1]);
                target.transform.localScale = new Vector3(float.Parse(parsed[2]), float.Parse(parsed[3]), float.Parse(parsed[4]));
                AddHistory();
            }
            catch
            {
                addError();
            }
        }
        else if (parsed[0] == "/rotate")
        {
            try
            {
                GameObject target = GameObject.Find(parsed[1]);
                target.transform.rotation = Quaternion.Euler(target.transform.rotation.x + float.Parse(parsed[2]), target.transform.rotation.y + float.Parse(parsed[3]), target.transform.rotation.z + float.Parse(parsed[4]));
                AddHistory();
            }
            catch
            {
                addError();
            }
        }
        else if (parsed[0] == "/delete")
        {
            try
            {
                string tmp = "";
                if (parsed.Length > 1)
                {
                    
                    for(int i = 1; i < parsed.Length; i++)
                    {
                        tmp += parsed[i] + " ";
                    }
                    tmp.TrimEnd(' ');
                }
                else
                {
                    tmp = lastCommand;
                }
                ReplaceHistory(tmp, "");
            }
            catch
            {
                addError();
            }
        }
        else if(parsed[0] == "/replace")
        {/*
            try
            {
                string tmp = "";
                for (int i = 1; i < parsed.Length; i++)
                {
                    tmp += parsed[i] + " ";
                }
                tmp.TrimEnd(' ');

            }
            catch
            {
                addError();
            }*/
        }
        else if(parsed[0] == "/animove")
        {

        }
        else if(parsed[0] == "/anim")
        {

        }
        else if (parsed[0] == "/sub")
        {
            AddHistory();
        }
        else if (parsed[0] == "/speed")
        {
            if(parsed.Length == 2 && int.Parse(parsed[1]) > 0)
                AddHistory();
        }
        else if (parsed[0] == "/view")
        {
            if (parsed.Length == 2)
                AddHistory();
        }
        else
        {
            Debug.Log("Not valid input");
        }


        this.GetComponent<InputField>().text = "";
    }

    void addError()
    {
        Debug.Log("Bad Syntax");
    }


    //all anims and motion is stored in the text
    //key removes a line
    //adds a line to stack
    //UpdateActions()
    //move works by lerp over time
}
