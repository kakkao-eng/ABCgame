using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSandboxManager : MonoBehaviour
{
    [SerializeField] AudioSource musicsource;

    public AudioClip background;

    private void Start()
    {
        musicsource.clip = background;
        musicsource.Play();
    }
}
