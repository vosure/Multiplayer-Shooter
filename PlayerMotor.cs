using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;

    private Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

    }

    public void Move(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    public void Rotate(Vector3 rotation)
    {
        this.rotation = rotation;
    }

    public void RotateCamera(Vector3 cameraRotation)
    {
        this.cameraRotation = cameraRotation;
    }

    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
        }
    }

    void PerformRotation()
    {
        if (rotation != Vector3.zero)
        {
            rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(rotation));

            if (camera != null)
            {
                camera.transform.Rotate(-cameraRotation);
            }
        }
    }
}
