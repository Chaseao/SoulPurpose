using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AudioControls : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<int, AudioSource> audioFiles = new Dictionary<int, AudioSource>();
    [SerializeField] float adjustmentSpeed;

    int[] desiredVolumes = { 50, 0, 0, 0, 0, 0};
    public void SetAudio(int[] audioVolumes)
    {
        desiredVolumes = audioVolumes;
    }

    private void Update()
    {
        MoveTowardsDesiredVolumes();
    }

    private void MoveTowardsDesiredVolumes()
    {
        for (int i = 0; i < desiredVolumes.Length; i++)
        {
            if (audioFiles.ContainsKey(i) && audioFiles[i].volume != desiredVolumes[i])
            {
                float directionToIncrement = Mathf.Sign(desiredVolumes[i] / 100f - audioFiles[i].volume);
                float amountToIncrment = adjustmentSpeed * Time.deltaTime / 100f;
                audioFiles[i].volume += directionToIncrement * amountToIncrment;
                audioFiles[i].volume = directionToIncrement > 0
                    ? Mathf.Min(audioFiles[i].volume, desiredVolumes[i] / 100f)
                    : Mathf.Max(audioFiles[i].volume, desiredVolumes[i] / 100f);
            }
        }
    }
}
