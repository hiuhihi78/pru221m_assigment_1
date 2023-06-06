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

    public Rigidbody2D rigidbody2D;

    public GameObject BigExplosionPrefabs;

    private float startTimeMove;
    private float randomTimeTankCanMove;
    private Direction tankDirectionToMove;
    private float timeStartShoot;
    private float timeEndShoot;
    private float randomTimeShoot;
    private Direction? stuckTankDirection;
    private Direction lastDirectionTankFirer;


    public float topWrap;
    public float bottomWrap;
    public float leftWrap;
    public float rightWrap;

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
            //Guid = GUID.Generate()
        };

        //gameObject.transform.position = _tank.Position;

        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        startTimeMove = Time.time;
        randomTimeTankCanMove = RandomTimeTankMove();
        tankDirectionToMove = RandomDirectionTank(null);
        timeStartShoot = 0f;
        randomTimeShoot = RandomTimeShoot();
        timeEndShoot = randomTimeShoot;
        lastDirectionTankFirer = tankDirectionToMove;
    }

    // Update is called once per frame
    void Update()
    {
        // moving
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
            randomTimeTankCanMove = Time.time + RandomTimeTankMove();
            tankDirectionToMove = RandomDirectionTank(stuckTankDirection);
        }

        //shooting
        if (timeStartShoot > timeEndShoot)
        {
            Fire(gameObject.transform.position);
            randomTimeShoot = RandomTimeShoot();
            timeEndShoot = timeStartShoot + randomTimeShoot;
        }
        else
        {
            timeStartShoot = Time.time;
        }


        /*
        if (timeStartShoot > timeEndShoot && lastDirectionTankFirer != tankDirectionToMove)
        {
            Fire(gameObject.transform.position);
            randomTimeShoot = RandomTimeShoot();
            timeEndShoot = timeStartShoot + randomTimeShoot;
            lastDirectionTankFirer = _tank.Direction;
        }else if(timeStartShoot > timeEndShoot &&  lastDirectionTankFirer == tankDirectionToMove)
        {
            timeStartShoot = Time.time;
            timeEndShoot = timeStartShoot + randomTimeShoot;
        }
        else
        {
            timeStartShoot = Time.time;
        }
        */

    }


    private float RandomTimeShoot()
    {
        return Random.Range(1, 3);
    }


    private void TankMove(Direction direction)
    {
        var currentPosition = gameObject.transform.position;
        var previousPos = gameObject.transform.position;

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

        
        if ((currentPosition.y > topWrap || currentPosition.y < bottomWrap
            ||
            currentPosition.x > rightWrap || currentPosition.x < leftWrap))
        {
            currentPosition = previousPos;
            tankDirectionToMove = TurnAroundDirectionTank(_tank.Direction);
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
    }

    private Direction RandomDirectionTank(Direction? stuckTankDirection)
    {
        Direction newDirection = RandomDirection(stuckTankDirection);

        if (stuckTankDirection != null && newDirection == stuckTankDirection)
        {
            //newDirection = RanDomDirection(stuckTankDirection);
            newDirection = RandomDirectionTank(stuckTankDirection);
        }

        return newDirection;
    }

    private Direction RandomDirection(Direction? unChoseDirection)
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
            switch (unChoseDirection)
            {
                case Direction.Down:
                    listDirection.Remove(Direction.Down); break;
                case Direction.Left:
                    listDirection.Remove(Direction.Left); break;
                case Direction.Right:
                    listDirection.Remove(Direction.Right); break;
                case Direction.Up:
                    listDirection.Remove(Direction.Up); break;
            }
        }

        var numberOfRandomDirection = listDirection.Count;
        var random = Random.Range(0, numberOfRandomDirection);

        switch (random)
        {
            case 0:
                return listDirection[0];
            case 1:
                return listDirection[1];
            case 2:
                return listDirection[2];
            case 3:
                return listDirection[3];
            default:
                return Direction.Down;
        }
    }


    private float RandomTimeTankMove()
    {
        return Random.Range(1, 2);
    }

    private Direction TurnAroundDirectionTank(Direction currentDirection)
    {
        switch (currentDirection)
        {
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:   
                return Direction.Left;
            case Direction.Up:
                return Direction.Down;
            default: 
                return currentDirection;
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

        var tankFirer = GetComponent<TankFirer>();  
        tankFirer.isTankPlayer = false;
        tankFirer.Fire(bullet); 
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionTag = collision.gameObject.tag;
        switch(collisionTag) 
        {
            case TagGameObject.wallWrap:
                tankDirectionToMove = TurnAroundDirectionTank(_tank.Direction);
                break;
            case TagGameObject.enemy:
                stuckTankDirection = _tank.Direction;
                tankDirectionToMove = RandomDirectionTank(stuckTankDirection);
                break;
            case TagGameObject.water:
                stuckTankDirection = _tank.Direction;
                tankDirectionToMove = RandomDirectionTank(stuckTankDirection);
                break;
            case TagGameObject.wallSteel:
                stuckTankDirection = _tank.Direction;
                tankDirectionToMove = RandomDirectionTank(stuckTankDirection);
                break;
            case TagGameObject.player:
                stuckTankDirection = _tank.Direction;
                tankDirectionToMove = RandomDirectionTank(stuckTankDirection);
                break;
            case TagGameObject.wallBrick:
                stuckTankDirection = _tank.Direction;
                tankDirectionToMove = RandomDirectionTank(stuckTankDirection);
                break;
        }
    }
}
