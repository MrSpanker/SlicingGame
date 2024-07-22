using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YG;

[RequireComponent(typeof(Slider))]
public class SliderComponent : MonoBehaviour
{
    public event UnityAction<float> OnSliderValueChanged;
    public event UnityAction OnProgressComplete;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _changeSpeed = 1f;
    [SerializeField] private float _resetDuration = 1f;

    private Slider _slider;
    private float _targetValue = 0f;
    private float _decreaseRate;
    private Coroutine _sliderCoroutine;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.value = 0f;
        _decreaseRate = 0.03f + YandexGame.savesData.CurrentLevelNumber / 100;
        StartSliderCoroutine();
    }

    public void AddSliderValue(float valueToAdd)
    {
        _targetValue += valueToAdd;
    }

    private void StartSliderCoroutine()
    {
        if (_sliderCoroutine != null)
            StopCoroutine(_sliderCoroutine);

        _sliderCoroutine = StartCoroutine(SliderChangeCoroutine());
    }

    private IEnumerator SliderChangeCoroutine()
    {
        while (true)
        {
            _targetValue -= _decreaseRate * Time.deltaTime;
            _targetValue = Mathf.Clamp(_targetValue, 0f, 1f); // Ограничиваем _targetValue в диапазоне [0, 1]

            float newValue = Mathf.Lerp(_slider.value, _targetValue, _changeSpeed * Time.deltaTime);
            _slider.value = newValue;
            OnSliderValueChanged?.Invoke(newValue);

            if (_slider.value >= 0.95f)
            {
                _audioSource.Play();
                _targetValue = 0f;
                OnProgressComplete?.Invoke();
                break;
            }

            yield return null;
        }
    }

    public void ResetSliderValue()
    {
        StartCoroutine(ResetSliderValueCoroutine());
    }

    private IEnumerator ResetSliderValueCoroutine()
    {
        float elapsedTime = 0f;
        float startValue = _slider.value;

        while (elapsedTime < _resetDuration)
        {
            float newValue = Mathf.Lerp(startValue, 0f, elapsedTime / _resetDuration);
            _slider.value = newValue;
            OnSliderValueChanged?.Invoke(newValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _decreaseRate = 0.03f + YandexGame.savesData.CurrentLevelNumber / 200;
        _slider.value = 0f;
        _targetValue = 0f;
        StartSliderCoroutine();
    }
}
