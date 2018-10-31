using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinLose : MonoBehaviour {

    public Text msg;
    public Button btn;
    public RectTransform panel;

	// Use this for initialization
	void Start () {
        msg = GetComponentInChildren<Text>();
        btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(OnContinue);
        panel = GetComponentInChildren<RectTransform>();


        if (PlayerPrefs.GetInt("Win", 0) == 0)
        {
            msg.text = "You Lose";
        }
        else
        {
            msg.text = "You Win";
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnContinue()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
