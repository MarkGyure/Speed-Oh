using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneChanger : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private EventSystem _eventSystem;

    [Space]
    [Header("Scene Transitions")]
    [SerializeField] private float _sceneLoadTime;

    [Space]
    [SerializeField] private Animator _animator;

    private const string SCENE_TRANSITION_INTRO = "Intro";
    private const string SCENE_TRANSITION_OUTRO = "Outro";

    public static SceneChanger Instance;

    //Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(int sceneID)
    {
        if (this != null)
            StartCoroutine(SceneLoadingProcess(sceneID));
    }

    private IEnumerator SceneLoadingProcess(int sceneID)
    {
        _eventSystem.enabled = false;

        _animator.SetTrigger(SCENE_TRANSITION_INTRO);
        yield return new WaitForSeconds(_sceneLoadTime);

        SceneManager.LoadScene(sceneID);

        _animator.SetTrigger(SCENE_TRANSITION_OUTRO);

        _eventSystem.enabled = true;
    }

}
