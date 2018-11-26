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
     * /animMove object speed (x y z/point) -
     * moves a character over speed time to a point or x y z
     * 
     * /delete string -
     * deletes the last row or every occurrance of a command
     * 
     * /replace -
     * replaces a line
     * 
     * /sub text -
     * displays a subtitle for x seconds
     * 
     * /speed x -
     * sets the delay between lines, default is 1 second
     * 
     * /view x -
     * switches camera to camera x
     * 
     * /addPos object x y z -
     * adds values to the position
     * 
     * /pause x -
     * pauses for x seconds
     * 
     * /load setting -
     * loads setting
     * 
     * /rename object name -
     * renames object to name
     */

    public GameObject history;
    public GameObject bigHistory;
    public GameObject textBox;
    public GameObject controller;
    public GameObject ObjectList;

    Animator m_Animator;
    float animationSpeed;

    public string fullText = "";
    string redoStorage = ""; //unused
    string setting;
    string lastCommand = "";

    //calls every frame
	void Update () {
        bigHistory.GetComponent<Text>().text = fullText;
        history.GetComponent<Text>().text = fullText;
    }
    //adds to the string fullText, containing the command history
    void AddHistory() {
        fullText += textBox.GetComponent<Text>().text + "\n";
    }

    //finds and replaces "target" in fullText with "replacement"
    void ReplaceHistory(string target, string replacement)
    {
        fullText = fullText.Replace(target, replacement);
    }

    //takes the command string from the text box and figures out what to do with it by splitting it into a string array
    //clears the text box and decides whether or not to add it to fullText
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
        } //-
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
        } //-
        else if (parsed[0] == "/move")
        {
            try
            {
                GameObject tmp = GameObject.Find("Pointer 1(Clone)");
                GameObject target = GameObject.Find(parsed[1]);
                float x, y, z;
                if (parsed[2].Contains("p"))
                    x = tmp.transform.position.x;
                else
                    x = float.Parse(parsed[2]);
                if (parsed[3].Contains("p"))
                    y = tmp.transform.position.y;
                else
                    y = float.Parse(parsed[3]);
                if (parsed[4].Contains("p"))
                    z = tmp.transform.position.z;
                else
                    z = float.Parse(parsed[4]);

                if (parsed[2] != "point")
                    target.transform.position = new Vector3(x, y, z);
                else
                {
                    target.transform.position = tmp.transform.position;
                    x = tmp.transform.position.x;
                    y = tmp.transform.position.y;
                    z = tmp.transform.position.z;
                }
                textBox.GetComponent<Text>().text = "/move " + parsed[1] + " " + x + " " + y + " " + z;
                fullText += "/move " + parsed[1] + " " + x + " " + y + " " + z + "\n";
            }
            catch
            {
                addError();
            }
        } //-
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
        } //-
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
        } //-
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
        } //-
        else if (parsed[0] == "/delete")
        {
            try
            {
                string tmp = "";
                if (parsed.Length > 1)
                {
                    for (int i = 1; i < parsed.Length; i++)
                    {
                        tmp += parsed[i].TrimEnd('\n') + " ";
                    }
                    tmp = tmp.TrimEnd(' ');
                }
                else
                {
                    Debug.Log("Found 2 " + lastCommand);
                    tmp = lastCommand;
                }
                ReplaceHistory(tmp, " ");
            }
            catch
            {
                addError();
            }
        } //-
        else if(parsed[0] == "/replace")
        {
            try
            {
                string[] tokens;
                string tmp = "";

                for (int i = 1; i < parsed.Length; i++)
                {
                    tmp += parsed[i].TrimEnd('\n') + " ";
                }
                tmp = tmp.TrimEnd(' ');
                tokens = tmp.Split('*');
                ReplaceHistory(tokens[0].TrimStart(' ').TrimEnd(' '), tokens[1].TrimStart(' ').TrimEnd(' '));
            }
            catch
            {
                addError();
            }
        } //-
        else if(parsed[0] == "/animove")
        {
            try
            {
                GameObject tmp = GameObject.Find("Pointer 1(Clone)");
                GameObject target = GameObject.Find(parsed[1]);
                float x, y, z;
                if (parsed[3].Contains("p"))
                    x = tmp.transform.position.x;
                else
                    x = float.Parse(parsed[3]);
                if (parsed[4].Contains("p"))
                    y = tmp.transform.position.y;
                else
                    y = float.Parse(parsed[4]);
                if (parsed[5].Contains("p"))
                    z = tmp.transform.position.z;
                else
                    z = float.Parse(parsed[5]);
                Debug.Log(x);
                if (parsed[3] != "point")
                    target.transform.position = new Vector3(x, y, z);
                else
                {
                    target.transform.position = tmp.transform.position;
                    x = tmp.transform.position.x;
                    y = tmp.transform.position.y;
                    z = tmp.transform.position.z;
                }
                textBox.GetComponent<Text>().text = "/animove " + parsed[1] + " " + parsed[2] + " " + x + " " + y + " " + z;
                fullText += "/animove " + parsed[1] + " " + parsed[2] + " " + x + " " + y + " " + z + "\n";
                //AddHistory();
            }
            catch
            {
                addError();
            }
        }
        else if (parsed[0] == "/rename")
        {
            try
            {
                GameObject obj = GameObject.Find(parsed[1]);
                string[] name = obj.name.Split('|');
                obj.name = name[0] + "|" + parsed[2].TrimEnd('\n');
                ObjectList.GetComponent<Objects_and_Commands>().UpdateList();
            }
            catch
            {
                addError();
            }
        } //-
        else if (parsed[0] == "/load")
        {
            try
            {
                controller.GetComponent<Data_Storage>().ReadSetting(parsed[1] + "(Setting).txt");
            }
            catch
            {
                Debug.Log("Not a Setting file");
            }
        } //-
        else if (parsed[0] == "/sub")
        {
            AddHistory();
        } //-
        else if (parsed[0] == "/speed" || parsed[0] == "/pause")
        {
            if(parsed.Length == 2 && int.Parse(parsed[1]) > 0)
                AddHistory();
        } //-
        else if (parsed[0] == "/view")
        {
            if (parsed.Length == 2)
                AddHistory();
        } //-
        else
        {
            Debug.Log("Not valid input");
        }

        lastCommand = textBox.GetComponent<Text>().text;
        this.GetComponent<InputField>().text = "";
    }

    //error handling for debugging purposes
    void addError()
    {
        Debug.Log("Bad Syntax");
    }
}
