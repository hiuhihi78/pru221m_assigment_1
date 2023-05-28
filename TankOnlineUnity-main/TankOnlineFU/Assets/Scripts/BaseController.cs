using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public Sprite baseDestroyedSprite;
    SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
