using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obj_Lister : MonoBehaviour {

    public List<GameObject> parts; 
    public GameObject button;
    GameObject searchBox;
    Forge cont;
    GameObject canvas;

    GameObject tmp;
    int posX, posY;
    public bool canScrollUp = true;
    public bool canScrollDown = true;
    List<GameObject> buttons;

    private void Start()
    {
        cont = GetComponent<Forge>();
        canvas = GameObject.Find("Canvas");
        searchBox = GameObject.Find("searchFilter");
        tmp = null;
        buttons = new List<GameObject>();
        GenerateList("");
    }

    void GenerateList(string search)
    {
        ClearButtons();
        posY = Screen.height - 30;
        foreach(GameObject i in parts)
        {
            if (search == "" || i.name.Contains(search))
            {
                tmp = Instantiate(button);
                tmp.transform.SetParent(canvas.transform);
                tmp.transform.position = new Vector3(Screen.width / 2 + 100, posY, 0);
                tmp.GetComponent<Button>().onClick.AddListener(delegate { cont.SpawnObject(i); });
                tmp.transform.GetChild(0).GetComponent<Text>().text = i.name;
                tmp.GetComponent<Button_Controls>().ol = this;

                posY -= 30;
                buttons.Add(tmp);
            }
        }
        tmp = null;
    }

    void ClearButtons()
    {
        foreach(GameObject i in buttons)
        {
            Destroy(i);
            posY += 30;
        }
        buttons = new List<GameObject>();
    }

    public void Search()
    {
        GenerateList(searchBox.GetComponent<InputField>().text);
    }

    private void Update()
    {
        if (buttons != null && buttons.Count != 0)
        {
            if (buttons[0].transform.position.y < 50)
                canScrollDown = false;
            else if (buttons[buttons.Count - 1].transform.position.y > Screen.height - 50)
                canScrollUp = false;
            else
            {
                canScrollDown = true;
                canScrollUp = true;
            }
        }
    }

}
