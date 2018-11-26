using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class PlayScript : MonoBehaviour {

    public float speed = 1;
    public float timeElapsed = 0;
    public bool play = false;
    bool coroutineRunning = false;
    bool ended = false;
    int index = 0;

    string[] script;
    string scriptPath;

    public GameObject input;
    public GameObject timeElapsedDisplay;
    public GameObject subtitle;
    public GameObject camera;
    Vector3 cameraPosition;
    Vector3 cameraRotation;

    //called at start
    //gets camera position and rotation
	void Start () {
        cameraPosition = camera.transform.position;
        cameraRotation = camera.transform.rotation.eulerAngles;
	}
	
	//decides whether to update the elapsed time every frame
	void Update () {
        if (play || coroutineRunning)
            timeElapsed += Time.deltaTime;
        UpdateTime(timeElapsed);
	}

    //actually updates the elapsed time and changes its color
    void UpdateTime(float t)
    {
        if (play || (ended && coroutineRunning))
            timeElapsedDisplay.GetComponent<Text>().color = Color.gray;
        else if((ended || !play) && !coroutineRunning)
            timeElapsedDisplay.GetComponent<Text>().color = Color.white;
        timeElapsedDisplay.GetComponent<Text>().text = timeElapsed + "s";
    }

    //starts the playthrough of the script
    public void RunScript()
    {
        if (scriptPath != null && scriptPath != "")
        {
            if (ended)
                this.GetComponent<Data_Storage>().ReadSetting(this.GetComponent<Data_Storage>().path);
            Time.timeScale = 1;
            coroutineRunning = false;
            ended = false;
            play = true;
            timeElapsed = 0;
            index = 0;
            if (play)
                InvokeRepeating("ParseCommand", 0, speed);
        }
    }

    //pauses and continues the playthrough
    public void PauseNPlay()
    {
	    if(scriptPath != "" && scriptPath != null){
        	if (play)
         	   Time.timeScale = 0;
        	else
        	    Time.timeScale = 1;
        	play = !play;
	    }
        //Debug.Log(play);
    }

    //opens an Explorer window to load a script file
    public void ReadScript()
    {
        try {
            //scriptPath = EditorUtility.OpenFilePanel("Choose a Script File", "", "txt");
            scriptPath = input.GetComponent<InputField>().text + "(Script).txt";
            if (scriptPath != ""){
        	    string tmp = File.ReadAllText(scriptPath);
        	    string[] tempScript = tmp.Split('/');
        	    script = new string[tempScript.Length - 1];
        	    for(int i = 1; i < tempScript.Length; i++)
        	    {
            	    script[i - 1] = tempScript[i];
        	    }
	        }
        }
        catch
        {
            input.GetComponent<InputField>().text = "";
        }
    }

    //parses the next line of the script by splitting the command into parsed[]
    //programmer's note: "//-" is to keep track of functioning commands
    void ParseCommand()
    {
        string[] parsed = script[index].Split(' ');
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
                    foreach (Transform child in tmp.transform)
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
        else if (parsed[0] == "addPos")
        {
            try
            {
                GameObject target = GameObject.Find(parsed[1]);
                target.transform.Translate(float.Parse(parsed[2]), float.Parse(parsed[3]), float.Parse(parsed[4]));
            }
            catch
            {
                ErrorDisp("Error thrown at: (" + script[index] + ").");
            }
        } //-
        else if (parsed[0] == "view")
        {
            try
            {
                if (parsed[1].TrimEnd('\n') == "-1")
                {
                    camera.transform.position = cameraPosition;
                    camera.transform.rotation = Quaternion.Euler(cameraRotation);
                    camera.GetComponent<Camera>().orthographic = true;
                }
                else
                {
                    GameObject tmp = GameObject.Find("Camera|" + parsed[1].TrimEnd('\n'));
                    camera.transform.position = tmp.transform.position;
                    camera.transform.rotation = tmp.transform.rotation;
                    camera.transform.rotation = Quaternion.Euler(camera.transform.rotation.eulerAngles.x + 90, camera.transform.rotation.eulerAngles.z, 0);
                    camera.GetComponent<Camera>().orthographic = false;
                }
            }
            catch
            {
                ErrorDisp("Error thrown at: (" + script[index] + ").");
            }
        } //-
        else if (parsed[0] == "animove")
        {
            try
            {
                GameObject obj = GameObject.Find(parsed[1]);
                Vector3 dest = new Vector3(float.Parse(parsed[3]), float.Parse(parsed[4]), float.Parse(parsed[5]));
                float time = float.Parse(parsed[2]);
                StartCoroutine(MoveOverSeconds(obj, dest, time));
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
            ended = true;
            if (!coroutineRunning)
                play = false;
            CancelInvoke("ParseCommand");
        }
    }
    
    //error handling
    //used in debugging or if something goes wrong in parsing
    void ErrorDisp(string error)
    {
        subtitle.GetComponent<Text>().color = Color.red;
        subtitle.GetComponent<Text>().text = "ERROR! " + error + " PLEASE CHECK YOUR SCRIPT AND RELOAD SCENE!";
    }

    //coroutine for moving an object over time
    public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        coroutineRunning = true;
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
        coroutineRunning = false;
        if (ended)
            play = false;
        //Debug.Log(coroutineRunning + "" + play);
    }


    /*Commands:
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
     * /animMove object speed (x y z) -
     * moves a character over speed time to x y z
     * 
     * /sub text -
     * displays a subtitle for x seconds
     * 
     * /view x -
     * switches view to camera x
     * 
     * /speed x -
     * sets the delay between lines, default is 1 second
     * 
     * /pause x -
     * pauses the scene
     * 
     * /addPos obj x y z -
     *  translates
     */
}
