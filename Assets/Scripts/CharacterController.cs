using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private CameraScript _camera;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _velocity;

    [SerializeField]
    private float _speed = 2f;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _rigidbody2D.linearVelocity = _velocity * _speed;
    }

    public void MovementAction(InputAction.CallbackContext callbackContext)
    {
        _velocity = callbackContext.ReadValue<Vector2>();
    }

    public void ZoomAction(InputAction.CallbackContext callbackContext)
    {
        _camera.Zoom(callbackContext.ReadValue<float>());
    }
}
