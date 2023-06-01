using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public Sprite baseDestroyedSprite;
    public GameObject gameOverUI;
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
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag(TagGameObject.bullet) 
            || collision.gameObject.CompareTag(TagGameObject.bulletEnemy))
        {
			_spriteRenderer.sprite = baseDestroyedSprite;
            gameOverUI.SetActive(true);
            Time.timeScale = 0;
        }
	}
}
