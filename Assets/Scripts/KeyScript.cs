using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
public class KeyScript : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshPro hintText;
    public float activationDistance = 3.0f;
    private bool isPlayerNearKey = false;
    private PlayerController playerController;

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        hintText.gameObject.SetActive(false); // hide hint text by default
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        if (distanceToPlayer <= activationDistance)
        {
            isPlayerNearKey = true;
            hintText.gameObject.SetActive(true); // show hint text when player is near
            hintText.text = "F";
        }
        else
        {
            isPlayerNearKey = false;
            hintText.gameObject.SetActive(false); // hide hint text when player is far
        }

        if (Input.GetKeyDown(KeyCode.F) && playerController.isGrounded == true && isPlayerNearKey)
        {
            playerController.Gather();
            playerController.key = true;
            gameObject.SetActive(false);
        }
        else if (Input.GetKeyUp(KeyCode.F) && playerController.isGrounded == true)
        {
            playerController.speed = 3.5f;
        }
    }
}