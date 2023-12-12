using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Enum;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public class BallManager : MonoBehaviour
{
    
    //singleton
    private static BallManager _instance;
    public static BallManager Instance => _instance;
    
    private List<PooledObject> activeBalls = new List<PooledObject>();
    
    
    
    public List<GameObject> ballsToMerge = new List<GameObject>();
    
    
    [SerializeField] private GameObject _americanBallScoreImage;
    


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

        StartCoroutine(SpawnBallAnimated(ball.gameObject));
        ScoreManager.Instance.AddScore((int)nextBall);

        if (nextBall == PooledObjectType.AmericanFootball)
        {
            StartCoroutine(LerpBallToScoreImage(ball.gameObject));
            ScoreManager.Instance.AddAmericanFootballScore();
        }


    }

    private PooledObjectType GetNextBall()
    {
        var type = ballsToMerge[0].GetComponent<Ball>()._type;
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
        foreach (var ball in activeBalls)
        {
            ball.ReturnToPool();
        }
        activeBalls.Clear();
        ballsToMerge.Clear();
        BallSpawner.Instance.SpawnBall();
    }
    //Create ball smoothly
    private IEnumerator SpawnBallAnimated(GameObject ball)
    {
        //lerp to original scale from 0
        //get original scale
        var scale = ball.transform.localScale;
        ball.transform.localScale = Vector3.zero;
        float elapsedTime = 0;
        float waitTime = 0.2f;
        while (elapsedTime < waitTime)
        {
            ball.transform.localScale = Vector3.Lerp(Vector3.zero, scale, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }


    }
    
    //lerp the ball to the position of americanfootbalImage
    public IEnumerator LerpBallToScoreImage(GameObject ball)
    {
        var pos = _americanBallScoreImage.transform.position;
        float elapsedTime = 0;
        float waitTime = 2f;
        yield return new WaitForSeconds(1f);
        while (elapsedTime < waitTime)
        {
            ball.transform.position = Vector3.Lerp(ball.transform.position, pos, (elapsedTime / waitTime));
            ball.transform.localScale = Vector3.Lerp(ball.transform.localScale, Vector3.zero, (elapsedTime / waitTime));
            ball.transform.Rotate(0,0,Time.deltaTime * 1000);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ball.GetComponent<Ball>()._pooledObject.ReturnToPool();
    }

}
