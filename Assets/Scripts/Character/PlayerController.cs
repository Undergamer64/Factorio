using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CameraScript _camera;

    [SerializeField]
    private PlayerData _characterData;

    private Vector2 _currentMousePosition = Vector2.zero;
    private Vector3 _lastMousePosition = Vector3.zero;
    private GameObject _currentPreviewStructure = null;

    private Quaternion _rotation = Quaternion.Euler(0,0,-90);
    private Quaternion _currentRotation = default;
    
    private Vector3 _velocity;

    [SerializeField]
    private float _speed = 2f;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += _velocity * (_speed * Time.deltaTime);
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
        _currentMousePosition = context.ReadValue<Vector2>();
        if (_currentMousePosition == Vector2.zero)
        {
            return;
        }

        StructureItem structureItem = _characterData._PlacedStructureItem;
        if (structureItem == null || structureItem.Structure == null)
        {
            return;
        }

        if (_currentPreviewStructure == null)
        {
            _currentPreviewStructure = TileManager._Instance.Place(structureItem.Structure, structureItem._SizeX, structureItem._SizeY, TileManager._Instance.RoundToCell(_currentMousePosition));
            
            if (_currentPreviewStructure == null) { return; }
            
            Structure structure = _currentPreviewStructure.GetComponent<Structure>();
            
            structure.enabled = false;
            
            ProgressCircle progressCircle = _currentPreviewStructure.GetComponentInChildren<ProgressCircle>();
            if (progressCircle != null)
            {
                progressCircle.enabled = false;
            }
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

            if (!TileManager._Instance.CanPlace(mousePos, itemStructure._SizeX, itemStructure._SizeY, _currentRotation))
            {
                ResetPreview();
                return;
            }
            
            if (itemStructure.Structure != null)
            {
                GameObject structure = TileManager._Instance.Place(itemStructure.Structure, itemStructure._SizeX, itemStructure._SizeY, mousePos, _currentRotation);
                
                if (structure == null)
                {
                    ResetPreview();
                }
                else
                {
                    structure.GetComponent<Structure>()._Visuals.transform.localRotation = Quaternion.Euler(_currentRotation.eulerAngles.x, _currentRotation.eulerAngles.y, -_currentRotation.eulerAngles.z);
                    if (structure.GetComponent<Structure>()._Input)
                    {
                        structure.GetComponent<Structure>()._Input._CanConnect = true;
                    }
                    if (structure.GetComponent<Structure>()._Output)
                    {
                        structure.GetComponent<Structure>()._Output._CanConnect = true;
                    }
                }
            }
        }
    }

    public void RightClickAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ResetPreview();
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
            Destroy(structure.gameObject);
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

            _currentRotation = Quaternion.Euler(_currentRotation.eulerAngles.x, _currentRotation.eulerAngles.y, _rotation.eulerAngles.z + _currentRotation.eulerAngles.z);

            UpdatePreview();
        }
    }

    private void ResetPreview()
    {
        _characterData._PlacedStructureItem = null;
        if (_currentPreviewStructure != null)
        {
            Destroy(_currentPreviewStructure);
            _currentPreviewStructure = null;
        }
    }

    private void UpdatePreview()
    {
        StructureItem structureItem = _characterData._PlacedStructureItem;
        if (_currentPreviewStructure == null || structureItem == null || structureItem.Structure == null || _currentMousePosition == Vector2.zero)
        {
            ResetPreview();
            return;
        }
        Vector3 currentMousePositionRounded = TileManager._Instance.RoundToCell(_camera.GetComponent<Camera>().ScreenToWorldPoint(_currentMousePosition));

        Vector3 sizeOffset = new Vector3(structureItem._SizeX / 2f - 0.5f, structureItem._SizeY / 2f - 0.5f, 0);

        sizeOffset = new Vector3(
            (sizeOffset.x * Mathf.Cos(_currentRotation.eulerAngles.z * (2 * Mathf.PI / 360f)) 
             - sizeOffset.y * Mathf.Sin(_currentRotation.eulerAngles.z * (2 * Mathf.PI / 360f))),
            (sizeOffset.x * Mathf.Sin(_currentRotation.eulerAngles.z * (2 * Mathf.PI / 360f)) 
             + sizeOffset.y * Mathf.Cos(_currentRotation.eulerAngles.z * (2 * Mathf.PI / 360f))),
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
