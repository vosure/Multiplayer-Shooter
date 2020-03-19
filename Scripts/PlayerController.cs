using UnityEngine;

[RequireComponent (typeof(ConfigurableJoint))]
[RequireComponent (typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;

    [SerializeField]
    private float mouseSensitivity = 3;

    [SerializeField]
    private float thrusterForce = 1000f;

    [Header("Spring settings")]
    [SerializeField]
    private JointDriveMode jointMode = JointDriveMode.Position;
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    private PlayerMotor motor;
    private ConfigurableJoint joint;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();

        SetJointSettings(jointSpring);
    }

    private void Update()
    {
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
        float cameraRotation = xRotation * mouseSensitivity;
        motor.RotateCamera(cameraRotation);

        Vector3 thrusterForce = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            thrusterForce = Vector3.up * this.thrusterForce;
            SetJointSettings(0.0f);
        }
        else
        {
            SetJointSettings(jointSpring);
        }
        motor.ApplyThruster(thrusterForce);
    }

    private void SetJointSettings(float jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            mode = jointMode,
            positionSpring = jointSpring,
            maximumForce = jointMaxForce
        };
    }

}
