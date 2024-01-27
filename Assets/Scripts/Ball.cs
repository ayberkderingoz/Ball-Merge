using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public PooledObjectType _type;
    public PooledObject _pooledObject;
    public bool isActive = false;
    public  Rigidbody2D _rigidbody2D;
    
    public float elapsed = 0f;
    
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    


    public void DisableCollider()
    {
        GetComponent<CircleCollider2D>().enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            if (other.gameObject.GetComponent<Ball>()._type == _type)
            {
                MergeBall(other);
            }
            
        }
        
    }
    

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("LevelTrigger"))
        {
            elapsed += Time.fixedDeltaTime;
            if (elapsed > 1f)
            {
                
                Debug.Log("Game OVer");
                GameManager.Instance.GameOver();
                elapsed = 0f;
            }
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            _rigidbody2D.AddForce(new Vector2(0.5f, 0.5f), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("LevelTrigger"))
        {
            elapsed = 0f;
        }
    }


    public void SetBall(PooledObjectType type,PooledObject pooledObject)
    {
        _type = type;
        _pooledObject = pooledObject;
    }
    

    private void MergeBall(Collision2D other) //Gets a new ball from the pool and sets it to the middle of the two balls and removes the other balls
    {
        BallManager.Instance.AddBallToMerge(gameObject);
        
    }

    public void StartDropping()
    {
        GetComponent<CircleCollider2D>().enabled = true;
    }
    public void ResetElapsed()
    {
        elapsed = 0f;
    }
    
    

}
