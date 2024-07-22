using Slicer2D;
using UnityEngine;
using YG;
using DG.Tweening;

public class Trainer : MonoBehaviour
{
    [SerializeField] private float _timeBetweenChecks = 6f;
    [SerializeField] private int _childObjectsCount;
    [SerializeField] private Transform _finger;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private FruitSlicerSpawner _fruitSlicerSpawner;
    [SerializeField] private FruitSlicerEvent2 _fruitSlicerEvent;

    private float _elapsed;
    private bool _isSliced = false;

    private void OnEnable()
    {
        _fruitSlicerEvent.OnSliced += ChangeHandTrajectory;
    }
    
    private void OnDisable()
    {
        _fruitSlicerEvent.OnSliced -= ChangeHandTrajectory;
    }

    private void ChangeHandTrajectory()
    {
        _finger.gameObject.SetActive(false);
        _isSliced = true;
    }

    private void Start()
    {
        if (YandexGame.savesData.CurrentLevelNumber == 0)
        {
            _fruitSlicerSpawner.enabled = false;
            _finger.DOMove(_targetPoint.position, 1f).SetLoops(-1, LoopType.Restart);
        }

        else
        {
            _fruitSlicerSpawner.enabled = true;
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!_isSliced) return;

        _elapsed += Time.deltaTime;

        if (_elapsed > _timeBetweenChecks /*&& transform.childCount == _childObjectsCount*/)
        {
            _fruitSlicerSpawner.enabled = true;
            Destroy(gameObject, 0.1f);
        }
    }
}