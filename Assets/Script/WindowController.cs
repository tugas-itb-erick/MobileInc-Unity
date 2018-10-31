using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowController : MonoBehaviour {

    public Button closeButton;

    // Use this for initialization
    void Start ()
    {
        GameObject.Find("Main Menu Background/Canvas").GetComponent<Canvas>().enabled = false;
        closeButton.onClick.AddListener(CloseOnClick);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void CloseOnClick()
    {
        GameObject.Find("Main Menu Background/Canvas").GetComponent<Canvas>().enabled = true;
        Destroy(this.gameObject);
    }
}
