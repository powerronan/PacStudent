using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    [SerializeField] private GameObject cherryPrefab;
    [SerializeField] private float moveDuration = 5f;

    private Camera mainCamera;
    private Vector2 levelCenter;
    private Tweener tweener;
    private float nextSpawnTime = 0f;
    private const float spawnInterval = 10f; // Handle spawn time.

    void Start()
    {
        mainCamera = Camera.main;
        levelCenter = Vector2.zero;
        tweener = GetComponent<Tweener>();
        nextSpawnTime = Time.time + spawnInterval; // Initialize the next spawn time
    }

    void Update()
    {
        // Check if it's time to spawn a new cherry
        if (Time.time >= nextSpawnTime)
        {
            SpawnCherry();
            nextSpawnTime += spawnInterval;
        }
    }

    void SpawnCherry()
    {
        // Get camera bounds
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        
        int screenSide = Random.Range(0, 4);
        Vector2 spawnPosition;

        switch (screenSide)
        {
            case 0: // Left
                spawnPosition = new Vector2(
                    -cameraWidth / 2f - 1f, 
                    Random.Range(-cameraHeight / 2f, cameraHeight / 2f));
                break;
            case 1: // Right
                spawnPosition = new Vector2(
                    cameraWidth / 2f + 1f, 
                    Random.Range(-cameraHeight / 2f, cameraHeight / 2f));
                break;
            case 2: // Top
                spawnPosition = new Vector2(
                    Random.Range(-cameraWidth / 2f, cameraWidth / 2f),
                    cameraHeight / 2f + 1f); 
                break;
            default: // Bottom
                spawnPosition = new Vector2(
                    Random.Range(-cameraWidth / 2f, cameraWidth / 2f),
                    -cameraHeight / 2f - 1f); 
                break;
        }

        
        Vector2 endPosition = GetOppositeEdgePosition(spawnPosition);

        
        GameObject cherry = Instantiate(cherryPrefab, spawnPosition, Quaternion.identity);
        tweener.AddTween(cherry.transform, spawnPosition, endPosition, moveDuration);

        Destroy(cherry, moveDuration);
    }

    Vector2 GetOppositeEdgePosition(Vector2 startPosition)
    {
        // Calculate the direction from the center to the start position
        Vector2 direction = (startPosition - levelCenter).normalized;

        // Calculate the end position by extending the direction beyond the center
        float distance = Vector2.Distance(startPosition, levelCenter) * 2f;

        Vector2 endPosition = levelCenter - direction * distance;

        return endPosition;
    }
}