using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DialogueEditor;

public class MainScriptEp2 : MonoBehaviour
{
    [SerializeField] private NPCConversation conversation;
    public GameObject timaNegr;  // родитель с черным Image и текстом
    public TMP_Text nextDay;      // текст TMP
    public GameObject mom1;
    public GameObject mom2;
    public GameObject mom4;
    public GameObject mom5;
    public GameObject mom6;
    public GameObject mom7;
    public GameObject moms;
    public GameObject badEnding;

    // --- Утилита для конвертации Coroutine в Task ---
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

    // --- Fade для TMP Text ---
    private IEnumerator FadeInText(TMP_Text text, float duration)
    {
        Color color = text.color;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            color.a = Mathf.Lerp(0f, 1f, t);
            text.color = color;
            yield return null;
        }
        color.a = 1f;
        text.color = color;
    }

    private IEnumerator FadeOutText(TMP_Text text, float duration)
    {
        Color color = text.color;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            color.a = Mathf.Lerp(1f, 0f, t);
            text.color = color;
            yield return null;
        }
        color.a = 0f;
        text.color = color;
    }

    // --- Fade для Image ---
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

    // --- Универсальный FadeOut для объекта с Image и Text ---
    private IEnumerator FadeOutGameObject(GameObject obj, float duration)
    {
        Image img = obj.GetComponent<Image>();
        TMP_Text text = obj.GetComponentInChildren<TMP_Text>();

        if (img == null && text == null) yield break;

        Color imgColor = img != null ? img.color : Color.black;
        Color textColor = text != null ? text.color : Color.white;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            if (img != null)
            {
                imgColor.a = Mathf.Lerp(1f, 0f, t);
                img.color = imgColor;
            }

            if (text != null)
            {
                textColor.a = Mathf.Lerp(1f, 0f, t);
                text.color = textColor;
            }

            yield return null;
        }

        if (img != null) img.color = new Color(imgColor.r, imgColor.g, imgColor.b, 0f);
        if (text != null) text.color = new Color(textColor.r, textColor.g, textColor.b, 0f);
    }

    // --- Начало сцены ---
    public async void Begin()
    {
        // Fade in текста
        await WaitForCoroutine(FadeInText(nextDay, 1f));

        // Fade out родителя (черный фон + текст)
        await WaitForCoroutine(FadeOutGameObject(timaNegr, 2f));
        nextDay.text = "";

        // Запуск диалога
        ConversationManager.Instance.StartConversation(conversation);
    }

    // --- Плохой конец ---
    public async void BadEnding()
    {
        timaNegr.SetActive(true);
        await WaitForCoroutine(FadeInImage(timaNegr, 0.5f));

        // 2️⃣ Скрываем обычных персонажей
        moms.SetActive(false);

        // 3️⃣ Показываем badEnding
        badEnding.SetActive(true);
        await WaitForCoroutine(FadeOutImage(timaNegr, 0.5f));
    }


    void Start()
    {
        Begin();
    }

    // --- Методы переключения мам ---
    public void M1M2() { mom1.SetActive(false); mom2.SetActive(true); }
    public void M2M1() { mom2.SetActive(false); mom1.SetActive(true); }
    public void M1M5() { mom1.SetActive(false); mom5.SetActive(true); }
    public void M5M6() { mom5.SetActive(false); mom6.SetActive(true); }
    public void M1M4() { mom1.SetActive(false); mom4.SetActive(true); }
    public void M6M7() { mom6.SetActive(false); mom7.SetActive(true); }
}
