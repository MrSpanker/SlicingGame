using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using YG;

public class CharacterComponent : MonoBehaviour
{
    public bool IsCharacterScreenActive => _isCharacterScreenActive;

    [SerializeField] private GameObject _characterScreen;
    [SerializeField] private SliderComponent _sliderComponent;
    [SerializeField] private CharacterView _characterView;
    [SerializeField] private float _scaleDuration = 1f;
    [SerializeField] private GameObject _button;

    private bool _isCharacterScreenActive = false;
    private Vector3 _initCharacterScreenScale;
    private Coroutine _scalingCoroutine;

    private void Start()
    {
        _initCharacterScreenScale = _characterScreen.transform.localScale;
        _characterScreen.SetActive(false);
    }

    private void OnEnable()
    {
        _sliderComponent.OnProgressComplete += GoToTheNextLevel;
    }

    private void OnDisable()
    {
        _sliderComponent.OnProgressComplete -= GoToTheNextLevel;
    }

    private void GoToTheNextLevel()
    {
        _isCharacterScreenActive = true;
        ShowScreen(_characterScreen, _initCharacterScreenScale);
        _characterView.StartSpriteChangeAnimation();
    }

    private void ShowScreen(GameObject gameObject, Vector3 endScale)
    {
        gameObject.transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        gameObject.transform.DOScale(endScale, _scaleDuration);
    }

    public void HideCharacterScreen()
    {
        if (_scalingCoroutine != null)
            StopCoroutine(_scalingCoroutine);
        _scalingCoroutine = StartCoroutine(ChangeScaleAndActive(_characterScreen, Vector3.zero, false));
        _isCharacterScreenActive = false;
    }

    private IEnumerator ChangeScaleAndActive(GameObject gameObject, Vector3 targetScale, bool active)
    {
        Vector3 initialScale = gameObject.transform.localScale;
        float timeElapsed = 0f;

        while (timeElapsed < _scaleDuration)
        {
            gameObject.transform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / _scaleDuration);
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        gameObject.transform.localScale = targetScale;
        gameObject.SetActive(active);
        _button.SetActive(active);
        _scalingCoroutine = null;
    }
}
