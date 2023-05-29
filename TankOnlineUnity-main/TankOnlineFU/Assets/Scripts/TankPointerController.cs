using DefaultNamespace;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
	public class TankPointerController : TankController
	{
		public float delayPress = 0.5f;

		private float _lastTimePress;
		private Animator _animator;
		private BoxCollider2D _boxCollider;
		private bool isPointer = true;
		protected override void Start()
		{
			base.Start();
			_animator = GetComponent<Animator>();
			_boxCollider = GetComponent<BoxCollider2D>();
			_boxCollider.isTrigger = true;
			_animator.enabled = true;
			Renderer.sortingOrder = 1;
		}

		protected override void Update()
		{
		}

		// Update is called once per frame
		protected void FixedUpdate()
		{
			//Debug.Log("Fix Update: " + Time.fixedDeltaTime);
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				Move(Direction.Left);
			}
			else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				Move(Direction.Down);
			}
			else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				Move(Direction.Right);
			}
			else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			{
				Move(Direction.Up);
			}
			
		}

		protected override void Move(Direction direction)
		{
			if(_lastTimePress + delayPress > Time.time)
			{
				return;
			}
			
			Tank.Position = TankMover.Move(direction, isPointer);
			Tank.Direction = direction;
			//_cameraController.Move(_tank.Position);
			Renderer.sprite = direction switch
			{
				Direction.Down => tankDown,
				Direction.Up => tankUp,
				Direction.Left => tankLeft,
				Direction.Right => tankRight,
				_ => Renderer.sprite
			};
			_lastTimePress = Time.time;
		}
	}
}
