using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Enum;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallManager : MonoBehaviour
{
    
    //singleton
    private static BallManager _instance;
    public static BallManager Instance => _instance;
    
    private List<PooledObject> activeBalls = new List<PooledObject>();
    
    
    
    public List<GameObject> ballsToMerge = new List<GameObject>();
    


    public void AddBallToMerge(GameObject ball)
    {
        if (ballsToMerge.Contains(ball))
        {
            return;
        }
        ballsToMerge.Add(ball);
        if (ballsToMerge.Count == 2)
        {
            MergeBall();
            ballsToMerge.Clear();
        }
    }

    private void MergeBall()
    {
        //get the position between the two balls from ballsToMerge
        var nextBall = GetNextBall();
        var ball = GetBall(nextBall);
        
        ball.transform.position = GetPositionBetween(ballsToMerge[0].transform.position, ballsToMerge[1].transform.position);
        ball.gameObject.SetActive(true);
        ball.gameObject.GetComponent<Ball>().SetBall(nextBall,ball);
        
        //get particle from objectpool
        var particle = ObjectPool.Instance.GetPooledObject(PooledObjectType.MergeParticle);
        particle.transform.position = ball.transform.position;
        particle.gameObject.SetActive(true);
        particle.gameObject.GetComponent<ParticleSystem>().Play();
        particle.gameObject.GetComponent<Particle>().pooledObject = particle;
        particle.gameObject.GetComponent<Particle>().DestroyObject();
        
        //Returns the balls to the pool
        ballsToMerge[0].GetComponent<Ball>()._pooledObject.ReturnToPool();
        ballsToMerge[1].GetComponent<Ball>()._pooledObject.ReturnToPool();
        
        
        ScoreManager.Instance.AddScore((int)nextBall);
        
        
    }

    private PooledObjectType GetNextBall()
    {
        var type = ballsToMerge[0].GetComponent<Ball>()._type;
        if (type == PooledObjectType.AmericanFootball) return PooledObjectType.AmericanFootball; //TODO: if the ball is the last one pop it 
        
        return type + 1;
    }

    private Vector3 GetPositionBetween(Vector3 pos1,Vector3 pos2)
    {
        return new Vector3((pos1.x + pos2.x) / 2, (pos1.y + pos2.y) / 2, pos1.z);
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    
    

    public GameObject GetRandomBall()
    {
        PooledObjectType ballType = (PooledObjectType)Random.Range(1, 5);
        var ball = ObjectPool.Instance.GetPooledObject(ballType);
        activeBalls.Add(ball);
        ball.gameObject.GetComponent<Ball>().SetBall(ballType,ball);
        return ball.gameObject;
    }
    
    public PooledObject GetBall(PooledObjectType type)
    {
        var ball = ObjectPool.Instance.GetPooledObject(type);
        activeBalls.Add(ball);
        return ball;
    }

    public void Restart()
    {
        //restart
        foreach (var ball in activeBalls)
        {
            ball.ReturnToPool();
        }
        activeBalls.Clear();
        ballsToMerge.Clear();
        BallSpawner.Instance.SpawnBall();
    }
}
