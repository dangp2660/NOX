using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerSwitch : MonoBehaviour
{
    [SerializeField] public GameObject defaultForm;
    [SerializeField] public GameObject darkForm;
    [SerializeField] private CinemachineVirtualCamera camera;
    private PlayerMovement defaultMove;
    private PlayerMovement darkMove;
    private void Start()
    {
        defaultMove = defaultForm.GetComponent<PlayerMovement>();
        darkMove = darkForm.GetComponent<PlayerMovement>();
    }

    public bool isDefault = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Vector3 currentPosition = isDefault ? defaultForm.transform.position : darkForm.transform.position;

            isDefault = !isDefault;

            defaultForm.SetActive(isDefault);
            darkForm.SetActive(!isDefault);

            if (isDefault)
            {
                currentPosition.y += -0.1900992f;
                defaultForm.transform.position = currentPosition;
                defaultMove.CopyStateFrom();
            }
            else
            {
                darkForm.transform.position = currentPosition;
                darkMove.CopyStateFrom();
            }
            camera.Follow = isDefault? defaultForm.transform : darkForm.transform;  
        }
    }
}
