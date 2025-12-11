using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DialogueEditor;

public class Story1Scripts : MonoBehaviour
{
    [SerializeField] private NPCConversation conversation;
    [SerializeField] private GameObject Image1;
    [SerializeField] private GameObject Image2;
    [SerializeField] private GameObject Image3;
    [SerializeField] private GameObject Image4;
    [SerializeField] private GameObject Image5;
    [SerializeField] private GameObject Image6;


    [SerializeField] private AudioSource audioSource; // drag your AudioSource here
    

    // Обёртка — позволяет await корутину
    private Task WaitForCoroutine(IEnumerator coroutine)
    {
        var tcs = new TaskCompletionSource<bool>();
        StartCoroutine(RunCoroutine());

        IEnumerator RunCoroutine()
        {
            yield return coroutine;
            tcs.SetResult(true);
        }

        return tcs.Task;
    }

    private IEnumerator FadeOutImage(GameObject imgObj, float duration)
    {
        Image img = imgObj.GetComponent<Image>();
        if (img == null) yield break;

        Color color = img.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            color.a = Mathf.Lerp(1f, 0f, t);
            img.color = color;
            yield return null;
        }

        color.a = 0f;
        img.color = color;
    }

    private IEnumerator FadeInImage(GameObject imgObj, float duration)
    {
        Image img = imgObj.GetComponent<Image>();
        if (img == null) yield break;

        Color color = img.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            color.a = Mathf.Lerp(0f, 1f, t);
            img.color = color;
            yield return null;
        }

        color.a = 1f;
        img.color = color;
    }

    // ❗ Полностью async последовательная функция для вызова из ноды
    public async void ShowImage3()
    {
        await WaitForCoroutine(FadeInImage(Image1, 0.5f));
        Image2.SetActive(false);
        await WaitForCoroutine(FadeOutImage(Image1, 0.5f));
    }

    public async void ShowImage2()
    {
       await WaitForCoroutine(FadeOutImage(Image1, 1f));
    }

    public void ShowImage4()
    {
        Image3.SetActive(false);
    }

    public void ShowImage5()
    {
        Image4.SetActive(false);
    }

    public void ShowImage6()
    {
        Image5.SetActive(false);
    }

    public async void ShowScene7(AudioClip doorClose)
    {
        await WaitForCoroutine(FadeInImage(Image1, 0.5f));
        Image6.SetActive(false);
        Image2.SetActive(true);
        PlaySound(doorClose);
        await WaitForCoroutine(FadeOutImage(Image1, 1f));
    }

    public void PlaySound(AudioClip sound) {
        audioSource.clip = sound;
        audioSource.Play();
    }

    private void Start()
    {
        ConversationManager.Instance.StartConversation(conversation);
    }
}
