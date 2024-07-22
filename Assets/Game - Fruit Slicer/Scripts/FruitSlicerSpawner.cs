using UnityEngine;
using Utilities2D;

namespace Slicer2D
{
    public class FruitSlicerSpawner : MonoBehaviour
    {
        private Pair2D _pair;
        private TimerHelper _timer;
        [SerializeField] private CharacterComponent _characterComponent;

        private float _timerRequired;

        private void Start()
        {
            Polygon2D edge = Polygon2DList.CreateFromGameObject(gameObject)[0];
            _pair = Pair2D.GetList(edge.pointsList)[0];
            _timer = TimerHelper.Create();
            _timerRequired = 1f;
        }

        private void Update()
        {
            if (_timer.Get() > _timerRequired && !_characterComponent.IsCharacterScreenActive)
            {
                GameObject fruit = FruitSlicerGameManager.Instance.GetRandomObjectForSlice();

                fruit.transform.parent = FruitSlicerGameManager.Instance.transform;

                double rotation = Vector2D.Atan2(_pair.B, _pair.A);
                float distance = (float)Vector2D.Distance(_pair.A, _pair.B);

                Vector3 newPosition = _pair.A.ToVector2() + Vector2D.RotToVec(rotation).ToVector2() * Random.Range(0, distance);
                newPosition.z = Random.Range(0, 30);

                fruit.transform.position = newPosition;
                fruit.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

                Rigidbody2D rb = fruit.GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(0, (Random.Range(400, 450))));
                rb.AddTorque(Random.Range(-100, 100));

                _timer = TimerHelper.Create();
                _timerRequired = Random.Range(3f, 4f);
            }
        }
    }
}