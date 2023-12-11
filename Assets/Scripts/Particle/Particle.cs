using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float lifeTime;
    public PooledObject pooledObject;
    
    public void DestroyObject()
    {
        Invoke(nameof(Disable), lifeTime);
    }

    private void Disable()
    {
        
        gameObject.SetActive(false);
        pooledObject.ReturnToPool();
    }
    
    
    
}
