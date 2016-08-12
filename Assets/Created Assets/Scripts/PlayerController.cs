using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivty = 3f;
    private PlayerMotor motor;

    [SerializeField]
    private float thrusterForce = 1000f;

    [Header("Joint Options:")]
    //[SerializeField]
    //private JointDriveMode jointmode;
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    private ConfigurableJoint joint;


    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        SetJointSettings(jointSpring);
    }

    void Update()
    {
        //Calculate velocity as a 3D vector

        float _xMovement = Input.GetAxisRaw("Horizontal");
        float _zMovement = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _xMovement;
        Vector3 _moveVertical = transform.forward * _zMovement;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * speed;

        motor.Move(_velocity);

        //Calculate Rotation
        float _yRotation = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRotation, 0f) * lookSensitivty;

        motor.Rotate(_rotation);

        //Calculate Camera Rotation
        float _xRotation = Input.GetAxisRaw("Mouse Y");

        float _camerarotation = _xRotation * lookSensitivty;

        motor.RotateCamera(_camerarotation);

        //Apply high jump
        Vector3 _thrusterForce = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        }else
        {
            SetJointSettings(jointSpring);
        }

        motor.ApplyThruster(_thrusterForce);
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive {positionSpring = _jointSpring, maximumForce = jointMaxForce };
    }
}
