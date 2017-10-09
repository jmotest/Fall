using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonStart : MonoBehaviour {

	public Button startButton;

	// Use this for initialization
	void Start () {
		Button btn = startButton.GetComponent<Button> ();
		btn.onClick.AddListener (TaskOnClick);
	}
	
	public void TaskOnClick () {
		SceneManager.LoadScene("Fall");
	}
}
