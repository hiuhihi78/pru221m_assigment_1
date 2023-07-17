using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Constants;
using Assets.Scripts.Entity;
using Entity;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Bullet Bullet { get; set; }

    public int MaxRange { get; set; }

    public GameObject BulletExplosionPrefabs { get; set; }   

    public GameObject BigExplosionPrefabs { get; set; }

    public bool isTankPlayer;


    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        DestroyAfterRange();
    }

    private void DestroyAfterRange()
    {
        var currentPos = gameObject.transform.position;
        var initPos = Bullet.InitialPosition;
        switch (Bullet.Direction)
        {
            case Direction.Down:
                if (initPos.y - MaxRange >= currentPos.y)
                {
                    Destroy(gameObject);
                }

                break;
            case Direction.Up:
                if (initPos.y + MaxRange <= currentPos.y)
                {
                    Destroy(gameObject);
                }

                break;
            case Direction.Left:
                if (initPos.x - MaxRange >= currentPos.x)
                {
                    Destroy(gameObject);
                }

                break;
            case Direction.Right:
                if (initPos.x + MaxRange <= currentPos.x)
                {
                    Destroy(gameObject);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Vector3 positionImpacted = this.transform.position;
        var collisionTag = collision.gameObject.tag;


        switch (collisionTag)
        {
            case TagGameObject.wallWrap:
                Destroy(this.gameObject);
                BulletExplosion(positionImpacted);
                break;
            case TagGameObject.wallSteel:
                Destroy(this.gameObject);
                BulletExplosion(positionImpacted);
                break;
            case TagGameObject.wallBrick:
                Destroy(this.gameObject);
                BulletExplosion(positionImpacted);
                Destroy(collision.gameObject);
                break;
        }
		if (ModeGame.Solo == Constants.modeGameChosen)
		{
			if (collision.gameObject.CompareTag("Player") && collision.gameObject.name == "Tank" && transform.tag == "bulletEnemy")
			{
				Destroy(collision.gameObject);
			}
			else if (collision.gameObject.CompareTag("Player") && collision.gameObject.name == "Tank2" && transform.tag == "bullet")
			{
				Destroy(collision.gameObject);
			}
			return;
		}
		if (isTankPlayer) 
        {
            switch (collisionTag)
            {
                case TagGameObject.enemy:
                    DestroyTank(collision.gameObject, this.gameObject);
                    break;
            }
        }
        else
        {
            switch (collisionTag)
            {
                case TagGameObject.player:
                    if (Constants.IsPlayerHaveHelmet)
                    {
                        BulletExplosion(positionImpacted);
                        Destroy(this.gameObject);
                    }
                    else
                    {
                        DestroyTank(collision.gameObject, this.gameObject);
                    }
                    break;
            }
        }
        
    }



    private void DestroyTank(GameObject tank, GameObject bullet)
     {
        Destroy(bullet);
        Destroy(tank);
        TankExplosion(tank.transform.position);
     }


    private void BulletExplosion(Vector3 positionImpacted)
    {
        var bullet = Instantiate(BulletExplosionPrefabs, positionImpacted, Quaternion.identity);
        Destroy(bullet, 1.0f);
    }

    private void TankExplosion(Vector3 position)
    {
        var explosion = Instantiate(BigExplosionPrefabs, position, Quaternion.identity);
        Destroy(explosion, 0.5f);
    }
}