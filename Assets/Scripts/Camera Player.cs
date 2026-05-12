using UnityEngine;
using System.Collections.Generic;

public class CameraPlayer : MonoBehaviour
{
    // ---- gizmo/pos ----
    public List<Transform> transforms;
    public int currentPosition = 0;

    // ---- mouse movement ----
    public float mouseInfluence = 25f;
    private float smoothSpeed = 5f;
    private Quaternion targetRotation;
    private bool justChangedPosition = false;

    // ---- zoom ----
    public float zoomSpeed = 5f;
    public float zoomMax = 20f;
    private float zoomSmooth = 8f;
    private float zoomOffset = 0f;
    private float targetZoomOffset = 0f;

    // ---- grab when zoomed ----
    public float grabSensitivity = 1.5f;
    private bool isGrabbing = false;
    private Vector3 grabStartMousePos;
    private Quaternion grabStartRotation;

    // ---- view limit ----
    public float pitchMin = -60f; 
    public float pitchMax = 60f; 
    public float minHeight = 1f;


    void Start()
    {
        SnapToCurrentPosition();
    }

    void Update()
    {
        justChangedPosition = false;

        HandleInputs();

        if (justChangedPosition) return;

        HandleZoom();

        bool isZoomed = Mathf.Abs(zoomOffset) > 0.01f;

        if (!isZoomed)
        {
            HandleMouseLook();
        }
        else
        {
            HandleGrab();
        }

        transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,Time.deltaTime * smoothSpeed);
    }

    void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentPosition < transforms.Count - 1) {
                currentPosition++;
                justChangedPosition = true; 
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentPosition > 0) { 
                currentPosition--;
                justChangedPosition = true;
            }
        }

        if (justChangedPosition) SnapToCurrentPosition();

    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        targetZoomOffset = Mathf.Clamp(targetZoomOffset - scroll * zoomSpeed, -zoomMax, 0f); 
        zoomOffset = Mathf.Lerp(zoomOffset, targetZoomOffset, Time.deltaTime * zoomSmooth);

        Vector3 basePosition = transforms[currentPosition].position;
        Vector3 newPosition;

        newPosition = basePosition + transform.forward * (-zoomOffset);

        newPosition.y = Mathf.Max(newPosition.y, minHeight);
        transform.position = newPosition;
    }

    void HandleGrab()
    {
        if (Input.GetMouseButtonDown(0))
        {
           
            isGrabbing = true;
            grabStartMousePos = Input.mousePosition;
            grabStartRotation = targetRotation;

            Vector3 euler = grabStartRotation.eulerAngles;
        }

        if (Input.GetMouseButtonUp(0)) isGrabbing = false;

        if (isGrabbing)
        {
            Vector3 delta = Input.mousePosition - grabStartMousePos;

            float grabX = (delta.x / Screen.width) * grabSensitivity * 90f;
            float grabY = (delta.y / Screen.height) * grabSensitivity * 90f;

            float startPitch;
            if (grabStartRotation.eulerAngles.x > 180f) startPitch = grabStartRotation.eulerAngles.x - 360f;
            else  startPitch = grabStartRotation.eulerAngles.x;

            float newPitch = Mathf.Clamp(startPitch - grabY,pitchMin,pitchMax);

            targetRotation = Quaternion.Euler(newPitch, grabStartRotation.eulerAngles.y + grabX, 0f);
        }
    }

    void HandleMouseLook()
    {
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        float mouseX = (Input.mousePosition.x - screenCenter.x) / screenCenter.x;
        float mouseY = (Input.mousePosition.y - screenCenter.y) / screenCenter.y;

        Quaternion baseRotation = transforms[currentPosition].rotation;

        float basePitch;
        if (baseRotation.eulerAngles.x > 180f)  basePitch = baseRotation.eulerAngles.x - 360f;
        else basePitch = baseRotation.eulerAngles.x;

        float targetPitch = Mathf.Clamp(basePitch - mouseY * mouseInfluence, pitchMin, pitchMax);
        float targetYaw = baseRotation.eulerAngles.y + mouseX * mouseInfluence;

        targetRotation = Quaternion.Euler(targetPitch, targetYaw, 0f);
    }

    void SnapToCurrentPosition()
    {
        transform.SetPositionAndRotation(transforms[currentPosition].position,transforms[currentPosition].rotation);
        targetRotation = transforms[currentPosition].rotation;
        zoomOffset = 0f;
        targetZoomOffset = 0f;
        isGrabbing = false;
    }
}