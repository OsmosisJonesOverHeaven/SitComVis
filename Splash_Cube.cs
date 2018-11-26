using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash_Cube : MonoBehaviour {

    public float speed = 500;

    //makes the GUI for the Scene Player look cool
    //spins the purple cubes around
	void Update () {
        transform.Rotate(0, speed * Time.deltaTime * (1 + Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y"))), 0);
	}
}
