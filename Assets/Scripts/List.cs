using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
public class List : MonoBehaviour
{
    //* Начать вызывать после обновления первого кадра
    public TextMeshPro hintText;
    public float activationDistance = 3.0f;
    private bool isPlayerNearList = false;
    private PlayerController playerController;

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        hintText.gameObject.SetActive(false); //* Скрывать подсказку по дефолту
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        if (distanceToPlayer <= activationDistance)
        {
            isPlayerNearList = true;
            hintText.gameObject.SetActive(true); //* Показать подсказку 
            hintText.text = "F";
        }
        else
        {
            isPlayerNearList = false;
            hintText.gameObject.SetActive(false); //* Скрыть подсказку
        }

        //* Проверка рядом ли ключ и если да то проигрывать анимацию поднятия и удалять ключ
        if (Input.GetKeyDown(KeyCode.F) && playerController.isGrounded == true && isPlayerNearList)
        {
            playerController.Gather();
            playerController.list = playerController.list + 1;
            gameObject.SetActive(false);
        }
        else if (Input.GetKeyUp(KeyCode.F) && playerController.isGrounded == true)
        {
            playerController.speed = 3.5f;
        }
    }
}