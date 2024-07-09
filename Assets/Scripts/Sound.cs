using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound 
{
    [SerializeField] string name;
    [SerializeField] AudioClip clip;

    public string GetName()
    {
        return name;
    }
    public AudioClip GetClip()
    {
        return clip;
    }
}
