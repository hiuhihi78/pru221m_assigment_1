using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WallController : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{

	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (gameObject.tag == TagGameObject.wallBrick &&
			(collision.gameObject.CompareTag(TagGameObject.bullet) || collision.gameObject.CompareTag(TagGameObject.bulletEnemy)))
		{
			Destroy(gameObject);
		}
	}
}
