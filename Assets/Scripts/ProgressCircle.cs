using UnityEngine;
using UnityEngine.UI;

public class ProgressCircle : MonoBehaviour
{
    private float _maxTimer;
    private float _currentTimer;
    private Slider _progressSlider;
    private Factory _parentFactory;

    private void Awake()
    {
        _parentFactory = transform.parent.parent.GetComponent<Factory>();
        _maxTimer = _parentFactory._Recipe._Cooldown;
        _progressSlider = GetComponent<Slider>();
        
    }

    private void Update()
    {
        _currentTimer = _parentFactory._craftCooldown;
        if (_parentFactory._CanCraft)
        {
            _progressSlider.value = 1 - _currentTimer / _maxTimer;
        }
        else
        {
            _progressSlider.value = 0;
        }
    }


}
