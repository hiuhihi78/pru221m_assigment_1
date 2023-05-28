using System;
using System.Collections;
using System.Collections.Generic;
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
		if (isTankPointer &&
			(currentPos.y > topWrap || currentPos.y < bottomWrap
			||
			currentPos.x > rightWrap || currentPos.x < leftWrap))
		{
			return previousPos;
		}
		gameObject.transform.position = currentPos;
		return currentPos;
	}
}