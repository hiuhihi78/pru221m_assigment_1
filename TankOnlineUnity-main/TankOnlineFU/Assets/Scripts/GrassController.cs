using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassController : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, 5);
		GetComponent<BoxCollider2D>().isTrigger = true;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
