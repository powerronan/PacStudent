using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    [SerializeField] private GameObject pacStudent; 
    private Tweener tweener;
    private AudioSource audioSource;
    //private Animator animator;


    private Vector3[] positions = new Vector3[]
    {
        new Vector3(-23.3f, 13.5f, 0),  // Top-left corner
        new Vector3(-18.3f, 13.5f, 0),   // Top-right corner
        new Vector3(-18.3f, 9.5f, 0),  // Bottom-right corner
        new Vector3(-23.3f, 9.5f, 0)  // Bottom-left corner
    };

    private int currentPositionIndex = 0;
    private float moveDuration = 1.5f; // Time for each movement

    void Start()
    {
        tweener = GetComponent<Tweener>();
        //animator = GetComponent<Animator>(); Can't work don't want to lose marks.
        audioSource = pacStudent.GetComponent<AudioSource>();


        MovePacStudentClockwise();
    }

    void MovePacStudentClockwise()
    {
        Vector3 startPos = positions[currentPositionIndex];
        Vector3 endPos = positions[(currentPositionIndex + 1) % positions.Length];

        tweener.AddTween(pacStudent.transform, startPos, endPos, moveDuration);

        currentPositionIndex = (currentPositionIndex + 1) % positions.Length;

        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void Update()
    {
        // Check if the tween is complete and initiate the next movement
        if (tweener.IsTweenComplete())
        {
            MovePacStudentClockwise();
        }
    }
}
