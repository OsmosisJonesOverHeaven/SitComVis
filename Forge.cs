using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forge : MonoBehaviour {

    Camera_Manipulator cm;

    public GameObject point;
    public GameObject selected;
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
            c[i] = p.transform.GetChild(i).GetComponent<Renderer>().material.color;
        }
    }
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
            p.transform.GetChild(i).GetComponent<Renderer>().material.color = c[i];
        }
        numColors = 0;
        c = null;
    }

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
            child.GetComponent<Renderer>().material.color = Color.red;
        }
        if (selected.GetComponent<Renderer>())
            selected.GetComponent<Renderer>().material.color = Color.red;
    }

    void SpawnMenu()
    {
        cm.RightMenu();
        canvas.SetActive(true);
    }

    void RemoveMenu()
    {
        cm.Revert();
        canvas.SetActive(false);
    }

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

    public void Drop()
    {
        CallColor(selected);
        selected = null;
        SpawnMenu();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Start () {
        canvas = GameObject.Find("Canvas");
        cam = GameObject.Find("Creator_Camera").GetComponent<Camera>();
        cm = this.gameObject.GetComponent<Camera_Manipulator>();
        room_holder = GameObject.Find("Room_Holder");
        //SpawnMenu();
	}
	
	
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
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
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
