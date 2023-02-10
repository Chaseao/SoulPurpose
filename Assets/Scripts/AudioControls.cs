using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using System.Linq;

public class AudioControls : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<int, AudioSource> audioFiles = new Dictionary<int, AudioSource>();
    [SerializeField] AudioSource[] audioProx;
    [SerializeField] float adjustmentSpeed;

    int desiredProxVolumes = 50;
    int[] desiredVolumes = { 50, 0, 0, 0, 0, 0};
    public void SetAudio(int[] audioVolumes, bool enableProx)
    {
        desiredVolumes = audioVolumes;
        desiredProxVolumes = enableProx ? 0 : 50;
    }

    private void Update()
    {
        foreach(var audioFile in audioFiles)
        {
            MoveTowards(audioFile.Value, desiredVolumes[audioFile.Key]);
        }

        audioProx.ToList().ForEach(audio => MoveTowards(audio, desiredProxVolumes));
    }

    private void MoveTowards(AudioSource audio, float desiredVolume)
    {
        if (audio.volume == desiredVolume) return;
            
        float directionToIncrement = Mathf.Sign(desiredVolume / 100f - audio.volume);
        float amountToIncrment = adjustmentSpeed * Time.deltaTime / 100f;
        audio.volume += directionToIncrement * amountToIncrment;
        audio.volume = directionToIncrement > 0
            ? Mathf.Min(audio.volume, desiredVolume / 100f)
            : Mathf.Max(audio.volume, desiredVolume / 100f);
    }
}
