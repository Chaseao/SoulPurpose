using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GreyScaleEffectManager : MonoBehaviour
{
    ColorGrading grading;

    private void Start()
    {
        if (TryGetComponent(out PostProcessVolume postProcessData) && postProcessData.profile.TryGetSettings(out ColorGrading grading))
        {
            this.grading = grading;

            DialogueManager.OnDialogueStarted += EnableGrayscale;
            DialogueManager.OnDialogueEnded += DisableGrayscale;
        }
        else
        {
            Destroy(this);
        }
    }

    private void EnableGrayscale(DialogueHelperClass.ConversationData _)
    {
        grading.active = true;
    }

    private void DisableGrayscale()
    {
        grading.active = false;
    }

    private void OnDestroy()
    {
        DialogueManager.OnDialogueStarted -= EnableGrayscale;
        DialogueManager.OnDialogueEnded -= DisableGrayscale;
    }
}
