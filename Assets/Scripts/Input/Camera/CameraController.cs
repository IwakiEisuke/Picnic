using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// カメラを操作するためのクラス
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] CameraInputManager cameraInputManager;
    [SerializeField] CinemachineCamera cineCamera;
    [SerializeField] LayerMask terrainLayer;
    [SerializeField] Vector3 horizontalPosition;
    [SerializeField] Vector3 rotate;
    [SerializeField] float distance;
    [SerializeField] float speed = 5f;
    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] float minDistance = 5f;
    [SerializeField] float maxDistance = 100f;

    Vector2 moveInput;
    float zoomInput;

    CinemachinePositionComposer positionComposer;

    private void Start()
    {
        cameraInputManager.OnCameraMove += input => moveInput = input;
        cameraInputManager.OnCameraZoom += input => zoomInput = input;

        positionComposer = cineCamera.GetComponent<CinemachinePositionComposer>();
    }

    private void Update()
    {
        var dt = Time.deltaTime;
        horizontalPosition += speed * dt * (Quaternion.Euler(0, rotate.y, 0) * new Vector3(moveInput.x, 0, moveInput.y));
        distance = Mathf.Clamp(distance - zoomInput * zoomSpeed * dt, minDistance, maxDistance);

        transform.rotation = Quaternion.Euler(rotate);
        if (Physics.Raycast(horizontalPosition + Vector3.up * 100, Vector3.down, out var hit, 100f, terrainLayer.value))
        {
            transform.position = hit.point - transform.forward * distance;
        }
        else
        {
            transform.position = horizontalPosition - transform.forward * distance;
        }

        if (positionComposer != null)
        {
            positionComposer.CameraDistance = distance;
        }
    }
}
