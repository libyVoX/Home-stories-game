using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScripts : MonoBehaviour
{
    public void ToStory1() {
        SceneManager.LoadScene("Episode1");
    }
}