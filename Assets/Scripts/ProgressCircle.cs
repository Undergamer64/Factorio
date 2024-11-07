using UnityEngine;
using UnityEngine.UI;

public class ProgressCircle : MonoBehaviour
{
    private float _maxTimer;
    private float _currentTimer;
    private Slider _progressSlider;

    private void Awake()
    {
        _maxTimer = transform.parent.GetComponentInParent<Factory>;
    }

    private void Update()
    {
        
    }


}
