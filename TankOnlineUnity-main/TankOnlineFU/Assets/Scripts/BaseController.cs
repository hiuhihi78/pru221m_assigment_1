using Assets.Scripts.Constants;
using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
            if(ModeGame.Solo == Constants.modeGameChosen)
            {
				GameObject gameObject1 = gameOverUI.transform.GetChild(2).gameObject;
				TextMeshProUGUI textMeshProUGUI = gameObject1.GetComponent<TextMeshProUGUI>();
				textMeshProUGUI.text = collision.gameObject.CompareTag(TagGameObject.bulletEnemy) && transform.name == "Base"
                    ? "PLAYER 2 WIN" : "PLAYER 1 WIN";
				gameOverUI.SetActive(true);
				Time.timeScale = 0;
                return;
			}
			_spriteRenderer.sprite = baseDestroyedSprite;
            gameOverUI.SetActive(true);
            Time.timeScale = 0;
        }
	}
}
