using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peas : MonoBehaviour
{
    [SerializeField] private int timeOfRespawnPeas;

    private bool invisible = false;

    private Timer respawnTimer;

    private System.Random random;

    private void Start()
    {
        random = GameObject.Find("LevelGenirator").GetComponent<LevelGenerator>().SysRandom;
        respawnTimer = gameObject.AddComponent<Timer>();
        transform.position = new Vector3(random.Next(-5, 5) + 0.5f, random.Next(-5, 5) + 0.5f, 0);
    }

    private void Update()
    {
        if (invisible && respawnTimer.Finished)
        {
            transform.position = new Vector3(random.Next(-5, 5) + 0.5f, random.Next(-5, 5) + 0.5f, 0);
            GetComponentInChildren<SpriteRenderer>().forceRenderingOff = false;
            invisible = false;
        }

    }

    /// <summary>
    /// This method does invisible the peas
    /// </summary>
    public void DoInvisible()
    {
        respawnTimer.Duration = timeOfRespawnPeas;
        respawnTimer.Run();
        invisible = true;
        transform.position = new Vector3(10.5f, 0.5f, 0);
        GetComponentInChildren<SpriteRenderer>().forceRenderingOff = true;
    }
}
