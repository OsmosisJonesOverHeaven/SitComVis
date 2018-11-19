using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMover : MonoBehaviour {

    public GameObject cam;
    float speed = 10.0f;
    float zoomSpeed = 5.0f;

	void Update () {
        if (!this.GetComponent<Forge>().selected && !EventSystem.current.IsPointerOverGameObject())
        {
            cam.transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0);
            if (Input.GetKey(KeyCode.LeftShift))
                cam.GetComponent<Camera>().orthographicSize -= zoomSpeed * Time.deltaTime * Input.GetAxis("Vertical");
            else
                cam.transform.Translate(Vector3.up * Input.GetAxis("Vertical") * speed * Time.deltaTime);
        }
    }
}
