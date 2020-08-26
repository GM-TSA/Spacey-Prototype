using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jetpackCollectables : MonoBehaviour
{
    public bool isToggleable = false;
    public GameObject jetpackStatus;

    // Start is called before the first frame update
    void Start()
    {
        jetpackStatus = GameObject.FindGameObjectWithTag("healthBar");
    }

    // Update is called once per frame
    void Update()
    {
        Collect();
    }
    void Collect() {
        if(isToggleable && Input.GetKeyDown(KeyCode.E)) {
            jetpackStatus.GetComponent<FlightController>().changeStatus(-20);
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            isToggleable = true;
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            isToggleable = false;
        }
    }
}
