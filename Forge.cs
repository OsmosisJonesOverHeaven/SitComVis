using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Forge : MonoBehaviour {

    Camera_Manipulator cm;

    public GameObject point;
    public GameObject selected;
    public GameObject CurrentObj;
    GameObject tmp;
    GameObject canvas;
    Camera cam;
    GameObject room_holder;

    float xSpd = .05f;
    float ySpd = .05f;
    float rotationSpeed = 1f;

    public bool rotationSnap = true;
    public int snapScale = 5;

    Color mainColor;
    int numColors;
    Color[] c;
    Color[] stackedColors;
    //stores colors of an object p while p is selected by the user
    //checks if the parent transform has children
    void StoreColors(GameObject p)
    {
        if (p.GetComponent<Renderer>())
        {
            mainColor = p.GetComponent<Renderer>().material.color;
            Material[] mats = selected.GetComponent<Renderer>().materials;
            stackedColors = new Color[mats.Length];
            int ind = 0;
            foreach (Material m in mats)
            {
                stackedColors[ind] = m.color;
                ind++;
            }
        }
        foreach(Transform child in p.transform)
        {
            numColors++;
        }
        c = new Color[numColors + 1];
        for(int i = 0; i < numColors; i++)
        {
            if(p.transform.GetChild(i).GetComponent<Renderer>())
                c[i] = p.transform.GetChild(i).GetComponent<Renderer>().material.color;
        }
    }
    //puts the stored colors back on the selected object
    void CallColor(GameObject p)
    {
        if (p.GetComponent<Renderer>())
        {
            p.GetComponent<Renderer>().material.color = mainColor;
            Material[] mats = selected.GetComponent<Renderer>().materials;
            int ind = 0;
            foreach (Material m in mats)
            {
                m.color = stackedColors[ind];
                ind++;
            }
        }
        for (int i = 0; i < numColors; i++)
        {
            if(p.transform.GetChild(i).GetComponent<Renderer>())
                p.transform.GetChild(i).GetComponent<Renderer>().material.color = c[i];
        }
        numColors = 0;
        c = null;
    }
    //colors an object and all its children red
    //used for selection
    void ColorRed()
    {
        if (selected.GetComponent<Renderer>())
        {
            //for multiple materials on one object
            Material[] mats = selected.GetComponent<Renderer>().materials;
            foreach (Material m in mats)
            {
                m.color = Color.red;
            }
        }
        //for materials in child objects
        foreach (Transform child in selected.transform)
        {
            if(child.GetComponent<Renderer>())
                child.GetComponent<Renderer>().material.color = Color.red;
        }
        if (selected.GetComponent<Renderer>())
            selected.GetComponent<Renderer>().material.color = Color.red;
    }

    //shows the GUI
    void SpawnMenu()
    {
        cm.RightMenu();
        canvas.SetActive(true);
    }

    //hides the GUI, makes the camera bigger
    void RemoveMenu()
    {
        cm.Revert();
        canvas.SetActive(false);
    }

    //creates an object based off requested prefab g, selects it
    public void SpawnObject(GameObject g)
    {
        Vector3 location = new Vector3(0, 0, 0);
        Quaternion rotation = g.transform.rotation;
        if (point)
            location = point.transform.position;
        canvas.SetActive(false);
        cm.Revert();
        selected = Instantiate(g, location, rotation);
        selected.transform.parent = room_holder.transform;
        StoreColors(selected);
        ColorRed();
    }

    //deselects current selected object
    public void Drop()
    {
        CallColor(selected);
        selected = null;
        SpawnMenu();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //plays at start of program
    //finds all the objects it needs
    void Start () {
        canvas = GameObject.Find("Canvas");
        cam = GameObject.Find("Creator_Camera").GetComponent<Camera>();
        cm = this.gameObject.GetComponent<Camera_Manipulator>();
        room_holder = GameObject.Find("Room_Holder");
	}
	
	//runs every frame
    //handles selected items and general mouse and keyboard actions
	void Update () {
        if (selected)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                selected.transform.Rotate((Input.GetAxis("Vertical") + Input.GetAxis("Mouse Y") * rotationSpeed), (Input.GetAxis("Horizontal") + Input.GetAxis("Mouse X")) * rotationSpeed, 0, Space.World);
                if (Input.GetKey(KeyCode.Q))
                    selected.transform.Rotate(0, 0, rotationSpeed * 50 * Time.deltaTime);
                else if (Input.GetKey(KeyCode.E))
                    selected.transform.Rotate(0, 0, rotationSpeed * -50 * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.LeftControl))
                selected.transform.Translate(0, (Input.GetAxis("Vertical") + Input.GetAxis("Mouse Y") * ySpd / 2), 0, Space.World);
            else
                selected.transform.Translate((Input.GetAxis("Horizontal") + Input.GetAxis("Mouse X") - Input.GetAxis("Mouse Y")) * xSpd, 0, (Input.GetAxis("Vertical") + Input.GetAxis("Mouse Y") + Input.GetAxis("Mouse X")) * ySpd, Space.World);
            if (rotationSnap && Input.GetKeyUp(KeyCode.LeftShift))
                selected.transform.Rotate(selected.transform.rotation.x % snapScale, selected.transform.rotation.y % snapScale, selected.transform.rotation.z % snapScale, Space.World);
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Drop();
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                tmp = selected;
                Drop();
                Destroy(tmp);
            }
        }
        else
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
                CurrentObj.transform.GetChild(0).GetComponent<Text>().text = hit.collider.name;
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                
                if (Physics.Raycast(ray, out hit) && hit.collider.tag != "NoClick")
                {
                    selected = hit.collider.gameObject;
                    StoreColors(selected);
                    ColorRed();
                    RemoveMenu();
                }
                
            }
        }
        if (this.GetComponent<Mouse_Pointer>().bPoint)
        {
            point = this.GetComponent<Mouse_Pointer>().bPoint;
        }
	}
}
