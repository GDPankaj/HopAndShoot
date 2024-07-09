using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] Transform targetPos;
    [SerializeField] float speed = 10f;
    [SerializeField]bool isLastLevel;
    bool _playerReached = false;


    private void Update()
    {
        if (_playerReached)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPos.position, step);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(StartNextLevel(other.gameObject));
        }
    }

    IEnumerator StartNextLevel(GameObject gameObj)
    {
        AudioManager.instance?.StopMusic();
        AudioManager.instance?.PlaySFX("ReachedEnd");
        yield return new WaitForSeconds(.35f);
        gameObj.SetActive(false);
        _playerReached = true;
        yield return new WaitForSeconds(3f);
        GameManager.instance.OpenEndLevelMenu(isLastLevel);
    }
}
