using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeleterBoxScript : MonoBehaviour
{
    public GameObject chromo;
    public int timer = 30;
    public TMP_Text statsTime;
    public GameObject resultScreen;
    public TMP_Text resultText;
    public GameObject basket;

    public static class GameData
    {
        public static bool chromoResult = false;
    }

    private void Start()
    {
        InvokeRepeating(nameof(TimerCounter), 1f, 1f);
        InvokeRepeating(nameof(SpawnObject), 0.65f, 0.65f);
    }

    void TimerCounter()
    {
        timer -= 1;
        if (timer >= 0)
            statsTime.text = "Время: " + timer.ToString();
        if (timer <= 0) 
            CancelInvoke(nameof(SpawnObject));
        if (timer <= -2)
        {
            CancelInvoke(nameof(TimerCounter));
            resultScreen.SetActive(true);
            string verdict = "Вы плохо изучили материал";
            BasketScript basketScript = basket.GetComponent<BasketScript>();
            if (basketScript.points >= 23) {
                GameData.chromoResult = true;
                verdict = "Эта тема оказалась вам по силам";
            }
            if (basketScript.points == 46) 
                verdict = "Вы безупречно освоили материал";
            resultText.text = "Результат: " + basketScript.points.ToString() + "/46 очков\n\n" + verdict;
        }
    }

    void SpawnObject()
    {
        if (timer <= 0)
            return;

        Vector3 randomPos = new Vector3(
            Random.Range(-6, 6),
            15f,
            7f
        );

        Vector3 rotationPos = new Vector3(
            Random.Range(-150, -60),
            -90f,
            90f
        );

        Instantiate(chromo, randomPos, Quaternion.Euler(rotationPos));
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
