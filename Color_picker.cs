using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Color_picker : MonoBehaviour {

    Color current;
    public bool colSelected;
    public GameObject Tile;
    Color[] defColors = new Color[8];
    Color[] custColors = new Color[2];
    List<GameObject> tiles = new List<GameObject>();

    int xPos;
    int yPos;

    public void GenerateList()
    {
        int index = 0;
        foreach(Color i in defColors)
        {
            tiles[index] = Instantiate(Tile, new Vector3(xPos, yPos, 0), Quaternion.Euler(new Vector3(0, 0, 0)), this.transform);
            tiles[index].GetComponent<Image>().color = i;
            tiles[index].GetComponent<Button>().onClick.AddListener(delegate { ChangeCurrent(i);});
        }
    }

    public void ChangeCurrent(Color c)
    {

    }

	void Start () {
		
	}
	
	
	void Update () {
		
	}
}
