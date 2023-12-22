using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTopScore : MonoBehaviour
{
    
    void Start()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = PlayerPrefs.GetInt("MaxScore").ToString();
    }
    
}
