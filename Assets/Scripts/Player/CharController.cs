using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {

    [System.Serializable]
    public class MoveSettings
    {
        public float forwardSpeed = 100;
        public float rotSpeed = 80;
        public float jumpSpeed = 25;
        public float distToGrounded = 0.1f;
        public LayerMask ground;
    }
    [System.Serializable]
    public class PhysSettings
    {
        public float downAccel = 0.75f;
    }

    [System.Serializable]
    public class InputSettings
    {
        public float inputDelay = 0.1f;
        public string FORWARD_AXIS = "Vertical";
        public string TURN_AXIS = "Horizontal";
        public string JUMP_AXIS = "Jump";
    }

    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();

    Vector3 velocity = Vector3.zero;
    private float forwardInput, rotInput, jumpInput;
    private Quaternion targetRot;
    private Rigidbody rb;

    public Quaternion TargetRotation {
        get { return targetRot; }
    }

    bool Grounded() {
        return Physics.Raycast(transform.position, Vector3.down, moveSetting.distToGrounded, moveSetting.ground);
    }

	// Use this for initialization
	void Start () {
        targetRot = transform.rotation;
        if (GetComponent<Rigidbody>())
            rb = GetComponent<Rigidbody>();
        else
            Debug.LogError("There is no rigidbody on the character");

        forwardInput = 0;
        rotInput = 0;
	}

    void GetInput()
    {
        forwardInput = Input.GetAxis(inputSetting.FORWARD_AXIS);
        rotInput = Input.GetAxis(inputSetting.TURN_AXIS);
        jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS);       // Not Interpolated
    }

    // Update is called once per frame
    void Update () {
        GetInput();
        Turn();
	}

    void FixedUpdate() {
        Run();
        Jump();

        rb.velocity = transform.TransformDirection(velocity);
    }

    void Run() {
        if (Mathf.Abs(forwardInput) > inputSetting.inputDelay) {
            velocity.z = moveSetting.forwardSpeed * forwardInput;
        }
        else
            velocity.z = 0;
    }

    void Turn() {
        if (Mathf.Abs(rotInput) > inputSetting.inputDelay) {
            targetRot *= Quaternion.AngleAxis(moveSetting.rotSpeed * rotInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRot;
    }

    void Jump() {
        if (jumpInput > 0 && Grounded())
        {
            velocity.y = moveSetting.jumpSpeed;
        }
        else if (jumpInput == 0 && Grounded())
        {
            velocity.y = 0;
            
        }
        else {
            velocity.y -= physSetting.downAccel;
        }
    }
}
