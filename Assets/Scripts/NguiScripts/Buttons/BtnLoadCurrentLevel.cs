using UnityEngine;
using System.Collections;

public class BtnLoadCurrentLevel : MonoBehaviour
{
    private void Start()
    {
    }

    private void OnClick()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }
}
