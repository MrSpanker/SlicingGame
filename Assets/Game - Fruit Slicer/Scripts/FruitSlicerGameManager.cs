using System.Collections.Generic;
using UnityEngine;
using Utilities2D;
using YG;

namespace Slicer2D
{
    public class FruitSlicerGameManager : MonoBehaviour
    {
        public static FruitSlicerGameManager Instance;

        [SerializeField] private SliderComponent _sliderComponent;
        [SerializeField] private List<GameObject> _objectsForSlicing;
        [SerializeField] private float _addedScore;

        private void Start()
        {
            Instance = this;
            Physics2D.gravity = new Vector2(0, -5);
        }

        private void Update()
        {
            Polygon2D cameraPolygon = Polygon2D.CreateFromCamera(Camera.main);
            cameraPolygon = cameraPolygon.ToRotation(Camera.main.transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
            cameraPolygon = cameraPolygon.ToOffset(new Vector2D(Camera.main.transform.position));

            foreach (Sliceable2D slicer in Sliceable2D.GetListCopy())
            {
                if (Math2D.PolyCollidePoly(slicer.shape.GetWorld(), cameraPolygon) == false)
                {
                    if (slicer.enabled == true)
                    {
                        //_lives--;
                        //if (Lives >= 0) 
                        //{
                        //	SpriteRenderer sr = LivesObjects[Lives].GetComponent<SpriteRenderer>();
                        //	sr.color = Color.white;
                        //} else {
                        //	Debug.Log("lose");
                        //}
                    }
                    Destroy(slicer.gameObject);
                }
            }
        }

        public GameObject GetRandomObjectForSlice()
        {
            return Instantiate(_objectsForSlicing[Random.Range(0, _objectsForSlicing.Count)]);
        }

        public void OnSliced()
        {
            _sliderComponent.AddSliderValue(_addedScore - 0.0001f * YandexGame.savesData.CurrentLevelNumber);
        }
    }
}