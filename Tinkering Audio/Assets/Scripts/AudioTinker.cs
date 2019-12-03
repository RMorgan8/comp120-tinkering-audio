using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioTinker : MonoBehaviour {
    private AudioSource audioSource;
    private AudioClip outAudioClip;

    private static int NUMBER_OF_CHANNELS = 1;
    
    
    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();

        System.Random rand = new System.Random();

        for (int i = 0; i < 5; i++)
        {
            outAudioClip = CreateToneAudioClip((int) (rand.NextDouble() * 21000), 0.25f);
            PlayOutAudio();
            System.Threading.Thread.Sleep(900);
        }
    }
    

    // Public APIs
    public void PlayOutAudio() {
        audioSource.PlayOneShot(outAudioClip);
    }


    public void StopAudio() {
        audioSource.Stop();
    }


    // Private 
    private AudioClip CreateToneAudioClip(int frequency, float durationInSeconds) {
        float sampleDurationSecs = durationInSeconds;
        int sampleRate = 44100;
        int sampleLength = (int) (sampleRate * sampleDurationSecs);
        float maxValue = 1f / 4f;
        
        var audioClip = AudioClip.Create("tone", sampleLength, NUMBER_OF_CHANNELS, sampleRate, false);

        float[] low_freq_samples = GenerateSquareWave(frequency, sampleLength, maxValue, sampleRate);

        float[] high_freq_samples = GenerateSquareWave(frequency * 2, sampleLength, maxValue, sampleRate);

        float[] added_freqs = new float[sampleLength];
        for (int i = 0; i < sampleLength; i++)
        {
            added_freqs[i] = low_freq_samples[i] + high_freq_samples[i];
        }

        audioClip.SetData(added_freqs, 0);
        return audioClip;
    }

    private float[] GenerateSquareWave(
        int frequency, 
        int sampleLength, 
        float maxValue, 
        float sampleRate)
    {
        float[] samples = new float[sampleLength];
        for (var i = 0; i < sampleLength; i++)
        {
            float s = Mathf.Sin(2.0f * Mathf.PI * frequency * ((float)i / (float)sampleRate));

            if (s >= 0)
            {
                s = 1;
            }
            else
            {
                s = -1;
            }
            

            float v = s * maxValue;
            samples[i] = v;
        }

        return samples;
    }

}