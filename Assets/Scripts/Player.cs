using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public delegate void TriggerEnterEvent(GameObject other);
	public event TriggerEnterEvent triggerEnter;

	[SerializeField] Transform sprite;
	[SerializeField] float speed;

	bool isMove;



	public bool IsMove()
	{
		return isMove;
	}

	public Vector2Int GetPos()
	{
		return new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
	}

	public void SetPos(Vector2Int coord)
	{
		transform.position = new Vector3(coord.x, coord.y, 0);
	}

	public void RotateByDir(Vector2Int dir) 
	{
		float degree = 0;

		if (dir == new Vector2Int(-1, 0)) degree = 270;
		else if (dir == new Vector2Int(1, 0)) degree = 90;
		else if (dir == new Vector2Int(0, -1)) degree = 0;
		else if (dir == new Vector2Int(0, 1)) degree = 180;

		sprite.rotation = Quaternion.Euler(0, 0, degree);
	}

	public void MoveToTarget(Vector3 targetPos) 
	{
		IEnumerator MoveToTargetCo(Vector3 targetPos)
		{
			isMove = true;
			while (transform.position != targetPos)
			{
				yield return null;
				transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
			}
			isMove = false;
		}

		StartCoroutine(MoveToTargetCo(targetPos));
	}


	void OnTriggerEnter2D(Collider2D collision)
	{
		 triggerEnter?.Invoke(collision.gameObject);
	}
}
