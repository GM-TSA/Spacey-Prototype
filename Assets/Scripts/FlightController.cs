using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlightController : MonoBehaviour
{
    private Slider jetpackStatus;
    public float currentStatus;

    void Awake() {
        jetpackStatus = GetComponent<Slider>();
        currentStatus = 100;
    }

    // Update is called once per frame
    void Update()
    {
        jetpackStatus.value = currentStatus;
    }

    public void changeStatus(float dStat) {
        currentStatus -= dStat;
    }
}
