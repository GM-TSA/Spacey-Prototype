using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    static float mouseSensitivityX = 2f;
    static float mouseSensitivityY = 2f;
    private float verticalLookRotation;

    public float moveSpeed;
    public float jumpForce;
    public bool isGrounded;
    private float currentMoveSpeed;

    public float hoverGravity;
    public float orbitGravity;
    private float currentOrbitGravity;
    public float gravityLetoff;

    public Collider col;
    public Rigidbody rb;
    public GameObject myCamera;
    public GameObject orbitPoint;

    //Spaceship Stuff//
    public bool isToggleable = false;
    public bool isActivated = false;
    public GameObject player;
    public GameObject spaceshipCamera;

    public GameObject spaceshipStats;

    public void CameraRotation() {
       transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
       verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
       verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
       myCamera.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }
    public void ControllerMovement() {
       if (isGrounded) {
           currentMoveSpeed = moveSpeed;

           //Jumping//
           float addJumpForce = 0f;
           if (Input.GetButton("Jump") && spaceshipStats.GetComponent<FlightController>().currentStatus > 0) { 
               addJumpForce = jumpForce;
               spaceshipStats.GetComponent<FlightController>().changeStatus(35*Time.deltaTime);
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

           if (DistanceBetween(orbitPoint) > 4f && !Input.GetButton("Jump")) {
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

           if (distToSurface > 4.5 && distToSurface < 150) {
               gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.FromToRotation(gameObject.transform.up, targetDir) * gameObject.transform.rotation, 2f * Time.deltaTime);
           }
           else if (distToSurface < 4.5) {
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

   public void SpaceshipInteraction() {
       if (isToggleable && Input.GetKeyDown(KeyCode.E)) {
           isActivated = true;
           player.transform.parent = gameObject.transform;
           player.SetActive(false);
           spaceshipCamera.SetActive(true);
       }
       if (isToggleable && Input.GetKeyDown(KeyCode.Q)) {
           isActivated = false;
           player.transform.parent = null;
           player.SetActive(true);
           spaceshipCamera.SetActive(false);
       }
   }

   private void OnAwake () {
       spaceshipStats = GameObject.FindGameObjectWithTag("SpaceshipStats");
   }
    // Update is called once per frame
    private void Update() {
        SpaceshipInteraction();
        if (isActivated) {
            CameraRotation();
            ControllerMovement();
            ChangePlanet();
            FeetToGround();
            OrbitalPull();
        }
   }
   private void FixedUpdate() {
       //Null out Rigidbody Physics//
       rb.velocity = Vector3.zero;
       rb.angularVelocity = Vector3.zero;
   }

   private void OnTriggerEnter(Collider other) {
       if (other.CompareTag("Player")) {
           isToggleable = true;
       }
   }

   private void OnTriggerExit(Collider other) {
       if (other.CompareTag("Player")) {
           isToggleable = false;
       }
   }
}
