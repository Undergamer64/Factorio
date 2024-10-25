using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerCharacter;

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

    public void ZoomAction(InputAction.CallbackContext callbackContext)
    {
        float scrollValue = callbackContext.ReadValue<float>();
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - scrollValue * _zoomForce, _minZoomValue, _maxZoomValue);
    }
}
