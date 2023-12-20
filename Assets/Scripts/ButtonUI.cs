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
    
    //open menu
    public void OpenMenu()
    {
        
        PanelManager.Instance.OpenRestartPanel();
        
    }

    public void CloseMenu()
    {
        PanelManager.Instance.CloseRestartPanel();  
    }
    
    public void ContinueWithAd()
    {
        GameManager.Instance.ContinueWithAd();
    }
}
