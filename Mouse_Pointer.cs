using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Pointer : MonoBehaviour {

    public GameObject pointer;
    public GameObject placeholder;

    public float mouseX;
    public float mouseY;
    public float mouseZ;

    public float storedX;
    public float storedY;
    public float storedZ;

    RaycastHit h;
    Ray r;

    GameObject rPoint;
    public GameObject bPoint;
    
    //runs every frame
    //handles placement of bPoint, the blue pointer that is used for saving x y z values
    void Update () {
        if (bPoint && this.GetComponent<Forge>().selected)
            bPoint.SetActive(false);
        else if(bPoint && !this.GetComponent<Forge>().selected)
            bPoint.SetActive(true);
        r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out h, 100.0f))
        {
            if (!rPoint)
                rPoint = Instantiate(pointer, this.transform);
            mouseX = h.point.x;
            mouseY = h.point.y;
            mouseZ = h.point.z;
            rPoint.transform.position = new Vector3(mouseX, mouseY, mouseZ);
            if (Input.GetMouseButtonDown(1) && !this.GetComponent<Forge>().selected)
            {
                storedX = mouseX;
                storedY = mouseY;
                storedZ = mouseZ;
                UpdateBluePoint(storedX, storedY, storedZ);
            }
        }
	}

    //actually creates bPoint at the given location
    public void UpdateBluePoint(float x, float y, float z)
    {
        if (!bPoint)
            bPoint = Instantiate(placeholder, this.transform);
        bPoint.transform.rotation = Quaternion.FromToRotation(h.point, h.normal);
        bPoint.transform.position = new Vector3(x, y, z);
    }
}
