using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageFlow : MonoBehaviour
{

    [SerializeField] GameObject[] toggleObj;
    
    public void ToggleObj()
    {
        foreach(GameObject gb in toggleObj)
        {
            gb.SetActive(!gb.activeInHierarchy);
        }
        //Scramble words
    }
}
