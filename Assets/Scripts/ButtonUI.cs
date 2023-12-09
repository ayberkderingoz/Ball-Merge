using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUI : MonoBehaviour
{
    public void Restart()
    {
        Debug.Log("Restart");
        GameManager.Instance.Restart();
    }
}
