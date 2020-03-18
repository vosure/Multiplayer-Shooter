using UnityEngine;

[RequireComponent (typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;

    [SerializeField]
    private float mouseSensitivity = 3;

    private PlayerMotor motor;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        Debug.Log(mouseSensitivity);
        Debug.Log(speed);
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMove;
        Vector3 moveVertical = transform.forward * zMove;
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;
        motor.Move(velocity);

        float yRotation = Input.GetAxis("Mouse X");
        Vector3 rotation = new Vector3(0.0f, yRotation, 0.0f) * mouseSensitivity;
        motor.Rotate(rotation);

        float xRotation = Input.GetAxis("Mouse Y");
        Vector3 cameraRotation = new Vector3(xRotation, 0.0f, 0.0f) * mouseSensitivity;
        motor.RotateCamera(cameraRotation);
    }


}
