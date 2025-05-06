using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarManager : MonoBehaviour
{
    public void EnableSignal()
    {
        this.gameObject.SetActive(true);
    }
    public void DisableSignal()
    {
        this.gameObject.SetActive(false);
    }
}
