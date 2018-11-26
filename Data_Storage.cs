using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Data_Storage : MonoBehaviour {

    public GameObject input;
    public GameObject roomHolder;
    public GameObject objLister;
    string setting;
    public string sceneName;
    public string path;
    string storage;

    //called at the start of the program
    //determines if it needs to supply objLister
    private void Start()
    {
        if (!objLister)
            objLister = this.gameObject;
    }

    //called every frame
    //makes sure sceneName isnt null or empty
    private void Update()
    {
        if(sceneName == "")
            sceneName = "defaultScene";
        if (input)
            if (input.GetComponent<InputField>().text != "")
                sceneName = input.GetComponent<InputField>().text;
    }

    //changes sceneName to whatever is in the text box
    public void UpdateSceneName(GameObject text)
    {
        string tmp = text.GetComponent<Text>().text;
        sceneName = tmp.Replace(' ', '_');
    }
    
    //writes the list of objects and their properties, contained in the variable "content" to a file
    //opens an Explorer window
    //may contain artifacts as was used originally for saving actions too
    public void WriteScript(string title)
    {
        //try
        //{
            string content = "";
            if (title == "Setting")
                content = setting;
            //path = EditorUtility.SaveFilePanel("Save" + title + "to Folder","", sceneName + "(" + title + ").txt", "txt");
            path = sceneName + "(Setting).txt";
            //Debug.Log(path);
            if (path != "")
            {
                File.WriteAllText(path, "");
                StreamWriter w = new StreamWriter(path, true);
                w.Write(content);
                w.Close();
            }
        //}
        //catch
        //{
            //input.GetComponent<InputField>().text = "";
        //}
    }
    
    //writes the actions contained in "actionlist" to a file
    //opens an Explorer window
    public void SaveActions(string actionlist)
    {
        //path = EditorUtility.SaveFilePanel("Save Action Script to Folder", "", sceneName + "(Script).txt", "txt");
        path = sceneName + "(Script).txt";
        if (path != "")
        {
            File.WriteAllText(path, "");
            StreamWriter w = new StreamWriter(path, true);
            w.Write(actionlist);
            w.Close();
        }
    }

    //changes value of the string "setting" that contains all objects and their properties
    //runs WriteScript afterwards to save it
    public void SaveSetting()
    {
        foreach (Transform c in roomHolder.transform)
        {
            string[] tmp = c.gameObject.name.Split('|');
            setting += tmp[0] + "|" + c.position.x + "," + c.position.y + "," + c.position.z + "|" + c.eulerAngles.x + "," + c.eulerAngles.y + "," + c.eulerAngles.z + "|" + c.gameObject.name + "LSEP\n"; //line 1 is the object name, (x, y, z) and rotation, and object ID;
            StoreColors(c.gameObject); //line 2 is color
            setting += "HSEP\n"; //LSEP separates lines, HSEP separates objects
        }
        WriteScript("Setting");
    }

    //gets the colors of an object
    //may or may not be used in the future since color storage may only be used via commands and will be written to script anyway
    void StoreColors(GameObject p)
    {
        if (p.GetComponent<Renderer>())
        {
            setting += p.GetComponent<Renderer>().material.color.r + ","+ p.GetComponent<Renderer>().material.color.g + "," + p.GetComponent<Renderer>().material.color.b + "c";
            Material[] mats = p.GetComponent<Renderer>().materials;
            foreach (Material m in mats)
            {
                setting += m.color.r + "," + m.color.g + "," + m.color.b + "c";
            }
        }
        foreach (Transform child in p.transform)
        {
            if(child.GetComponent<Renderer>())
                setting += child.GetComponent<Renderer>().material.color.r + "," + child.GetComponent<Renderer>().material.color.g + "," + child.GetComponent<Renderer>().material.color.b + "c";
        }
    }

    //loads the setting of a scene
    //if p is empty it opens up an Explorer window, otherwise it uses the value already in path
    public void ReadSetting(string p)
    {
        try
        {
            ClearRoom();
            //if (p == "")
            path = input.GetComponent<InputField>().text + "(Setting).txt";
            Debug.Log(path);
            //path = EditorUtility.OpenFilePanel("Choose setting file", "", "txt");
            if (path != "")
            {
                
                storage = File.ReadAllText(path);
                string[] objects = storage.Split(new string[] { "HSEP" }, System.StringSplitOptions.None);
                objects[objects.Length - 1] = null;
                foreach (string s in objects)
                {
                    if (s != null)
                    {
                        
                        string[] lines = s.Split(new string[] { "LSEP" }, System.StringSplitOptions.None);
                        string[] stats = lines[0].Split('|');
                        string[] position = stats[1].Split(',');
                        string[] rotation = stats[2].Split(',');
                        foreach (GameObject g in objLister.GetComponent<Obj_Lister>().parts)
                        {
                            if (stats[0].TrimEnd(' ').Replace("\n", "") == (g.name))
                            {
                                GameObject tmp = Instantiate(g, new Vector3(float.Parse(position[0]), float.Parse(position[1]), float.Parse(position[2])), Quaternion.Euler(float.Parse(rotation[0]), float.Parse(rotation[1]), float.Parse(rotation[2])), roomHolder.transform);
                                tmp.name = stats[3] + "|" + stats[4];
                            }
                        }
                    }
                }
            }
        }
        catch
        {
            input.GetComponent<InputField>().text = "";
        }
    }

    //clears all the objects currently spawned in the scene
    public void ClearRoom()
    {
        foreach(Transform child in roomHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
