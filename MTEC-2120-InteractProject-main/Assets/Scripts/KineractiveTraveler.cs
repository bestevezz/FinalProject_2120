using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using UnityEngine;
using Kineractive;


public class KineractiveTraveler : PortalTraveller
{
    [SerializeField] float _moveSpeed = 5f;



    public Vector2 pitchMinMax = new Vector2(-40, 85);
    public float rotationSmoothTime = 0.1f;

    public float yaw;
    public float pitch;
    float smoothYaw;
    float smoothPitch;

    float yawSmoothV;
    float pitchSmoothV;
    float verticalVelocity;
    Vector3 velocity;

    private GameObject _mainCamera;



    Vector3 smoothV;

    CharacterController _characterController = null;
    Repositioner _repositioner = null;

    public bool overrideInput = false;

    float longitudinalInput = 0;
    float lateralInput = 0;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _repositioner = GetComponent<Repositioner>();

        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        yaw = transform.eulerAngles.y;
        pitch = _mainCamera.transform.localEulerAngles.x;
        smoothYaw = yaw;
        smoothPitch = pitch;

    }


    void Update()
    {

        if (!overrideInput)
            InputBasedMovement();


        pitch = transform.eulerAngles.y;
        yaw = transform.eulerAngles.x;

        smoothPitch = Mathf.SmoothDampAngle(smoothPitch, pitch, ref pitchSmoothV, rotationSmoothTime);
        smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yawSmoothV, rotationSmoothTime);
    }



    public override void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        Vector3 eulerRot = rot.eulerAngles;
        float delta = Mathf.DeltaAngle(smoothYaw, eulerRot.y);
        yaw += delta;
        smoothYaw += delta;
        transform.eulerAngles = Vector3.up * smoothYaw;
        velocity = toPortal.TransformVector(fromPortal.InverseTransformVector(velocity));
        Physics.SyncTransforms();
    }




    public void InsertVerticalInput(float verticalInput)
    {
        longitudinalInput = verticalInput;
    }

    public void InsertHorizontalInput(float horizontalInput)
    {
        lateralInput = horizontalInput;
    }

    void InputBasedMovement()
    {
        Vector3 moveLongitudinal = transform.forward * longitudinalInput * Time.deltaTime * _moveSpeed;
        Vector3 moveLateral = transform.right * lateralInput * Time.deltaTime * _moveSpeed;
        Vector3 moveVector = moveLongitudinal + moveLateral;

        _characterController.Move(moveVector);
    }

    void EnableInput()
    {
        overrideInput = false;
    }
    void DisableInput()
    {
        overrideInput = true;
    }

    void OnEnable()
    {
        _repositioner.RepoEventStart += DisableInput;
        _repositioner.RepoEventEnd += EnableInput;
    }

    void OnDisable()
    {
        _repositioner.RepoEventStart -= DisableInput;
        _repositioner.RepoEventEnd -= EnableInput;
    }


}
