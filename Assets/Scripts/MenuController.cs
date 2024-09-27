using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour
{
    public GameObject btn;
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            btn.SetActive(true);
            Time.timeScale = 0;
        }
    }

    // Update is called once per frame
    public void HideBtn()
    {
        // Deativates the GameObject
        btn.SetActive(false);
        Time.timeScale = 1;
    }

    public void Exit(){
        SceneManager.LoadScene("Menu");
    }
}