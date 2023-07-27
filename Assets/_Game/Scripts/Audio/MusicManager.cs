using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] musicTracks;
    private int currentIndex;
    private AudioSource audioSource;
    private bool[] trackPlayed;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        trackPlayed = new bool[musicTracks.Length];
        ShuffleMusic();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    void ShuffleMusic()
    {
        currentIndex = 0;
        for (int i = 0; i < trackPlayed.Length; i++)
        {
            trackPlayed[i] = false;
        }

        // Fisher-Yates shuffle algorithm to shuffle the music array
        for (int i = musicTracks.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            AudioClip temp = musicTracks[i];
            musicTracks[i] = musicTracks[randomIndex];
            musicTracks[randomIndex] = temp;
        }
    }

    void PlayNextTrack()
    {
        if (currentIndex >= musicTracks.Length)
        {
            ShuffleMusic();
        }

        audioSource.clip = musicTracks[currentIndex];
        audioSource.Play();
        trackPlayed[currentIndex] = true;
        currentIndex++;
    }
}
