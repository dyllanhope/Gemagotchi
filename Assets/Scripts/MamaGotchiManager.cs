using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamaGotchiManager : MonoBehaviour
{
    [SerializeField] List<Sprite> spriteList = new();
    [SerializeField] ParticleSystem upgradeParticles;

    int currentIndex = -1;

    AudioPlayer player;

    private void Awake()
    {
        player = FindObjectOfType<AudioPlayer>();
    }
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = null;
    }

    public void UpgradeMamaGatchi()
    {
        currentIndex++;

        GetComponent<SpriteRenderer>().sprite = spriteList[currentIndex];

        if (upgradeParticles != null)
        {
            ParticleSystem instance = Instantiate(upgradeParticles, transform.position, Quaternion.identity);
            instance.Play();
            player.PlayPartyHornClips();
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }
    public void SetCurrentIndex(int newIndex)
    {
        currentIndex = newIndex;
    }
}
