using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class Data_Storage : MonoBehaviour {

    public GameObject roomHolder;
    public GameObject objLister;
    //public List<GameObject> placedObjects = new List<GameObject>();
    string setting;
    public string sceneName;
    public string path;
    string storage;

    private void Update()
    {
        if(sceneName == "")
            sceneName = "defaultScene";
    }

    public void UpdateSceneName(GameObject text)
    {
        string tmp = text.GetComponent<Text>().text;
        sceneName = tmp.Replace(' ', '_');
    }
    
    public void WriteScript(string title)
    {
        string content = "";
        if (title == "Setting")
            content = setting;
        path = EditorUtility.SaveFilePanel("Save" + title + "to Folder","", sceneName + "(" + title + ").txt", "txt");
        //Debug.Log(path);
        if (path != "")
        {
            File.WriteAllText(path, "");
            StreamWriter w = new StreamWriter(path, true);
            w.Write(content);
            w.Close();
        }
    }
    
    public void SaveActions(string actionlist)
    {
        path = EditorUtility.SaveFilePanel("Save Action Script to Folder", "", sceneName + "(Script).txt", "txt");
        if (path != "")
        {
            File.WriteAllText(path, "");
            StreamWriter w = new StreamWriter(path, true);
            w.Write(actionlist);
            w.Close();
        }
    }

    //REGARDING saving everything
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

    //REGARDING loading everything
    public void ReadSetting()
    {
        ClearRoom();
        path = EditorUtility.OpenFilePanel("Choose setting file", "", "txt");
        if(path != "")
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
                    //string[] colors = lines[1].Split('c');
                    string[] position = stats[1].Split(',');
                    string[] rotation = stats[2].Split(',');
                    foreach (GameObject g in objLister.GetComponent<Obj_Lister>().parts)
                    {
                        if (stats[0].TrimEnd(' ').Replace("\n", "") == (g.name))
                        {
                            GameObject tmp = Instantiate(g, new Vector3(float.Parse(position[0]), float.Parse(position[1]), float.Parse(position[2])), Quaternion.Euler(float.Parse(rotation[0]), float.Parse(rotation[1]), float.Parse(rotation[2])), roomHolder.transform);
                            tmp.name = stats[3] + "|" + stats[4];
                            //color time
                            /*
                            string[] colTmp = colors[0].Split(',');
                            int index = 0;
                            if (tmp.GetComponent<Renderer>())
                            {
                                Material[] mats = tmp.GetComponent<Renderer>().materials;
                                foreach (Material m in mats)
                                {
                                    colTmp = colors[index].Split(',');
                                    m.color = new Color(float.Parse(colTmp[0]), float.Parse(colTmp[1]), float.Parse(colTmp[2]));
                                    index++;
                                }
                            }
                            foreach (Transform child in tmp.transform)
                            {  
                                colTmp = colors[index].Split(',');


                                int ind = 0;
                                foreach (string c in colTmp)
                                {
                                    Debug.Log(ind + " |" + c + "|");
                                    ind++;
                                }
                                Debug.Log(child.gameObject.name);
                                if (child.GetComponent<Renderer>())
                                    child.GetComponent<Renderer>().material.color = new Color(float.Parse(colTmp[0]), float.Parse(colTmp[1]), float.Parse(colTmp[2]));
                                index++;
                            }
                            */
                        }
                    }
                }
            }
        }
    }

    public void ClearRoom()
    {
        foreach(Transform child in roomHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
