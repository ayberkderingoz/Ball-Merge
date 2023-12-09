using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    private Camera _camera;
    // Start is called before the first frame update
    
    
    void Start()
    {
        _camera = Camera.main;
        //Debug.Log("ClickManager Start");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Clicked!");;
            Vector3 mousePosition = Input.mousePosition;

            Vector3 worldPosition = _camera.ScreenToWorldPoint(mousePosition);

            DropBall(worldPosition);

        }
    }
    
    
    
    private void DropBall(Vector3 position)
    {
        var activeBall = BallSpawner.Instance.GetActiveBall(); 
        activeBall.GetComponent<Rigidbody2D>().gravityScale = 1;
        activeBall.transform.position = new Vector3(position.x, _camera.orthographicSize/1.5f, 0);
        activeBall.transform.localPosition = new Vector3(activeBall.transform.localPosition.x, activeBall.transform.localPosition.y,0);
        activeBall.GetComponent<Ball>().StartDropping();
        BallSpawner.Instance.SpawnBall();
    }









}
