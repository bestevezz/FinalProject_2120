using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kvant; 

public class WigInteraction : MonoBehaviour
{
    public List<WigController> wigs;
    public float length = 3.0f; 


    public void AdjustWigLength(float length)
    {
        length *= 5; 
        Debug.Log("Changing Wig Length to : " + length);
        foreach (var wig in wigs)
        {
            wig.length = length;
            wig.randomSeed = Random.Range(0, 5.0f); 
        }
    }

}
