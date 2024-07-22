using UnityEngine;
using UnityEngine.Events;

namespace Slicer2D
{
    public class FruitSlicerEvent : MonoBehaviour 
	{
		private void Start () 
		{
			Sliceable2D slicer = GetComponent<Sliceable2D>();
			slicer.AddResultEvent(SliceEvent);
		}

        private void SliceEvent(Slice2D slice)
		{
			FruitSlicerGameManager.Instance.OnSliced();

			foreach(GameObject gameObject in slice.GetGameObjects()) 
			{
				Vector3 position = gameObject.transform.position;
				position.z = Random.Range(position.z, 50);
				gameObject.transform.position = position;

				//Rigidbody2D rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
				//rigidbody2d.AddForce(new Vector2(Random.Range(-5, 5), Random.Range(-5, -10)));
				//rigidbody2d.AddTorque(Random.Range(-50, 50));

				//PolygonCollider2D collider = g.GetComponent<PolygonCollider2D>();
				//collider.isTrigger = false;

				//Sliceable2D slicer = gameObject.GetComponent<Sliceable2D>();
				//slicer.enabled = false;

				//slicer.gameObject.AddComponent<FruitSlicerFadeAway>();

				//ColliderLineRenderer2D lineRenderer = g.GetComponent<ColliderLineRenderer2D>();
				//lineRenderer.customColor = true;
				//lineRenderer.color = Color.red;
				//lineRenderer.lineWidth = 0.5f;
			}
		}
	}	
}