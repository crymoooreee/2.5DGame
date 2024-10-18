using UnityEngine;
using TMPro;
using System.Collections;

public class ButtonDoorOpener : MonoBehaviour
{
    public GameObject door;
    public float activationDistance = 2.0f; //* Дистанция для активации рычага и подсказки
    public float doorOpenSpeed = 2.0f; //* Скорость открывания двери
    public TextMeshPro hintText;
    public AudioSource doorOpenSound;

    private bool isPlayerNearButton = false;
    private bool isDoorOpen = false;

    private PlayerController playerController;

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        //* Проверка рядом ли игрок
        float distanceToPlayer = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        if (distanceToPlayer <= activationDistance)
        {
            isPlayerNearButton = true;
            hintText.gameObject.SetActive(true); //* Показать подсказку
        }
        else
        {
            isPlayerNearButton = false;
            hintText.gameObject.SetActive(false); //* Скрыть подсказку
        }

        if (Input.GetKeyDown(KeyCode.E) && isPlayerNearButton && !isDoorOpen && playerController.key == true )
        {
            OpenDoor();
            playerController.key = false; //* Обнулить состояние ключа
            playerController.list = 0;    //* Обнулить количество листов
        }
        if (Input.GetKeyDown(KeyCode.E) && isPlayerNearButton && !isDoorOpen && playerController.list >= 3 )
        {
            OpenDoor();
            playerController.key = false; //* Обнулить состояние ключа
            playerController.list = 0;    //* Обнулить количество листов
        }
    }

    void OpenDoor()
    {
        isDoorOpen = true;
        StartCoroutine(AnimateDoorOpen());
        doorOpenSound.Play();
    }

    IEnumerator AnimateDoorOpen()
    {
        float elapsedTime = 0;
        Quaternion startRotation = door.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, 90, 0);

        while (elapsedTime < doorOpenSpeed)
        {
            door.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / doorOpenSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        door.transform.rotation = endRotation;
    }
}