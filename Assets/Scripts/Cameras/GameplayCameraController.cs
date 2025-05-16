using UnityEngine;
using Cinemachine;

public enum CameraType { FIXED , FREE }

public class GameplayCameraController : MonoBehaviour
{
    [SerializeField] public CinemachineVirtualCamera virtualCam;
    public Transform player;

    public float movePhaseZoom = 8f;
    public float zoomSpeed = 8f;
    public float strategyZoomMin = 5f;
    public float strategyZoomMax = 20f;
    public float moveSpeed = 5f;
    public float mouseDragSpeed = 0.8f;
    [SerializeField] public Texture2D dragCursor;
    public Vector2 cursorHotspot = Vector2.zero;


    public CameraType currentPhase;

    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject freeCameraRig;

    void Start()
    {
        mainCam = Camera.main;

        // Ensure the virtual cam is assigned
        if (virtualCam != null)
        {
            virtualCam.Follow = player;
        }
        
    }

    void Update()
    {
        switch (currentPhase)
        {
            case CameraType.FIXED:
                freeCameraRig.transform.position = player.position;
                break;
            case CameraType.FREE:
                HandleFreeCamera();
                break;
        }
    }

    void ZoomTo(float targetZoom, bool instant = false)
    {
        if (instant)
            virtualCam.m_Lens.OrthographicSize = targetZoom;
        else
        {
            float currentZoom = virtualCam.m_Lens.OrthographicSize;
            virtualCam.m_Lens.OrthographicSize = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSpeed);
        }
    }

    void HandleFreeCamera()
    {
        // Panning with WASD or arrow keys
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 keyboardMovement = new Vector3(h, v, 0) * Time.deltaTime * moveSpeed;

        // Mouse drag panning
        Vector3 mouseDragMovement = Vector3.zero;
        if (Input.GetMouseButton(1)) // Right mouse button held
        {
            Cursor.SetCursor(dragCursor, cursorHotspot, CursorMode.Auto);
            float dragX = -Input.GetAxis("Mouse X") * mouseDragSpeed;
            float dragY = -Input.GetAxis("Mouse Y") * mouseDragSpeed;
            mouseDragMovement = new Vector3(dragX, dragY, 0);
        }
        else
        {
            //Cursor.SetCursor(null, cursorHotspot, CursorMode.Auto);
            CursorChanger.SetDefaultCursor();
        }
        // Apply combined movement
        freeCameraRig.transform.position += keyboardMovement + mouseDragMovement;

        // Zooming with scroll
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        virtualCam.m_Lens.OrthographicSize = Mathf.Clamp(
            virtualCam.m_Lens.OrthographicSize - scroll * zoomSpeed,
            strategyZoomMin,
            strategyZoomMax
        );

        Debug.Log("H: " + h + " | V: " + v + " | Scroll: " + scroll);
    }


    public void SetType(CameraType type)
    {
        currentPhase = type;
        if (type == CameraType.FIXED)
        {
            ZoomTo(movePhaseZoom, true); 
            virtualCam.Follow = player;
        }
        else if (type == CameraType.FREE)
        {
            virtualCam.Follow = freeCameraRig.transform;
        }
    }
    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
        if (virtualCam != null)
        {
            virtualCam.Follow = player;
        }
    }
}
