using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpDrive : MonoBehaviour
{
    public string nextLevel;
    public bool isToggleable = false;

    void NextScene () {
        if(isToggleable && Input.GetKeyDown(KeyCode.E)) {
            SceneManager.LoadScene(nextLevel);
        }
    }

    // Update is called once per frame
    void Update()
    {
        NextScene();
    }
    void OnTriggerEnter (Collider other) {
        if (other.CompareTag("Player")) {
            isToggleable = true;
        }
    }

    void OnTriggerExit (Collider other) {
        if (other.CompareTag("Player")) {
            isToggleable = false;
        }
    }
}
