using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
	[SerializeField] float speed;



	public void MoveToTargetInstant(Vector3 target)
	{
		transform.position = new Vector3(target.x, target.y, -10);
	}

	public void MoveToTarget(Vector3 target) 
	{
		transform.position = Vector3.Lerp(transform.position, new Vector3(target.x, target.y, -10), Time.deltaTime * speed);
	}
}
