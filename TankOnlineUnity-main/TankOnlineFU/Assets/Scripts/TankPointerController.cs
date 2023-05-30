using DefaultNamespace;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
			//_boxCollider.isTrigger = true;
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

			if (_lastTimePress + delayPress > Time.time)
			{
				return;
			}
			if (CollideBase(direction))
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
		private bool CollideBase(Direction direction)
		{
			Vector3 endPoint;
			if (direction == Direction.Left)
			{
				endPoint = new Vector3(transform.position.x - 1.3f, transform.position.y, transform.position.z);

			}
			else if (direction == Direction.Up)
			{
				endPoint = new Vector3(transform.position.x, transform.position.y + 1.3f, transform.position.z);
			}
			else if (direction == Direction.Right)
			{
				endPoint = new Vector3(transform.position.x + 1.3f, transform.position.y, transform.position.z);
			}
			else
			{
				endPoint = new Vector3(transform.position.x, transform.position.y - 1.3f, transform.position.z);
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

}
