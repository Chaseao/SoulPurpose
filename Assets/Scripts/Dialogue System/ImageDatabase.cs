using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/ImageDatabase")]
public class ImageDatabase : ScriptableObject
{
    [SerializeField] Dictionary<string, Sprite> portraits;

    public Sprite GetPortrait(string characterName)
    {
        return portraits[characterName];
    }
}