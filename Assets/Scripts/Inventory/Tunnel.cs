using UnityEngine;

public abstract class Tunnel : MonoBehaviour
{
    public Transform _Transform;

    private void Awake()
    {
        _Transform = transform;
    }
}
