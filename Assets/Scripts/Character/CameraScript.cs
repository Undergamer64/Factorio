using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private float _minZoomValue = 5, _maxZoomValue = 13;

    [SerializeField]
    private float _zoomForce = 0.5f;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        if (Camera.main != null)
        {
            Camera.main.gameObject.SetActive(false);
            _camera.tag = "MainCamera";
        }
    }

    /// <summary>
    /// Changes the camera orthographicSize between the minZoomValue and the maxZoomValue
    /// </summary>
    public void Zoom(float scrollValue)
    {
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - scrollValue * _zoomForce, _minZoomValue, _maxZoomValue);
    }
}
