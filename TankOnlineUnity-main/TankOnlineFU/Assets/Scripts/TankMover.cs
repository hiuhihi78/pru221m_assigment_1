using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entity;
using UnityEngine;

public class TankMover : MonoBehaviour
{
	// Start is called before the first frame update

	public float speed = 1;
	public float topWrap;
	public float bottomWrap;
	public float leftWrap;
	public float rightWrap;
	void Start()
	{
		//speed = 1;
	}

	// Update is called once per frame
	void Update()
	{
	}


	public Vector3 Move(Direction direction, bool isTankPointer = false)
	{
		var currentPos = gameObject.transform.position;
		var previousPos = gameObject.transform.position;
		switch (direction)
		{
			case Direction.Down:
				currentPos.y -= !isTankPointer ? speed * Time.deltaTime : speed;
				break;
			case Direction.Left:
				currentPos.x -= !isTankPointer ? speed * Time.deltaTime : speed;
				break;
			case Direction.Right:
				currentPos.x += !isTankPointer ? speed * Time.deltaTime : speed;
				break;
			case Direction.Up:
				currentPos.y += !isTankPointer ? speed * Time.deltaTime : speed;
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
		}
		if (isTankPointer && CollideBase(direction))
		{
			return previousPos;
		}
		gameObject.transform.position = currentPos;
		return currentPos;
	}
	private bool CollideBase(Direction direction)
	{
		Vector3 endPoint;
		switch (direction)
		{
			case Direction.Down:
				endPoint = new Vector3(transform.position.x, transform.position.y - 1.3f, transform.position.z);
				break;
			case Direction.Left:
				endPoint = new Vector3(transform.position.x - 1.3f, transform.position.y, transform.position.z);
				break;
			case Direction.Right:
				endPoint = new Vector3(transform.position.x + 1.3f, transform.position.y, transform.position.z);
				break;
			case Direction.Up:
				endPoint = new Vector3(transform.position.x, transform.position.y + 1.3f, transform.position.z);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
		}
		RaycastHit2D[] ray = Physics2D.LinecastAll(transform.position, endPoint);
		if (ray.Length > 1)
		{
			GameObject baseGameObject = ray.Where(x => x.collider.gameObject.tag == TagGameObject.basePlayer
				|| x.collider.gameObject.tag == TagGameObject.wallWrap
			)
				.Select(x => x.transform.gameObject)
				.FirstOrDefault();
			if (baseGameObject != null && baseGameObject.tag == TagGameObject.basePlayer)
			{
				return true;
			}
			else if (baseGameObject != null && baseGameObject.tag == TagGameObject.wallWrap)
			{
				return true;
			}
		}
		Debug.DrawLine(transform.position, endPoint);
		return false;
	}
}