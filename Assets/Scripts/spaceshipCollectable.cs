using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceshipCollectable : MonoBehaviour
{
    public bool isToggleable = false;
    public GameObject spaceshipStatus;

    // Start is called before the first frame update
    void Start()
    {
        spaceshipStatus = GameObject.FindGameObjectWithTag("SpaceshipStats");
    }

    // Update is called once per frame
    void Update()
    {
        Collect();
    }
    void Collect() {
        if(isToggleable && Input.GetKeyDown(KeyCode.E)) {
            spaceshipStatus.GetComponent<FlightController>().changeStatus(-20);
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
