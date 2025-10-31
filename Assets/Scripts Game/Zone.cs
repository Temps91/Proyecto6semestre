using UnityEngine;

public class Zone : MonoBehaviour
{
    public GameObject[] spawners;
    public bool hasPlayer;    
    public void PlayerEntered()
    { 
        hasPlayer = true; 
    }
    public void PlayerExited()
    {
        hasPlayer = false;
    }
    
}
