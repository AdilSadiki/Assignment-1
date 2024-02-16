using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 50f;

    private Ray groundRay;
    private Ray barrelRay;
    private GameObject barrel;

    private float forwardInput;
    private float rotationInput;

    private void Start()
    {
        groundRay = new Ray(transform.position, Vector3.down);
        barrel = transform.GetChild(2).gameObject;
        barrelRay = new Ray(barrel.transform.position, Vector3.up);
    }

    private void Update()
    {
        ProcessMovementInput();
        ProcessRotationInput();
    }

    private void FixedUpdate()
    {
        UpdateGroundRotation();
    }

    public void Movement(InputAction.CallbackContext value)
    {
        forwardInput = value.ReadValue<float>();
        Debug.Log(forwardInput);
    }

    public void Rotation(InputAction.CallbackContext value)
    {
        rotationInput = value.ReadValue<float>();
        Debug.Log(rotationInput);
    }

    public void Fire()
    {
        RayCastBarrel();
        Debug.Log("Shoot!");
    }

    private void ProcessMovementInput()
    {
        var movement = Vector3.forward * (forwardInput * movementSpeed * Time.deltaTime);
        transform.Translate(movement);
    }

    private void ProcessRotationInput()
    {
        transform.Rotate(Vector3.up * (rotationInput * rotationSpeed * Time.deltaTime));
    }

    private void UpdateGroundRotation()
    {
        groundRay.origin = transform.position;
        groundRay.direction = Vector3.down;

        if (Physics.Raycast(groundRay, out RaycastHit groundHit))
        {
            Vector3 groundNormal = groundHit.normal;
            Quaternion rotationToGround = Quaternion.FromToRotation(transform.up, groundNormal);
            transform.rotation = rotationToGround * transform.rotation;
            Debug.DrawRay(groundRay.origin, groundRay.direction, Color.red);
        }
    }

    private void RayCastBarrel()
    {
        barrelRay.origin = barrel.transform.position;
        barrelRay.direction = barrel.transform.up;

        if (Physics.Raycast(barrelRay, out RaycastHit barrelHit))
        {
            Debug.DrawRay(barrelRay.origin, barrel.transform.TransformDirection(Vector3.up) * 10, Color.blue);
            var hitObject = barrelHit.collider.gameObject;

            Debug.Log(hitObject.tag);
            if (!hitObject.CompareTag("unpaintable"))
            {
                hitObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
            }

            Debug.Log(barrelHit.collider);
        }
    }
}
