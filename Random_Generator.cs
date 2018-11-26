using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Random_Generator : MonoBehaviour {

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
    * /animove object speed (x y z) -
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


    string[] commands = new string[]
    {
        "/color", "/move", "/scale", "/rotate", "/animove", "/sub", "/view", "/speed", "/pause", "/addPos"
    };

    public GameObject input;
    public GameObject roomHolder;
    public GameObject subtitle;
    GameObject selected;
    string fullText;
    int nameNumber = 0;
    string path;

    List<GameObject> parts;
    List<GameObject> cameras;

    //gets objects from parts and names them properly
    //always spawns a floor and at least 2 figures
    void makeSetting()
    {
        foreach(Transform child in roomHolder.transform)
        {
            Destroy(child.gameObject);
        }

        parts = new List<GameObject>();
        cameras = new List<GameObject>();
        nameNumber = 0;
        GameObject tmp = Instantiate(this.GetComponent<Obj_Lister>().parts[11], new Vector3(0 ,0 ,0), Quaternion.Euler(0, 0, 0));
        tmp.name = tmp.name.Replace("(Clone)", "");
        tmp.name += "|" + nameNumber++;
        tmp.transform.parent = roomHolder.transform;
        parts.Add(tmp);
        tmp = Instantiate(this.GetComponent<Obj_Lister>().parts[13], new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)), Quaternion.Euler(0, Random.Range(0, 360), 0));
        tmp.name = tmp.name.Replace("(Clone)", "");
        tmp.name += "|" + nameNumber++;
        tmp.transform.parent = roomHolder.transform;
        parts.Add(tmp);
        tmp = Instantiate(this.GetComponent<Obj_Lister>().parts[13], new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)), Quaternion.Euler(0, Random.Range(0, 360), 0));
        tmp.name = tmp.name.Replace("(Clone)", "");
        tmp.name += "|" + nameNumber++;
        tmp.transform.parent = roomHolder.transform;
        parts.Add(tmp);
        for (int i = 0; i < 10; i++)
        {
            tmp = Instantiate(this.GetComponent<Obj_Lister>().parts[(int)Random.Range(0, this.GetComponent<Obj_Lister>().parts.Count - 1)], new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)), Quaternion.Euler(Random.Range(0, 10), Random.Range(0, 360), Random.Range(0, 10)));
            tmp.name = tmp.name.Replace("(Clone)", "");
            tmp.name += "|" + nameNumber++;
            tmp.transform.parent = roomHolder.transform;
            parts.Add(tmp);
            if (tmp.name.Contains("Camera"))
                cameras.Add(tmp);
        }
        this.GetComponent<Data_Storage>().SaveSetting();
    }

    //gets a random object from the roomHolder object
    void getObject()
    {
        selected = parts[(int)Random.Range(0, parts.Count - 1)];
    }

    //adds a command to fullText
    void addHistory(string text)
    {
        fullText += text + "\n";
    }

    //generates a random script with "length" amount of commands
    public void GenerateRandom(int length)
    {
        input.GetComponent<InputField>().text = "randomScene";
        makeSetting();
        fullText = "";
        for(int i = 0; i < length; i++)
        {
            getObject();
            commands = new string[] {"/color " + selected.name.Replace("(Clone)", "") + " " + Random.Range(0, 254) + " " + Random.Range(0, 254) + " " + Random.Range(0, 254),
                /*"/move " + selected.name + " " + Random.Range(-5, 5) + " " + Random.Range(-5, 5) + " " + Random.Range(-5, 5),*/
                "/scale " + selected.name.Replace("(Clone)", "") + " " + Random.Range(0, 4) + " " + Random.Range(0, 4) + " " + Random.Range(0, 4),
                "/rotate " + selected.name.Replace("(Clone)", "") + " " + Random.Range(0, 360) + " " + Random.Range(0, 360) + " " + Random.Range(0, 360),
                "/animove " + selected.name.Replace("(Clone)", "") + " " + Random.Range(1, 10) + " " + Random.Range(-5, 5) + " " + Random.Range(-5, 5) + " " + Random.Range(-5, 5),
                "/speed " +  Random.Range(1, 5),
                "/pause " +  Random.Range(1, 10),
                "/addPos " + selected.name.Replace("(Clone)", "") +  " " + Random.Range(-5, 5) + " " + Random.Range(-5, 5) + " " + Random.Range(-5, 5)
            };

            addHistory(commands[(int)Random.Range(0, commands.Length - 1)].TrimEnd('\n'));
        }
        save();
    }

    //save the scene
    void save()
    {
        //path = EditorUtility.SaveFilePanel("Save Action Script to Folder", "", "randomScene" + "(Script).txt", "txt");
        try
        {
            path = "randomScene(Script).txt";
            if (path != "")
            {
                File.WriteAllText(path, "");
                StreamWriter w = new StreamWriter(path, true);
                w.Write(fullText);
                w.Close();
            }
        }
        catch
        {
            input.GetComponent<InputField>().text = "";
        }
    }

}
