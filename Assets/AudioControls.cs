using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AudioControls : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<int, AudioSource> audioFiles = new Dictionary<int, AudioSource>();

    public void SetAudio(int[] audioVolumes)
    {
        for (int i = 0; i < audioVolumes.Length; i++)
        {
            if (audioFiles.ContainsKey(i))
            {
                audioFiles[i].volume = audioVolumes[i] / 100f;
            }
        }
    }
}
