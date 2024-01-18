using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainMenuBallEffect : MonoBehaviour
{
    //7 serialize field gameobjects
    [SerializeField] private GameObject _ball1;
    [SerializeField] private GameObject _ball2;
    [SerializeField] private GameObject _ball3;
    [SerializeField] private GameObject _ball4;
    [SerializeField] private GameObject _ball5;
    [SerializeField] private GameObject _ball6;
    [SerializeField] private GameObject _ball7;
    

    
    //Randomly move the balls
    void Start()
    {
        StartCoroutine(MoveBall(_ball1));
        StartCoroutine(MoveBall(_ball2));
        StartCoroutine(MoveBall(_ball3));
        StartCoroutine(MoveBall(_ball4));
        StartCoroutine(MoveBall(_ball5));
        StartCoroutine(MoveBall(_ball6));
        StartCoroutine(MoveBall(_ball7));
    }
    
    //add random force to the balls smoothly
    IEnumerator MoveBall(GameObject ball)
    {
        var rb = ball.GetComponent<Rigidbody2D>();
        while (true)
        {
 
            rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), ForceMode2D.Impulse);
            var randomTime = Random.Range(0.1f, 1.9f);
            yield return new WaitForSeconds(randomTime);
        }
    }
    

}
