using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class Data_Storage : MonoBehaviour {

    public GameObject roomHolder;
    public List<GameObject> placedObjects = new List<GameObject>();
    string setting;
    public string sceneName;
    public string path;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SaveSetting();
        }
        if(sceneName == "")
        {
            sceneName = "defaultScene";
        }
    }

    public void UpdateSceneName(GameObject text)
    {
        string tmp = text.GetComponent<Text>().text;
        sceneName = tmp.Replace(' ', '_');
        //Debug.Log(sceneName);
    }
    
    public void WriteScript(string title)
    {
        string content = "";
        if (title == "Setting")
            content = setting;
        path = EditorUtility.SaveFilePanel("Save" + title + "to Folder","", sceneName + "(" + title + ").txt", "txt");
        Debug.Log(path);
        if (path != "")
        {
            File.WriteAllText(path, "");
            StreamWriter w = new StreamWriter(path, true);
            w.Write(content);
            w.Close();
        }
    }
    


    //REGARDING saving setting
    public void SaveSetting()
    {
        //var path = EditorUtility.OpenFolderPanel("Choose Scene Directory", "", "");
        foreach (Transform c in roomHolder.transform)
        {
            string[] tmp = c.gameObject.name.Split('|');
            setting += tmp[0] + "|" + c.position.x + "," + c.position.y + "," + c.position.z + "|" + c.rotation.x + "," + c.rotation.y + "," + c.rotation.z + "|" + c.gameObject.name + "LSEP\n"; //line 1 is the object name, (x, y, z) and rotation, and object ID;
            StoreColors(c.gameObject); //line 2 is color
            setting += "HSEP\n"; //LSEP separates lines, HSEP separates objects
            //Debug.Log(setting);
        }
        WriteScript("Setting");
    }

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
            setting += child.GetComponent<Renderer>().material.color.r + "," + child.GetComponent<Renderer>().material.color.g + "," + child.GetComponent<Renderer>().material.color.b + "c";
        }
    }

}
