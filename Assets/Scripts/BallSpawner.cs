using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    
    //get main camera property
    public Camera mainCamera;
    public float spawnHeigth;

    public GameObject activeBall;




    private static BallSpawner _instance;
    public static BallSpawner Instance => _instance;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    void Start()
    {
        spawnHeigth = mainCamera.orthographicSize/1.5f;
        SpawnBall();
    }

    public void SpawnBall()
    {
        activeBall = BallManager.Instance.GetRandomBall();
        activeBall.transform.position  = new Vector3(0, spawnHeigth,0);
        activeBall.transform.localPosition = new Vector3(activeBall.transform.localPosition.x, activeBall.transform.localPosition.y,0);
        activeBall.gameObject.SetActive(true);
        activeBall.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        activeBall.gameObject.GetComponent<Ball>().DisableCollider();
    }
    public GameObject GetActiveBall()
    {
        return activeBall;
    }
    
}
