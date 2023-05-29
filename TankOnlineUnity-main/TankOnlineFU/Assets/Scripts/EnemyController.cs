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
    private Direction? stuckTankDirection;


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
        randomTimeTankCanMove = Random.Range(0.5f, 1f);
        tankDirectionToMove = RandomDirectionTank(null);
        timeStartShoot = 0f;

    }

    // Update is called once per frame
    void Update()
    {


        if (startTimeMove < randomTimeTankCanMove && stuckTankDirection == null)
        {
            TankMove(tankDirectionToMove);
            startTimeMove = Time.time;
        }
        else if(startTimeMove < randomTimeTankCanMove && stuckTankDirection != null)
        {
            tankDirectionToMove = RandomDirectionTank(stuckTankDirection);
            startTimeMove = randomTimeTankCanMove + 1;
            stuckTankDirection = null;
        }
        else
        {
            startTimeMove = Time.time;
            randomTimeTankCanMove = Time.time + Random.Range(0.5f, 1f);
        }


        /*
        if(startTimeMove < randomTimeTankCanMove)
        {
            if(stuckTankDirection != null)
            {
                startTimeMove = randomTimeTankCanMove + 1;
                stuckTankDirection = null;
            }
            else
            {
                TankMove(tankDirectionToMove);
                startTimeMove = Time.time;
            }
        }
        else
        {
            startTimeMove = Time.time;
            randomTimeTankCanMove = Time.time + Random.Range(1, 4);
            tankDirectionToMove = RandomDirectionTank(stuckTankDirection);
        }
        */


    }

    private Direction RandomDirectionTank(Direction? stuckTankDirection)
    {
        Direction newDirection = RanDomDirection(stuckTankDirection);

        if (stuckTankDirection != null && newDirection == stuckTankDirection)
        {
            newDirection = RanDomDirection(stuckTankDirection);
        }

        return newDirection;
    }

    private Direction RanDomDirection(Direction? unChoseDirection)
    {
        
        List<Direction> listDirection = new List<Direction>() 
        {
            Direction.Up,
            Direction.Down,
            Direction.Left,
            Direction.Right
        };

        if (unChoseDirection != null)
        {
            if (unChoseDirection == Direction.Up)
            {
                listDirection.Remove(Direction.Up);
            }
            else if (unChoseDirection == Direction.Down)
            {
                listDirection.Remove(Direction.Down);
            }
            else if (unChoseDirection == Direction.Left)
            {
                listDirection.Remove(Direction.Left);
            }
            else if (unChoseDirection != Direction.Right)
            {
                listDirection.Remove(Direction.Right);
            }
        }


        var numberOfRandomDirection = listDirection.Count;
        var random = Random.Range(0, numberOfRandomDirection);

        switch (random)
        {
            case 1:
                return listDirection[0];
            case 2: 
                return listDirection[1];
            case 3: 
                return listDirection[2];    
            case 4: 
                return listDirection[3];
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
        
        /*
        delay = Random.Range(1, 4);

        if (Time.time > timeStartShoot + delay)
        {
            Fire(gameObject.transform.position);
            timeStartShoot = Time.time;
        }
        */
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
        var collisionTag = collision.gameObject.tag;
        if (collisionTag == "wallWrapUp" ||
            collisionTag == "wallWrapLeft" ||
            collisionTag == "wallWrapRight" ||
            collisionTag == "wallWrapButton" ||
            collisionTag == "Enemy" ||
            collisionTag == "Player" ||
            collisionTag == "water" ||
            collisionTag == "wallSteel"
            )
        {
            stuckTankDirection = _tank.Direction;
            Debug.Log(stuckTankDirection.ToString());
            tankDirectionToMove = RandomDirectionTank(stuckTankDirection);
        }
    }

}
