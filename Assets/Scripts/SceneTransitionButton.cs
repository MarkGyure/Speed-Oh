using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionButton : MonoBehaviour
{
    public void StartSceneTransition(int id)
    {
        SceneChanger.Instance.LoadScene(id);
    }
}
