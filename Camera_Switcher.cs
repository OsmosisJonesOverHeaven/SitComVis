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

    public void SaveCamPos(Transform g)
    {
        if(!list.Contains(g))
            list.Add(g);
        Debug.Log("Camera Position Saved: " + g.name);
    }

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

    private void Start()
    {
        cCamPos = cam1.transform.position;
        cCamRot = cam1.transform.rotation;
        list.Add(cam1.transform);
    }

    private void Update()
    {
        if (list.Count == index)
            index = 0;
        if (Input.GetKeyDown("m"))
        {
            //ChangeCamPos(list[index++]);
        }
        if (Input.GetKeyDown("n"))
        {
            //ChangeCamPos(list[0]);
        }
        if (!isMain) {
            //cam1.transform.Rotate(-Input.GetAxis("Mouse Y"), 0, 0);
        }
    }
}
