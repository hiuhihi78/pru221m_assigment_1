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

    private SpriteRenderer _renderer;
    public float speed;

    private bool isMove;

    public new Rigidbody2D rigidbody2D;

    private float timer;
    public float delay;



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

        gameObject.transform.position = _tank.Position;
        isMove = false;

        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > delay)
        {
            isMove = true;
        }

    }


    private void Fire(Vector3 initialPoint)
    {
        var bullet = new Bullet()
        {
            Direction = _tank.Direction,
            Tank = _tank,
            InitialPosition = initialPoint
        };
        GetComponent<TankFirer>().Fire(bullet);
    }
}
