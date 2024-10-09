using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad;
    [SerializeField] private string _sceneTransitionName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EldritchSceneManager.Instance.SetTransitionName(_sceneTransitionName);
            StartCoroutine(EldritchSceneManager.Instance.LoadScene(_sceneToLoad));
            /*EldritchSceneManager.Instance.LoadScene(_sceneToLoad);*/
        }
    }
}
