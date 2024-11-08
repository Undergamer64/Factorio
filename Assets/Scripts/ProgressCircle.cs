using UnityEngine;
using UnityEngine.UI;

public class ProgressCircle : MonoBehaviour
{
    private float _maxTimer;
    private float _currentTimer;
    private Slider _progressSlider;
    private Factory _parentFactory;
    [SerializeField] private Image _fill;
    [SerializeField] private Gradient _gradient;

    private void Start()
    {

        _parentFactory = transform.parent.parent.GetComponent<Factory>();
        _maxTimer = _parentFactory._Recipe._Cooldown;
        _progressSlider = GetComponent<Slider>();
        
    }

    private void Update()
    {
        _currentTimer = _parentFactory._craftCooldown;
        float ratio = _currentTimer / _maxTimer;
        if (_parentFactory._CanCraft)
        {
            _progressSlider.value = 1 - ratio;
            _fill.color = _gradient.Evaluate(ratio);
        }
        else
        {
            _progressSlider.value = 0;
            _fill.color = Color.red;
        }
    }


}
