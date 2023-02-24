using UnityEngine;

public class TitleUIButton : UIButton
{
    [SerializeField] GameObject starImage;

    public override void ToggleSelected(bool isSelected)
    {
        starImage.SetActive(isSelected);
    }
}