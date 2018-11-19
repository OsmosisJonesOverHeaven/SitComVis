using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

public class PlayScript : MonoBehaviour {

    public float speed = 1;
    public float timeElapsed = 0;
    public bool play = false;
    int index = 0;
    float fullTime = 0;

    string[] script;
    //IEnumerator coroutine;

    public GameObject subtitle;

	void Start () {
        //coroutine = ParseCommand();
	}
	
	
	void Update () {
        if (play)
        {
            timeElapsed += Time.deltaTime;
            fullTime += Time.deltaTime;
        }
	}

    public void PauseNPlay()
    {
        play = true;
        if (play)
            InvokeRepeating("ParseCommand", 0, speed);
    }

    public void ReadScript()
    {
        string path = EditorUtility.OpenFilePanel("Choose a Script File", "", "txt");
        string tmp = File.ReadAllText(path);
        string[] tempScript = tmp.Split('/');
        script = new string[tempScript.Length - 1];
        for(int i = 1; i < tempScript.Length; i++)
        {
            script[i - 1] = tempScript[i];
        }
    }
    
    void ParseCommand()
    {
        //parse the commands
        string[] parsed = script[index].Split(' ');  //ISSUE IS HERE
        if (parsed[0] == "speed")
        {
            try
            {
                speed = float.Parse(parsed[1]);
                CancelInvoke("ParseCommand");
                InvokeRepeating("ParseCommand", speed, speed);
            }
            catch
            {
                ErrorDisp("Error thrown at: (" + script[index] + ").");
            }
        } //-
        else if (parsed[0] == "pause")
        {
            try
            {
                CancelInvoke("ParseCommand");
                InvokeRepeating("ParseCommand", float.Parse(parsed[1]), speed);
            }
            catch
            {
                ErrorDisp("Error thrown at: (" + script[index] + ").");
            }
        } //-
        else if (parsed[0] == "color")
        {
            try
            {
                GameObject tmp = GameObject.Find(parsed[1]);
                if (tmp.GetComponent<Renderer>())
                {
                    Material[] mats = tmp.GetComponent<Renderer>().materials;
                    foreach (Material m in mats)
                    {
                        m.color = new Color(float.Parse(parsed[2]), float.Parse(parsed[3]), float.Parse(parsed[4]));
                    }
                }
                else
                {
                    foreach(Transform child in tmp.transform)
                    {
                        if (child.GetComponent<Renderer>())
                        {
                            child.GetComponent<Renderer>().material.color = new Color(float.Parse(parsed[2]), float.Parse(parsed[3]), float.Parse(parsed[4]));
                        }
                    }
                }
            }
            catch
            {
                ErrorDisp("Error thrown at: (" + script[index] + ").");
            }
        } //-
        else if (parsed[0] == "move")
        {
            try
            {
                GameObject target = GameObject.Find(parsed[1]);
                float x, y, z;
                x = float.Parse(parsed[2]);
                y = float.Parse(parsed[3]);
                z = float.Parse(parsed[4]);

                target.transform.position = new Vector3(x, y, z);
            }
            catch
            {
                ErrorDisp("Error thrown at: (" + script[index] + ").");
            }
        } //-
        else if (parsed[0] == "rotate")
        {
            try
            {
                GameObject tmp = GameObject.Find(parsed[1]);
                tmp.transform.rotation = Quaternion.Euler(float.Parse(parsed[2]), float.Parse(parsed[3]), float.Parse(parsed[4]));
            }
            catch
            {
                ErrorDisp("Error thrown at: (" + script[index] + ").");
            }
        } //-
        else if (parsed[0] == "scale")
        {
            try
            {
                GameObject tmp = GameObject.Find(parsed[1]);
                tmp.transform.localScale = new Vector3(float.Parse(parsed[2]), float.Parse(parsed[3]), float.Parse(parsed[4]));
            }
            catch
            {
                ErrorDisp("Error thrown at: (" + script[index] + ").");
            }
        } //-
        else if (parsed[0] == "sub")
        {
            try
            {
                string tmp = "";
                if (parsed[1] != "CLEAR")
                {
                    for (int i = 1; i < parsed.Length; i++)
                    {
                        tmp += parsed[i] + " ";
                    }
                }
                subtitle.GetComponent<Text>().text = tmp;
            }
            catch
            {
                ErrorDisp("Error thrown at: (" + script[index] + ").");
            }
        } //-
        else
        {
            Debug.Log(parsed[0] + "not found");
        }
        if (index + 1 <= script.Length - 1)
            index++;
        else
        {
            play = false;
            CancelInvoke("ParseCommand");
        }
    }
    

    void ErrorDisp(string error)
    {
        subtitle.GetComponent<Text>().color = Color.red;
        subtitle.GetComponent<Text>().text = "ERROR! " + error + " PLEASE CHECK YOUR SCRIPT AND RELOAD SCENE!";
    }


    /*Conventions:
     * /color object red blue green -
     * changes the color of an object
     * 
     * /move object (x y z) -
     * moves an object to xyz or to the marked point
     * 
     * /scale object (x y z) -
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
     * /sub text -
     * displays a subtitle for x seconds
     * 
     * /view x
     * switches view to camera x
     * 
     * /speed x -
     * sets the delay between lines, default is 1 second
     * 
     * /pause x -
     * pauses the scene
     */
}
