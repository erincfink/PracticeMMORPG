﻿using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;

    [System.Serializable]
    public class PositionSettings {
        public Vector3 targetPosOffset = new Vector3(0, 1.4f, 0);
        public float lookSmooth = 100f;
        public float distanceFromTarget = -8;
        public float zoomSmooth = 120;
        public float maxZoom = -2;
        public float minZoom = -15;
    }

    [System.Serializable]
    public class OrbitSettings {
        public float xRotation = -20;
        public float yRotation = -180;
        public float maxXRotation = 25;
        public float minXRotation = -85;
        public float vOrbitSmooth = 150;
        public float hOrbitSmooth = 150;
    }

    [System.Serializable]
    public class InputSettings {
        public string ORBIT_HORIZONTAL_SNAP = "OrbitHorizontalSnap";
        public string ORBIT_HORIZONTAL = "OrbitHorizontal";
        public string ORBIT_VERTICAL = "OrbitVertical";
        public string ZOOM = "Mouse ScrollWheel";
    }

    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();

    private Vector3 targetPos = Vector3.zero;
    private Vector3 destination = Vector3.zero;
    private CharController charCtrl;
    private float vOrbitInput, hOrbitInput, zoomInput, hOrbitSnapInput;

	// Use this for initialization
	void Start () {
        SetCameraTarget(target);
        targetPos = target.position + position.targetPosOffset;
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * position.distanceFromTarget;
        destination += targetPos;
        transform.position = destination;
    }

    void SetCameraTarget(Transform t) {
        target = t;

        if (target != null)
        {
            if (target.GetComponent<CharController>()) {
                charCtrl = target.GetComponent<CharController>();
            }
        }
        else
            Debug.LogError("Your Camera needs a target.");
    }

    void GetInput() {
        vOrbitInput = Input.GetAxisRaw(input.ORBIT_VERTICAL);
        hOrbitInput = Input.GetAxisRaw(input.ORBIT_HORIZONTAL);
        hOrbitSnapInput = Input.GetAxisRaw(input.ORBIT_HORIZONTAL_SNAP);
        zoomInput = Input.GetAxisRaw(input.ZOOM);
    }

    void Update() {
        GetInput();
        OrbitTarget();
        ZoomInOnTarget();
    }


    void LateUpdate() {
        // Moving
        MoveToTarget();

        // Rotating
        LookAtTarget();
    }

    void MoveToTarget() {
        targetPos = target.position + position.targetPosOffset;
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * position.distanceFromTarget;
        destination += targetPos;
        transform.position = destination;
    }

    void LookAtTarget() {
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, position.lookSmooth * Time.deltaTime);
    }

    void OrbitTarget() {
        if (hOrbitSnapInput > 0) {
            orbit.yRotation = -180;
        }

        orbit.xRotation += -vOrbitInput * orbit.vOrbitSmooth * Time.deltaTime;
        orbit.yRotation += -hOrbitInput * orbit.vOrbitSmooth * Time.deltaTime;

        if (orbit.xRotation > orbit.maxXRotation) {
            orbit.xRotation = orbit.maxXRotation;
        }
        if (orbit.xRotation < orbit.minXRotation)
        {
            orbit.xRotation = orbit.minXRotation;
        }
    }

    void ZoomInOnTarget() {
        position.distanceFromTarget += zoomInput * position.zoomSmooth * Time.deltaTime;

        if (position.distanceFromTarget > position.maxZoom) {
            position.distanceFromTarget = position.maxZoom;
        }
        if (position.distanceFromTarget < position.minZoom) {
            position.distanceFromTarget = position.minZoom;
        }
    }

}





