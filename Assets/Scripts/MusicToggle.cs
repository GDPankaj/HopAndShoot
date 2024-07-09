using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    Toggle musicToggle;

    private void OnEnable ()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        musicToggle = GetComponent<Toggle>();
        musicToggle.isOn = AudioManager.instance.IsMusicMuted();
    }
}
