﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   //Camera Rotation//
   static float mouseSensitivityX = 2f;
   static float mouseSensitivityY = 2f;
   private float verticalLookRotation;

   //Controller Movement//
   public float moveSpeed;
   public float sprintSpeed;
   public float jumpForce;
   public bool isGrounded;
   private float currentMoveSpeed;

   //Gravity//
   public float hoverGravity;
   public float orbitGravity;
   private float currentOrbitGravity;
   public float gravityLetoff;

   //Objects and Componenets//
   public Collider col;
   public Rigidbody rb;
   public GameObject myCamera;
   public GameObject orbitPoint;
   
   //Jetpack//
   public GameObject jetpackSlider;

   //Character Activation (For Interactions)//
   public bool isActive = true;

   //Core Functions//
   public void CameraRotation() {
       transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
       verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
       verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
       myCamera.transform.localEulerAngles = Vector3.left * verticalLookRotation;
   }
   public void ControllerMovement() {
       if (isGrounded && isActive) {
           //Sprinting//
           if (Input.GetButton("Run")) { currentMoveSpeed = sprintSpeed; } else { currentMoveSpeed = moveSpeed; }

           //Jumping//
           float addJumpForce = 0f;
           if (Input.GetButton("Jump") && jetpackSlider.GetComponent<FlightController>().currentStatus > 0) { 
               addJumpForce = jumpForce; 
               jetpackSlider.GetComponent<FlightController>().changeStatus(20 * Time.deltaTime);
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
   }

   public void OrbitalPull() {
       //Inward Rotation to Core//
       Vector3 targetDir = (gameObject.transform.position - orbitPoint.transform.position).normalized;

       //Normal Spherical Movement Application//
       if (isGrounded) {
           gameObject.transform.rotation = Quaternion.FromToRotation(gameObject.transform.up, targetDir) * gameObject.transform.rotation;

           if (DistanceBetween(orbitPoint) > 1.5f && !Input.GetButton("Jump")) {
               transform.Translate(new Vector3(0, -hoverGravity * Time.deltaTime, 0));
           }
       }
       //Move Toward new Planet//
       else {
           transform.Translate(
               (-targetDir.x * Time.deltaTime * orbitGravity),
               (-targetDir.y * Time.deltaTime * orbitGravity),
               (-targetDir.z * Time.deltaTime * orbitGravity), Space.World
           );
       }
   }
   public void ChangePlanet() {
       if(DistanceBetween(orbitPoint) > gravityLetoff && isGrounded == true) {
           orbitPoint = ClosestPlanet();
           isGrounded = false;
       }
   }
   public void FeetToGround() {
       if(!isGrounded) {
           Vector3 targetDir = (gameObject.transform.position - orbitPoint.transform.position).normalized;
           float distToSurface = DistanceBetween(orbitPoint);

           if (distToSurface > 2 && distToSurface < 150) {
               gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.FromToRotation(gameObject.transform.up, targetDir) * gameObject.transform.rotation, 1f * Time.deltaTime);
           }
           else if (distToSurface < 2) {
               isGrounded = true;
           }
       }
   }

   public GameObject ClosestPlanet() {
       GameObject returnPlanet = null;
       float shortestDistance = Mathf.Infinity;

       orbitPoint.gameObject.tag = "NotPlanet";
       GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
       orbitPoint.gameObject.tag = "Planet";

       foreach (GameObject obj in planets) {
           float distanceBetween = DistanceBetween(obj);

           if (distanceBetween < shortestDistance) {
               shortestDistance = distanceBetween;
               returnPlanet = obj;
           }
       }

       return returnPlanet;
   }
   public float DistanceBetween(GameObject planet) {
       float radius = planet.transform.localScale.y / 2;
       Vector3 outerPoint = planet.transform.position;
       outerPoint += ((gameObject.transform.position - planet.transform.position).normalized) * radius;
       return Vector3.Distance(outerPoint, gameObject.transform.position);
   }

   private void Awake () {
       jetpackSlider = GameObject.FindGameObjectWithTag("healthBar");
   }
   //Implimentation//
   private void Update() {
       CameraRotation();
       ControllerMovement();
       ChangePlanet();
       FeetToGround();
       OrbitalPull();
   }
   private void FixedUpdate() {
       //Null out Rigidbody Physics//
       rb.velocity = Vector3.zero;
       rb.angularVelocity = Vector3.zero;
   }
}
