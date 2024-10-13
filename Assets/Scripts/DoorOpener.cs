using UnityEngine;
using TMPro;
using System.Collections; // Import the TextMeshPro namespace

public class ButtonDoorOpener : MonoBehaviour
{
    // The door game object that we want to open
    public GameObject door;

    // The distance from the button that the player must be within to activate the door
    public float activationDistance = 2.0f;

    // Скорость открытия двери
    public float doorOpenSpeed = 2.0f;

    // The 3D TextMeshPro component to display the hint
    public TextMeshPro hintText; // Use TextMeshPro for 3D text

    // The AudioSource component to play the door opening sound
    public AudioSource doorOpenSound; // Assign the door opening sound in the Inspector

    private bool isPlayerNearButton = false;
    private bool isDoorOpen = false;

    private PlayerController playerController;

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        // Check if the player is near the button
        float distanceToPlayer = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        if (distanceToPlayer <= activationDistance)
        {
            isPlayerNearButton = true;
            hintText.gameObject.SetActive(true); // Show hint
        }
        else
        {
            isPlayerNearButton = false;
            hintText.gameObject.SetActive(false); // Hide hint
        }

        // Check if the player presses the E key
        if (Input.GetKeyDown(KeyCode.E) && isPlayerNearButton && !isDoorOpen && playerController.key == true)
        {
            // Open the door
            OpenDoor();
            playerController.key = false;
        }
    }

    void OpenDoor()
    {
        isDoorOpen = true;
        StartCoroutine(AnimateDoorOpen());
        doorOpenSound.Play(); // Play the door opening sound
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