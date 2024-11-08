using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private CameraScript _camera;

    [SerializeField]
    private CharacterData _characterData;

    private Vector2 _currentMousePosition = Vector2.zero;
    private Vector3 _lastMousePosition = Vector3.zero;
    private GameObject _currentPreviewStructure = null;

    private Quaternion _rotation = Quaternion.Euler(0,0,90);
    private Quaternion _currentRotation = default;

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
        if (context.ReadValue<Vector2>() == Vector2.zero)
        {
            return;
        }
        _currentMousePosition = context.ReadValue<Vector2>();

        StructureItem structureItem = _characterData._PlacedStructureItem;
        if (structureItem == null || structureItem.Structure == null)
        {
            return;
        }

        if (_currentPreviewStructure == null)
        {
            _currentPreviewStructure = TileManager._Instance.Place(structureItem.Structure, structureItem._SizeX, structureItem._SizeY, TileManager._Instance.RoundToCell(_currentMousePosition));
            if (_currentPreviewStructure == null) { return; }
            _currentPreviewStructure.GetComponent<Structure>().enabled = false;
            _currentPreviewStructure.GetComponentInChildren<Collider2D>().enabled = false;
        }

        UpdatePreview();
    }

    public void LeftClickAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StructureItem itemStructure = _characterData._PlacedStructureItem;
            if (_currentMousePosition == Vector2.zero || itemStructure == null) 
            {
                ResetPreview();
                return; 
            }

            if (CheckUIInTheWay())
            {
                ResetPreview();
                return;
            }

            Vector3 mousePos = _camera.GetComponent<Camera>().ScreenToWorldPoint(_currentMousePosition);

            if (!TileManager._Instance.CanPlace(mousePos, itemStructure._SizeX, itemStructure._SizeY))
            {
                ResetPreview();
                return;
            }
            
            if (itemStructure.Structure != null)
            {
                //_characterData._Inventory.TryRemoveItems(itemStructure, 1);
                GameObject structure = TileManager._Instance.Place(itemStructure.Structure, itemStructure._SizeX, itemStructure._SizeY, mousePos, _currentRotation);
                if (structure == null)
                {
                    ResetPreview();
                }
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
                ResetPreview();
                return;
            }

            Vector3 mousePos = _camera.GetComponent<Camera>().ScreenToWorldPoint(_currentMousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos + Vector3.back * 10, _camera.GetComponent<Camera>().transform.forward, 11f);
            if (hit.collider == null)
            {
                ResetPreview();
                return;
            }

            Structure structure = hit.collider.GetComponentInParent<Structure>();
            if (structure == null || structure._Item == null)
            {
                ResetPreview();
                return;
            }

            StructureItem item = structure._Item;
            Destroy(structure.gameObject);

            //_characterData._Inventory.TryAddItems(item);

            ResetPreview();
        }
    }

    public void RotateAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_currentPreviewStructure == null)
            {
                return;
            }

            _currentRotation = Quaternion.Euler(_currentRotation.eulerAngles.x, _currentRotation.eulerAngles.y, 90 + _currentRotation.eulerAngles.z);

            UpdatePreview();
        }
    }

    private void ResetPreview()
    {
        _characterData._PlacedStructureItem = null;
        Destroy(_currentPreviewStructure);
        _currentPreviewStructure = null;
    }

    private void UpdatePreview()
    {
        StructureItem structureItem = _characterData._PlacedStructureItem;
        if (_currentPreviewStructure == null || structureItem == null || structureItem.Structure == null)
        {
            return;
        }
        Vector3 currentMousePositionRounded = TileManager._Instance.RoundToCell(_camera.GetComponent<Camera>().ScreenToWorldPoint(_currentMousePosition));

        Vector3 sizeOffset = new Vector3(structureItem._SizeX / 2f - 0.5f, structureItem._SizeY / 2f - 0.5f, 0);

        sizeOffset = new Vector3(
            sizeOffset.x * Mathf.Cos(-_currentRotation.eulerAngles.z * (2 * Mathf.PI / 360f)) - sizeOffset.y * Mathf.Sin(-_currentRotation.eulerAngles.z * (2 * Mathf.PI / 360f)),
            sizeOffset.x * Mathf.Sin(-_currentRotation.eulerAngles.z * (2 * Mathf.PI / 360f)) + sizeOffset.y * Mathf.Cos(-_currentRotation.eulerAngles.z * (2 * Mathf.PI / 360f)),
            0
        );

        currentMousePositionRounded += sizeOffset + TileManager._Instance._TileOffset;
        if (_lastMousePosition != currentMousePositionRounded)
        {
            _lastMousePosition = currentMousePositionRounded;
        }
        _currentPreviewStructure.transform.rotation = _currentRotation;
        _currentPreviewStructure.transform.position = _lastMousePosition;
    }

    private bool CheckUIInTheWay()
    {
        PointerEventData customEventData = new PointerEventData(EventSystem.current);

        customEventData.position = _currentMousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(customEventData, results);

        if (results.Where(x => x.gameObject.GetComponentInParent<Structure>() == null).Count() > 0)
        {
            return true;
        }
        return false;
    }
}
