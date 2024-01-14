using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ClickManager : MonoBehaviour
{
    private Camera _camera;
    
    public float _cooldown = 1.2f;
    private float _currentCooldown = 0f;
    private bool _isClickable = true;
    


    void Start()
    {
        _camera = Camera.main;
        PanelManager.Instance.OnPanelOpen += OnPanelOpen;
        PanelManager.Instance.OnPanelClose += OnPanelClose;
        
        
        

    }

    private void OnPanelOpen()
    {
        _isClickable = false;
    }

    private void OnPanelClose()
    {
        _isClickable = true;
    }
    void Update()
    {
        _currentCooldown += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && _currentCooldown >= _cooldown && _isClickable)
        {
            
            Vector3 mousePosition = Input.mousePosition;

            Vector3 worldPosition = _camera.ScreenToWorldPoint(mousePosition);
            
            //check if button is clicked
            if (worldPosition.y > 4)
            {
                return;
            }






            DropBall(worldPosition);
            _currentCooldown = 0f;
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
