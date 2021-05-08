using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private int speed;

    private Timer speedTimer;

    private int points = 0;
    
    LevelGenerator levelGenerator;
    
    public int XPozition 
    {
        get
        {
            return (int)(transform.position.x - 0.5f);
        }
    }

    public int YPozition
    {
        get
        {
            return (int)(transform.position.y - 0.5f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        levelGenerator = GameObject.FindObjectOfType<LevelGenerator>();
        speedTimer = gameObject.AddComponent<Timer>();
        speedTimer.Duration = 1f / speed;
        speedTimer.Run();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 celledPosition = transform.position;
        if (Input.GetKey(KeyCode.W) && speedTimer.Finished && !levelGenerator.CheckTheWall(XPozition, YPozition, Side.Up))
        {
            celledPosition += Vector3Int.up;
            speedTimer.Duration = 1f / speed;
            speedTimer.Run();
        }
        else if (Input.GetKey(KeyCode.S) && speedTimer.Finished && !levelGenerator.CheckTheWall(XPozition, YPozition, Side.Down))
        {
            celledPosition += Vector3Int.down;
            speedTimer.Duration = 1f / speed;
            speedTimer.Run();
        }
        else if (Input.GetKey(KeyCode.A) && speedTimer.Finished && !levelGenerator.CheckTheWall(XPozition, YPozition, Side.Left))
        {
            celledPosition += Vector3Int.left;
            speedTimer.Duration = 1f / speed;
            speedTimer.Run();
        }
        else if (Input.GetKey(KeyCode.D) && speedTimer.Finished && !levelGenerator.CheckTheWall(XPozition, YPozition, Side.Right))
        {
            celledPosition += Vector3Int.right;
            speedTimer.Duration = 1f / speed;
            speedTimer.Run();
        }
        transform.position = celledPosition;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Peas")
        {
            points += 1;
            GameObject.Find("TextPoints").GetComponent<Text>().text = "Points: " + points;
            collision.GetComponent<Peas>().DoInvisible();
        }
    }

}
