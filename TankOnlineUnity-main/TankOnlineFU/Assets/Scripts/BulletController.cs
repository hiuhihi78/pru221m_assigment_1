using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Bullet Bullet { get; set; }

    public int MaxRange { get; set; }

    public GameObject BulletExplosionPrefabs { get; set; }   

    public GameObject BigExplosionPrefabs { get; set; }


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
        if (collisionTag == "wallWrapUp" ||
            collisionTag == "wallWrapLeft" ||
            collisionTag == "wallWrapRight" ||
            collisionTag == "wallWrapButton"
            )
        {
            Destroy(this.gameObject);
            BulletExplosion(positionImpacted);
        }

        if(collisionTag == "Enemy")
        {
            Destroy(this.gameObject);
            var enemyObj = collision.gameObject;
            Destroy(enemyObj);
            TankExplosion(enemyObj.transform.position);
        }
    }


    private void BulletExplosion(Vector3 positionImpacted)
    {
        var bullet = Instantiate(BulletExplosionPrefabs, positionImpacted, Quaternion.identity);
        Destroy(bullet, 1.0f);
    }

    private void TankExplosion(Vector3 position)
    {
        var explosion = Instantiate(BigExplosionPrefabs, position, Quaternion.identity);
        Destroy(explosion, 1.5f);
    }
}