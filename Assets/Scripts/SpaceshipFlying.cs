using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipFlying : MonoBehaviour
{
    public float moveSpeed;
    private float currentMoveSpeed;
    public float jumpForce;

    static float mouseSensitivityX = 2f;
    static float mouseSensitivityY = 2f;
    private float verticalLookRotation;
    public GameObject myCamera;

    public void Movement () {
        currentMoveSpeed = moveSpeed;

           //Jumping//
           float addJumpForce = 0f;
           if (Input.GetButton("Jump")) { 
               addJumpForce = jumpForce;
           }

           //Flat Directional Movement//
           Vector3 moveDir = new Vector3(
               Input.GetAxisRaw("Horizontal") * currentMoveSpeed * Time.deltaTime,
               addJumpForce * Time.deltaTime,
               Input.GetAxisRaw("Vertical") * currentMoveSpeed * Time.deltaTime
           );

           //Translation
           transform.Translate(moveDir * currentMoveSpeed);
    }
    public void CameraRotation() {
       transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
       verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
       verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
       myCamera.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        CameraRotation();
    }
}
