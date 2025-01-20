using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void GameOver()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        foreach (Transform child in transform.transform)
        {
            child.gameObject.SetActive(true);
        }
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlayMusic("GameOver");
        animator.SetTrigger("GameOver");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(2);
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene(1);
    }
}
