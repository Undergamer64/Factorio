using UnityEngine;
using UnityEngine.UI;

public class ProgressCircle : MonoBehaviour
{
    private float _maxTimer = 0f;
    private float _currentTimer;
    private Slider _progressSlider;
    private Factory _parentFactory;
    [SerializeField] private Image _fill;
    [SerializeField] private Gradient _gradient;

    private void Start()
    {
        _parentFactory = transform.GetComponentInParent<Factory>();
        _progressSlider = GetComponent<Slider>();
        if (_parentFactory._Recipe != null)
        {
            _maxTimer = _parentFactory._Recipe._Cooldown;
        }
    }

    private void Update()
    {
        if (_maxTimer == 0f)
        {
            return;
        }
        _currentTimer = _parentFactory._CraftCooldown;
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
