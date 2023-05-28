using DefaultNamespace;
using Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    // Start is called before the first frame update
    private Tank _tank;

    public Sprite tankUp;
    public Sprite tankDown;
    public Sprite tankLeft;
    public Sprite tankRight;

    public SpriteRenderer spriteRenderer;

    public float speed;

    public new Rigidbody2D rigidbody2D;

    public float delay;

    private float startTimeMove;
    private float randomTimeTankCanMove;
    private Direction tankDirectionToMove;
    private float timeStartShoot;


    // Start is called before the first frame update
    void Start()
    {
        _tank = new Tank
        {
            Name = "Default",
            Direction = Direction.Down,
            Hp = 0,
            Point = 0,
            Position = new Vector3(3, 0, 0),
            Guid = GUID.Generate()
        };

        //gameObject.transform.position = _tank.Position;

        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        startTimeMove = Time.time;
        randomTimeTankCanMove = Random.Range(1, 4);
        tankDirectionToMove = RandomDirectionTank();
        timeStartShoot = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        if(startTimeMove < randomTimeTankCanMove)
        {
            TankMove(tankDirectionToMove);
            startTimeMove = Time.time;
        }
        else
        {
            startTimeMove = 0;
            randomTimeTankCanMove = Time.time + Random.RandomRange(1, 4);
            tankDirectionToMove = RandomDirectionTank();
        }
        
    }

    private Direction RandomDirectionTank()
    {
        var random = Random.Range(1, 5);
        switch (random)
        {
            case 1:
                return Direction.Up;
            case 2:
                return Direction.Down;
            case 3:
                return Direction.Left;
            case 4:
                return Direction.Right;
            default:
                return Direction.Down;
        }

    }


    private void TankMove(Direction direction)
    {
        var currentPosition = gameObject.transform.position;
        if (direction == Direction.Down) 
        {
            currentPosition.y -= speed * Time.deltaTime;    
        }
        else if (direction == Direction.Up) 
        {
            currentPosition.y += speed * Time.deltaTime;
        }
        else if(direction == Direction.Left) 
        {
            currentPosition.x -= speed * Time.deltaTime;
        }
        else // right
        {
            currentPosition.x += speed * Time.deltaTime;
        }

        gameObject.transform.position = currentPosition;    

        /// sprite
        spriteRenderer.sprite = direction switch
        {
            Direction.Down => tankDown,
            Direction.Up => tankUp,
            Direction.Left => tankLeft,
            Direction.Right => tankRight,
            _ => spriteRenderer.sprite
        };

        _tank.Direction = direction;
        

        delay = Random.Range(1, 4);

        if (Time.time > timeStartShoot + delay)
        {
            Fire(gameObject.transform.position);
            timeStartShoot = Time.time;
        }
    }


    private void Fire(Vector3 initialPoint)
    {
        timeStartShoot = Time.time; 
        var bullet = new Bullet()
        {
            Direction = _tank.Direction,
            Tank = _tank,
            InitialPosition = initialPoint
        };


        GetComponent<TankEnemyFirer>().Fire(bullet);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(
            collision.gameObject.tag == "Enemy"    ||
            collision.gameObject.tag == "wallWrap" ||
            collision.gameObject.tag == "Player"   ||
            collision.gameObject.tag == "water"    ||
            collision.gameObject.tag == "wallSteel"
           )
        {
            RandomDirectionTank();
        }
    }

}
