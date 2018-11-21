using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene : MonoBehaviour {

    public Button button;
    // Use this for initialization
    void Start () {
        button.onClick.AddListener(change);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void change()
    {
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        string name = scene.name;

        if (name.Equals("cylinder"))
        {
            SceneManager.LoadScene("mesh");
        } else
        {
            SceneManager.LoadScene("cylinder");
        }
    }
}
