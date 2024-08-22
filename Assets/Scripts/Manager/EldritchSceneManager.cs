using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EldritchSceneManager : Singleton<EldritchSceneManager>
{
    [SerializeField] private Animator _transitionAnimator;
   /* [SerializeField] public string SceneTransitionName { get; private set; }*/
    public string SceneTransitionName => _sceneTransitionName;
    private string _sceneTransitionName;

    public void SetTransitionName(string sceneTransitionName)
    {
        _sceneTransitionName = sceneTransitionName;
    }

    public IEnumerator LoadScene(string scene)
    {
        _transitionAnimator.SetTrigger("End");
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Loading scene " + scene);
        SceneManager.LoadSceneAsync(scene);
        _transitionAnimator.SetTrigger("Start");
    }

    /*public void LoadScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }*/

}
