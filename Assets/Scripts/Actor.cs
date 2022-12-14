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

	public override void OnActionReceived(ActionBuffers actions)
	{
		// Cuando la Red Neuronal devuelve un output / acción, tomamos el resultado y hacemos que esa acción influya en cómo se comporta nuestro agente.
		// En este caso la ContinousAction[0] es la que se encarga de mover el agrnte.

		Vector3 direction = new Vector3(actions.ContinuousActions[0], 0, 0);

		transform.localPosition += direction * Time.deltaTime * speed;

	}

	public override void CollectObservations(VectorSensor sensor)
	{
		// Le pasamos a la red neuronal todos los inputs / parámetros necesarios para que 'piense'.
		// En este caso le estamos pasando la posición del agente (su propia posición) y a del obstáculo
		sensor.AddObservation(transform.localPosition);
		sensor.AddObservation(obstacle.transform.localPosition);
	}

	public override void OnEpisodeBegin()
	{
		// Código para resetear el escenario
		transform.localPosition = localStartPosition;
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
		// Si el obstáculo golpea al agente le damos una recompensa negativa (lo castigamos) y acabamos el episodio.
		SetReward(-1f);
		EndEpisode();
	}

	IEnumerator RewardCoroutine() {

		// Lo que hacemos aquí es cada cierto tiempo recompensar al agente por mantenerse cerca del punto central.
		while (isActiveAndEnabled)
		{
			yield return new WaitForSeconds(.2f);

			float reward = .05f / (Vector3.Distance(transform.localPosition, localStartPosition) + 1);

			AddReward(reward);
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
