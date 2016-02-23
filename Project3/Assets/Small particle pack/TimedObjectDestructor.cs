using UnityEngine;
using System.Collections;

public class TimedObjectDestructor : MonoBehaviour 
{
    int timeOut = 1;
    bool detachChildren = false;

void Awake ()
{
        DestroyNow(timeOut);
}

    public void DestroyNow (int time)
    {
        if (detachChildren) 
        {
            transform.DetachChildren ();
        }
        DestroyObject (gameObject);
    }
}
