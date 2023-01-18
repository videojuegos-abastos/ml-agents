using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Actor : Agent
{
	[SerializeField]
	[Range(1f, 5f)]
	float speed;

	[SerializeField]
	GameObject obstacle;

	[SerializeField]
	Color colorGood;
	[SerializeField]
	Color colorBad;

	Vector3 localStartPosition;

	void Start()
	{
		localStartPosition = transform.localPosition;

		StartCoroutine(nameof(RewardCoroutine));
	}

	public override void OnEpisodeBegin()
	{
		// Código para resetear el escenario
		transform.localPosition = localStartPosition;
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		// #BLOQUE
		// Podemos añadir observaciones / inputs con la función: sensor.AddObservation
		// Esta función puede tomar varios tipos de argumentos pero recuerda que no todos añadiran el mismo número de observaciones



	}

	public override void OnActionReceived(ActionBuffers actions)
	{
		// #BLOQUE
		// Movemos al agente en el eje X.
		// Recuerda que tenemos las acciones en: actions.ContinuousActions



	}


	public override void Heuristic(in ActionBuffers actionsOut)
	{
		// Si queremos probar que funcioen las salidas, podemos sobreescribir el output de la red.
		// *En los behaviour parameters, tendremos que ponerle que utilice la heurística.
		ActionSegment<float> continousActions = actionsOut.ContinuousActions;

		continousActions[0] = Input.GetAxis("Horizontal");
	}

	void OnTriggerEnter(Collider other)
	{
		// #BLOQUE
		// El agente entra en contacto con algún trigger



	}

	IEnumerator RewardCoroutine() {

		// Lo que hacemos aquí es cada cierto tiempo recompensar al agente por mantenerse cerca del punto central.
		while (isActiveAndEnabled)
		{
			yield return new WaitForSeconds(.2f);
			
			// #BLOQUE
			// Este bloque se ejecuta cada 0.2 segundos, aquí podemos recompensar al agente dependiendo de qué tal lo haga
			{




			}

			SetColor();
		}

	}

	void SetColor()
	{
		// Para visualizar mejor el resultado, cambiamos el color del agente para ver si lo está haciendo bien o mal
		MeshRenderer mr = GetComponent<MeshRenderer>();
		mr.material.color = Color.Lerp(colorBad, colorGood, GetCumulativeReward());
	}

}
