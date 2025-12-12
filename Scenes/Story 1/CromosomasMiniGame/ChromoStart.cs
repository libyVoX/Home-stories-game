using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChromoStart : MonoBehaviour
{
    public GameObject basket;
    public GameObject floor;
    public GameObject stats;
    public int chromoResult;
    public void StartChromoGame() {
        BasketScript BasketScript = basket.GetComponent<BasketScript>();
        DeleterBoxScript deleter = floor.GetComponent<DeleterBoxScript>();
        BasketScript.enabled = true;
        deleter.enabled = true;
        gameObject.SetActive(false);
        stats.SetActive(true);
    }

    public void GoToEpisode2() {
        SceneManager.LoadScene("Episode2");
    }
}
