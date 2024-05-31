using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Swapping")]
    [SerializeField] AudioClip swappingClip;
    [SerializeField][Range(0f, 5f)] float swappingVolume = 1f;
    public void PlaySwappingClip()
    {
        PlayClip(swappingClip, swappingVolume);
    }
    private void PlayClip(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            Vector3 soundPos = transform.position;
            AudioSource.PlayClipAtPoint(clip, soundPos, volume);
        }
    }
}
