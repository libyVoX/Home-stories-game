using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeleterBoxScript : MonoBehaviour
{
    public GameObject chromo;
    public int timer = 30;
    public TMP_Text statsTime;

    private void Start()
    {
        InvokeRepeating(nameof(TimerCounter), 1f, 1f);
        InvokeRepeating(nameof(SpawnObject), 0.7f, 0.7f);
    }

    void TimerCounter()
    {
        timer -= 1;
        statsTime.text = "Время: " + timer.ToString();
        if (timer <= 0)
        {
            CancelInvoke(nameof(SpawnObject));
            CancelInvoke(nameof(TimerCounter));
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
