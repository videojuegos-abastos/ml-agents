using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{

	
	[Header("Obstacle Properties")]

	[SerializeField]
	GameObject objectToMove;

	[SerializeField]
	[Range(.2f, 5f)]
	float speed;


	[Header("Key Positions")]

	[SerializeField]
	Transform startTransform;

	[SerializeField]
	Transform endTransform;


	void Update()
	{
		// Movemos el objeto
		objectToMove.transform.position += Vector3.forward * Time.deltaTime * speed;

		// Si se sale, lo reseteamos
		if (objectToMove.transform.position.z > endTransform.position.z)
		{
			OnExitFloor();
		}

	}


	void OnExitFloor()
	{
		objectToMove.transform.position = startTransform.position;
	}
}
