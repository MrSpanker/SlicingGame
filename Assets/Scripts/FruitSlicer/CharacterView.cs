using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private Image _characterImage;
    [SerializeField] private SpriteContainer _spriteContainer;
    [SerializeField] private Image _flashScreen;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _rotationAngle = 15f;
    [SerializeField] private float _initialRotationSpeed = 30f;
    [SerializeField] private float _rotationAcceleration = 40f;
    [SerializeField] private float _scaleSpeed = 2f;
    [SerializeField] private float _flashScreenFadeDuration = 0.4f;
    [SerializeField] private Vector2 _targetScale = new(0.8f, 0.8f);
    [SerializeField] private GameObject _button;

    private RectTransform _rectTransform;
    private float _currentRotationSpeed;
    private int _rotationDirection = 1; // 1 - вправо, -1 - влево
    private Vector3 _initialScale;
    private Quaternion _initialRotation;
    private Coroutine _rotationCoroutine;
    private Coroutine _scaleCoroutine;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _initialScale = transform.localScale;
        _initialRotation = transform.rotation;
    }

    private void Start()
    {
        _currentRotationSpeed = _initialRotationSpeed;
        UpdateCharacterView();
        ActiveCoroutines(true);
    }

    public void StartSpriteChangeAnimation()
    {
        ActiveCoroutines(true);
    }

    private void UpdateCharacterView()
    {
        _characterImage.sprite = _spriteContainer.GetSprite(YandexGame.savesData.CurrentLevelNumber);
    }

    private void ActiveCoroutines(bool active)
    {
        if (_rotationCoroutine != null)
            StopCoroutine(_rotationCoroutine);
        if (_scaleCoroutine != null)
            StopCoroutine(_scaleCoroutine);

        ResetParameters();

        if (active)
        {
            _rotationCoroutine = StartCoroutine(RotateObject());
            _scaleCoroutine = StartCoroutine(ScaleObject());
        }
    }

    private void ResetParameters()
    {
        transform.localScale = _initialScale;
        transform.rotation = _initialRotation;
        _currentRotationSpeed = _initialRotationSpeed;
    }

    private IEnumerator RotateObject()
    {
        float currentAngle = _initialScale.z;

        while (true)
        {
            float targetAngle = _initialScale.z + _rotationAngle * _rotationDirection;
            float timeToRotate = Mathf.Abs(_rotationAngle / _currentRotationSpeed);

            float elapsedTime = 0f;
            while (elapsedTime < timeToRotate)
            {
                float newAngle = Mathf.Lerp(currentAngle, targetAngle, elapsedTime / timeToRotate);
                _rectTransform.rotation = Quaternion.Euler(0f, 0f, newAngle);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _rectTransform.rotation = Quaternion.Euler(0f, 0f, targetAngle);

            _rotationDirection *= -1;
            _currentRotationSpeed += _rotationAcceleration;
            currentAngle = targetAngle;
            yield return null;
        }
    }

    private IEnumerator ScaleObject()
    {
        float tolerance = 0.01f;

        while (true)
        {
            _rectTransform.localScale = Vector2.Lerp(_rectTransform.localScale, _targetScale, Time.deltaTime * _scaleSpeed);

            if (Mathf.Abs(_rectTransform.localScale.x - _targetScale.x) < tolerance &&
                Mathf.Abs(_rectTransform.localScale.y - _targetScale.y) < tolerance)
            {
                _rectTransform.localScale = _targetScale;

                _audioSource.Play();
                ActiveCoroutines(false);
                ShowFlashScreen();
                YandexGame.savesData.CurrentLevelNumber++;
                YandexGame.savesData.PlayerScore++;
                YandexGame.NewLeaderboardScores("LB", YandexGame.savesData.PlayerScore);
                YandexGame.SaveProgress();
                UpdateCharacterView();
                StartCoroutine(ShowButtonWithAnimation());

                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator ShowButtonWithAnimation()
    {
        yield return new WaitForSeconds(1f);

        _button.transform.localScale = Vector3.zero;
        _button.SetActive(true);
        _button.transform.DOScale(new Vector3(1f, 1f, 1f), 1f);

        // ¬озможно, добавить задержку перед выполнением следующего кода
        yield return new WaitForSeconds(1f);

        // ≈сли есть какой-то дополнительный код, который должен выполнитьс€ после анимации, добавьте его сюда
    }

    private void ShowFlashScreen()
    {
        Color color = _flashScreen.color;
        color.a = 1f;
        _flashScreen.color = color;
        _flashScreen.DOFade(0f, _flashScreenFadeDuration);
    }
}