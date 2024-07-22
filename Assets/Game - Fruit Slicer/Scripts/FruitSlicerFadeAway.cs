using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSlicerFadeAway : MonoBehaviour 
{
	private MeshRenderer _meshRenderer;
	private float _fadeTime = 2f;

    private void Start() 
	{
		_meshRenderer = GetComponent<MeshRenderer>();
	}

    private void Update() 
	{
		//Color color = _meshRenderer.sharedMaterial.color;

		//if (color.a < 0.01f) 
		//{
		//	Destroy(gameObject);
		//}

		//_meshRenderer.sharedMaterial.color = Color.Lerp(color, new Color(1, 1, 1, 0), Time.deltaTime);
	}
}
