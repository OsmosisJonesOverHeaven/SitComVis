using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Switcher : MonoBehaviour {

    public GameObject cam;
    public Camera cam1;
    Vector3 cCamPos;
    Quaternion cCamRot;
    bool isMain = true;
    int index = 0;
    public List<Transform> list = new List<Transform>();


    //saves the given transform to the list of cameras
    public void SaveCamPos(Transform g)
    {
        if(!list.Contains(g))
            list.Add(g);
        Debug.Log("Camera Position Saved: " + g.name);
    }

    //changes camera position to the desired transform value
    public void ChangeCamPos(Transform g)
    {
        if (list.Contains(g)){
            Debug.Log("Camera Position Changed to: " + g.name);
            if (g.name == "Creator_Camera")
            {
                cam1.transform.position = cCamPos;
                cam1.transform.rotation = cCamRot;
                cam1.GetComponent<Camera>().orthographic = true;
                isMain = true;
            }
            else
            {
                cam1.transform.position = g.position;
                cam1.transform.rotation = g.rotation;
                cam1.transform.rotation = Quaternion.Euler(cam1.transform.rotation.eulerAngles.x + 90, cam1.transform.rotation.eulerAngles.z, 0);
                cam1.GetComponent<Camera>().orthographic = false;
                isMain = false;
            }
        }
    }

    //runs at the start of the program
    //adds the main camera to the list
    private void Start()
    {
        cCamPos = cam1.transform.position;
        cCamRot = cam1.transform.rotation;
        list.Add(cam1.transform);
    }

    //runs every frame
    //checks if the index is right
    private void Update()
    {
        if (list.Count == index)
            index = 0;
        /*Used for debugging
        if (Input.GetKeyDown("m"))
        {
            ChangeCamPos(list[index++]);
        }
        if (Input.GetKeyDown("n"))
        {
            ChangeCamPos(list[0]);
        }*/
        if (!isMain) {
            //cam1.transform.Rotate(-Input.GetAxis("Mouse Y"), 0, 0);
        }
    }
}
