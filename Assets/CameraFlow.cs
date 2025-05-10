
using Cinemachine;
using UnityEngine;

public class CameraFlow : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;
    private Transform playerTransform;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            camera.Follow = playerTransform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player object has the 'Player' tag.");
        }
    }

    private void LateUpdate()
    {
        if (playerTransform != null && camera.Follow != playerTransform)
        {
            camera.Follow = playerTransform;
        }
    }
}
