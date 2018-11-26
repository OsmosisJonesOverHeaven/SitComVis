using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Obj_Lister : MonoBehaviour {

    public List<GameObject> parts; 
    public GameObject button;
    GameObject searchBox;
    Forge cont;
    Camera_Manipulator camM;
    GameObject canvas;
    public GameObject location;

    GameObject tmp;
    float posX, posY;
    public bool canScrollUp = true;
    public bool canScrollDown = true;
    public bool canScroll = true;
    List<GameObject> buttons;

    public bool list;

    //runs at the start of the program
    //finds gameobjects and initializes variables
    private void Start()
    {
        if (list)
        {
            cont = GetComponent<Forge>();
            canvas = GameObject.Find("Canvas");
            searchBox = GameObject.Find("searchFilter");
            tmp = null;
            buttons = new List<GameObject>();
            GenerateList("");
        }
    }

    /*take height of screen
     * divide by number of parts
     * set button height to that division
     * move down by height
     */

    //creates the list of objects based off prefabs in list
    void GenerateList(string search)
    {
        ClearButtons();
        posY = location.transform.position.y;
        foreach(GameObject i in parts)
        {
            if (search == "" || i.name.Contains(search))
            {
                tmp = Instantiate(button, location.transform, false);
                tmp.SetActive(true);
                tmp.transform.SetParent(location.transform);
                tmp.transform.position = new Vector3(location.transform.position.x, posY, 0);
                tmp.GetComponent<Button>().onClick.AddListener(delegate { cont.SpawnObject(i); });
                tmp.transform.GetChild(0).GetComponent<Text>().text = i.name;
                tmp.GetComponent<Button_Controls>().ol = this;
                posY -= tmp.GetComponent<RectTransform>().sizeDelta.y * canvas.GetComponent<Canvas>().scaleFactor;
                buttons.Add(tmp);
            }
        }
        tmp = null;
    }

    //gets rid of all the buttons
    //used for search function
    void ClearButtons()
    {
        foreach(GameObject i in buttons)
        {
            Destroy(i);
            posY += 30;
        }
        buttons = new List<GameObject>();
    }

    //generates a new list using only the input keyword
    public void Search()
    {
        GenerateList(searchBox.GetComponent<InputField>().text);
    }

    //called every frame
    //handles button scrolling
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            canScroll = true;
        }
        else
        {
            canScroll = false;
            canScrollDown = false;
            canScrollUp = false;
        }
    }

}
