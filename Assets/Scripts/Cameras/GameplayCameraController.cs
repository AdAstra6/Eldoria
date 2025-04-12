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
                
                virtualCam.Follow = player;
                ZoomTo(movePhaseZoom);
                freeCameraRig.transform.position = player.position;
                break;

            case CameraType.FREE:
                virtualCam.Follow = freeCameraRig.transform;

                HandleFreeCamera();
                break;
        }
    }

    void ZoomTo(float targetZoom)
    {
        float currentZoom = mainCam.orthographicSize;
        mainCam.orthographicSize = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSpeed);
    }

    void HandleFreeCamera()
    {

        // Panning with WASD or arrow keys
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        freeCameraRig.transform.position += new Vector3(h, v, 0) * Time.deltaTime * moveSpeed;

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
