using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamaGotchiManager : MonoBehaviour
{
    [SerializeField] List<Sprite> spriteList = new();
    [SerializeField] ParticleSystem upgradeParticles;
    [SerializeField] GameObject winnerGatchi;

    int currentIndex = -1;

    AudioPlayer player;

    private void Awake()
    {
        player = FindObjectOfType<AudioPlayer>();
        winnerGatchi.SetActive(false);
    }
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = null;
    }

    public void UpgradeMamaGatchi(bool winState)
    {
        Debug.Log(winState);
        if (!winState)
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
        else
        {
            winnerGatchi.SetActive(true);
            ParticleSystem instance = Instantiate(upgradeParticles, transform.position, Quaternion.identity);
            var main = instance.main;
            main.loop = true;
            instance.Play();
            player.PlayPartyHornClips();
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
