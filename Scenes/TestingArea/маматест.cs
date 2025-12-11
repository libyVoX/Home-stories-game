using System.Collections;
using UnityEngine;
using DialogueEditor;

public class маматест : MonoBehaviour
{
    [SerializeField] private NPCConversation convers;
    [SerializeField] private GameObject DialoguePanel;
    [SerializeField] private float duration = 0.001f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (DialoguePanel != null)
        {
            canvasGroup = DialoguePanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = DialoguePanel.AddComponent<CanvasGroup>();
            }
        }
    }

    public void StartConvers() 
    {
        ConversationManager.Instance.StartConversation(convers);
    }

    public void ShowDialoguePanel() 
    {
        if (DialoguePanel == null) return;

        DialoguePanel.SetActive(true);
        StartCoroutine(FadePanel(0f, 1f, duration, false));
    }

    public void HideDialoguePanel() 
    {
        if (DialoguePanel == null) return;

        StartCoroutine(FadePanel(1f, 0f, duration, true));
    }

    // Плавное появление/исчезновение одной панели через CanvasGroup
    private IEnumerator FadePanel(float startAlpha, float targetAlpha, float duration, bool disableAfter)
    {
        if (canvasGroup == null) yield break;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime * 3;
            float t = Mathf.Clamp01(elapsed / duration);
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            canvasGroup.alpha = alpha;

            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        if (disableAfter)
            DialoguePanel.SetActive(false);
    }
}
