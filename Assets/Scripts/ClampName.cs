using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClampName : MonoBehaviour
{

    public Slider SliderLabel;


    // Update is called once per frame
    void Update()
    {
        Vector3 sliderPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        SliderLabel.transform.position = sliderPosition;
    }
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            SliderLabel.gameObject.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            SliderLabel.gameObject.SetActive(false);
        }
    }
}
