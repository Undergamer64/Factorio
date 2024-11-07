using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private CameraScript _camera;

    [SerializeField]
    private CharacterData _characterData;

    private Vector2 _currentMousePosition;

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
        Move();
    }

    private void Move()
    {
        _rigidbody2D.linearVelocity = _velocity * _speed;
    }

    public void MovementAction(InputAction.CallbackContext context)
    {
        _velocity = context.ReadValue<Vector2>();
    }

    public void ZoomAction(InputAction.CallbackContext context)
    {
        _camera.Zoom(context.ReadValue<float>());
    }

    public void MouseMovementAction(InputAction.CallbackContext context)
    {
        Vector2 oldPos = _currentMousePosition;
        _currentMousePosition = context.ReadValue<Vector2>();

        StructureItem structure = _characterData._PlacedStructureItem;
        if (structure == null)
        {
            return;
        }

        Vector2 structurePos = _currentMousePosition;

        if (_currentMousePosition == Vector2.zero)
        {
            if (oldPos ==  Vector2.zero)
            { 
                return; 
            }
            structurePos = oldPos;
        }
        

    }

    public void LeftClickAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StructureItem itemStructure = _characterData._PlacedStructureItem;
            if (_currentMousePosition == Vector2.zero || itemStructure == null) { return; }

            if (CheckUIInTheWay())
            {
                return;
            }

            Vector3 mousePos = _camera.GetComponent<Camera>().ScreenToWorldPoint(_currentMousePosition);

            RaycastHit2D hit2D = Physics2D.Raycast(mousePos, _camera.GetComponent<Camera>().transform.forward, 1f);
            if (hit2D.collider != null || !TileManager._Instance.CanPlace(mousePos, itemStructure._SizeX, itemStructure._SizeY))
            {
                return;
            }
            
            if (itemStructure.Structure != null)
            {
                //_characterData._Inventory.TryRemoveItems(itemStructure, 1);
                TileManager._Instance.Place(itemStructure.Structure, itemStructure._SizeX, itemStructure._SizeY, mousePos);
            }
        }
    }

    public void RigthClickAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_currentMousePosition == Vector2.zero) { return; }
            
            if (CheckUIInTheWay())
            {
                return;
            }

            Vector3 mousePos = _camera.GetComponent<Camera>().ScreenToWorldPoint(_currentMousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos + Vector3.back * 10, _camera.GetComponent<Camera>().transform.forward, 11f);
            if (hit.collider == null)
            {
                return;
            }

            Structure structure = hit.collider.GetComponentInParent<Structure>();
            if (structure == null || structure._Item == null)
            {
                return;
            }

            StructureItem item = structure._Item;
            Destroy(structure.gameObject);

            //_characterData._Inventory.TryAddItems(item);
        }
    }

    private bool CheckUIInTheWay()
    {
        PointerEventData customEventData = new PointerEventData(EventSystem.current);

        customEventData.position = _currentMousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(customEventData, results);

        if (results.Count > 0)
        {
            return true;
        }
        return false;
    }
}
