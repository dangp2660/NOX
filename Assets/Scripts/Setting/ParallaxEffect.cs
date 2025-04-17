using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera Camera;
    [SerializeField] private Transform follwerTarget;
    private Vector2 startPositions;
    private float startZ;
    //position of camera this frame - last frame
    private Vector2 camMoveScinceStart => ( Vector2)Camera.transform.position - startPositions;

    private float distanceFromTarget => transform.position.z - follwerTarget.transform.position.z;

    private float clippingLane => (Camera.transform.position.z + (distanceFromTarget > 0 ? Camera.farClipPlane : Camera.nearClipPlane));
    private float ParallaxFactor => Mathf.Abs(distanceFromTarget) / clippingLane;
    void Start()
    {
        startPositions = transform.position;
        startZ = transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = startPositions + camMoveScinceStart * ParallaxFactor;

        transform.position = new Vector3(newPosition.x, newPosition.y, startZ);
    }
}
