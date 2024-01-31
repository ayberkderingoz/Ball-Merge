using System;
using System.Collections;
using System.Collections.Generic;
using CandyCoded.HapticFeedback;
using DefaultNamespace;
using Scripts.Enum;
using SocialPlatforms;
using Unity.VisualScripting;
using UnityEngine;
using Achievement = UnityEngine.SocialPlatforms.Impl.Achievement;
using Random = UnityEngine.Random;

public class BallManager : MonoBehaviour
{
    
    //singleton
    private static BallManager _instance;
    public static BallManager Instance => _instance;
    
    private List<PooledObject> activeBalls = new List<PooledObject>();
    
    
    
    public List<GameObject> ballsToMerge = new List<GameObject>();
    
    
    [SerializeField] private GameObject _americanBallScoreImage;
    
    public Action OnBallMerge;

    public int _chainCount;
    private int _spawnedBallCount;

    private bool _isBowlingBallReached;

    public BallType _lastMergedBall;
    



    

    public void AddBallToMerge(GameObject ball)
    {
        // TODO: Gamobjectler için Contains yapmak yerine gameobject.name veya nesnelerini classlarını çekip id verebilirsin
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

    

    void Start()
    {
        GameManager.Instance.OnRestart += OnRestart;
        GameManager.Instance.OnGameOver += OnGameOver;
    }
    
    
    private void OnRestart() //TODO: Remove
    {
        Restart();
    }

    private void OnGameOver()
    {
        if (!_isBowlingBallReached)
        {
            AchievementsManager.Instance.GetAchievement(AchievementType.BallisticBlunder).SetCompleted();
        }
        _isBowlingBallReached = false;
        Restart();
    }


    private void MergeBall()
    {
        //get the position between the two balls from ballsToMerge
        var nextBall = GetNextBall();
        if(nextBall == BallType.BowlingBall)
        {
            _isBowlingBallReached = true;
        }
        if (nextBall == _lastMergedBall+1)
        {
            _chainCount++;
            if (_chainCount == 5)
            {
                AchievementsManager.Instance.GetAchievement(AchievementType.CascadeChampion).SetCompleted();
            }
            _lastMergedBall = nextBall;
            
        }
        else
        {
            _lastMergedBall = nextBall;
            _chainCount = 0;
        }
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


        var ball0Rigidbody = ballsToMerge[0].GetComponent<Rigidbody2D>();
        var ball1Rigidbody = ballsToMerge[1].GetComponent<Rigidbody2D>();

        var ball0Velocity = ball0Rigidbody.velocity;
        var ball1Velocity = ball1Rigidbody.velocity;

        var avgVelocity = ball0Velocity + ball1Velocity;


        if (ball.type != PooledObjectType.AmericanFootball)
        {
            ball.gameObject.GetComponent<Rigidbody2D>().velocity = avgVelocity;
        }

        //Returns the balls to the pool
        ballsToMerge[0].GetComponent<Ball>()._pooledObject.ReturnToPool();
        ballsToMerge[1].GetComponent<Ball>()._pooledObject.ReturnToPool();
        
        OnBallMerge?.Invoke(); //plays sound for now
        HapticFeedback.LightFeedback();
        StartCoroutine(SpawnBallAnimated(ball.gameObject));
        
        ScoreManager.Instance.AddScore((int)nextBall);

        if (nextBall == BallType.AmericanFootball)
        {
            StartCoroutine(LerpBallToScoreImage(ball.gameObject));
            ScoreManager.Instance.AddAmericanFootballScore();
            AchievementsManager.Instance.GetAchievement(AchievementType.BallCollector).SetCompleted();
            if (GameTimer.Instance.Time < GameTimer.Instance.MaxTime)
            {
                AchievementsManager.Instance.GetAchievement(AchievementType.FootballFrenzy).SetCompleted();
            }
        }
        else
        {
            ball.gameObject.GetComponent<Collider2D>().enabled = true;
        }


    }

    private BallType GetNextBall()
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
        OnBallMerge+= SoundManager.Instance.OnBallMerge;

    }
    
    

    public GameObject GetRandomBall()
    {

        var random = Random.Range(1, 5);
        PooledObjectType ballTypePooledObject = (PooledObjectType)random;
        BallType ballType = (BallType)random-1;
        var ball = ObjectPool.Instance.GetPooledObject(ballTypePooledObject);
        activeBalls.Add(ball);
        ball.gameObject.GetComponent<Ball>().SetBall(ballType,ball);
        _spawnedBallCount++;
        CheckBallCount();
        return ball.gameObject;
    }
    
    private void CheckBallCount()
    {
        if (_spawnedBallCount == 100)
        {
            AchievementsManager.Instance.GetAchievement(AchievementType.PrecisionDropper).SetCompleted();
        }
    }
    
    public PooledObject GetBall(BallType type)
    {
        var ball = ObjectPool.Instance.GetPooledObject((PooledObjectType)type+1);
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
        _lastMergedBall = BallType.TennisBall;
        _chainCount = 0;
        _spawnedBallCount = 0;
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
        float waitTime = 4f;
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
