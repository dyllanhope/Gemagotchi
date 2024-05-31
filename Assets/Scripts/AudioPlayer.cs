using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Swapping")]
    [SerializeField] AudioClip swappingClip;
    [SerializeField][Range(0f, 1f)] float swappingVolume = 1f;

    [Header("Party Horn")]
    [SerializeField] AudioClip hornOne;
    [SerializeField] AudioClip hornTwo;
    [SerializeField] AudioClip hornThree;
    [SerializeField][Range(0f, 1f)] float hornVolume = 0.3f;
    public void PlaySwappingClip()
    {
        PlayClip(swappingClip, swappingVolume);
    }
    public void PlayPartyHornClips()
    {
        PlayClip(hornOne, hornVolume);
        PlayClip(hornTwo, hornVolume);
        PlayClip(hornThree, hornVolume);
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
