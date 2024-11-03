using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    [SerializeField] private GameObject pacStudent;
    private AudioSource audioSource;
    private Animator animator;
    private ParticleSystem particleSystem;
    private Tweener tweener;

    // Full Level map grid becuase I could not figure out how to deal with the other quadrants.
    private int[,] fullLevelMap =
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7,7,2,2,2,2,2,2,2,2,2,2,2,2,1},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4,4,5,5,5,5,5,5,5,5,5,5,5,5,2},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4,4,5,3,4,4,4,3,5,3,4,4,3,5,2},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4,4,5,4,0,0,0,4,5,4,0,0,4,6,2},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3,3,5,3,4,4,4,3,5,3,4,4,3,5,2},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,2},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4,4,4,4,3,5,3,3,5,3,4,4,3,5,2},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3,3,4,4,3,5,4,4,5,3,4,4,3,5,2},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,5,5,2},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4,4,0,3,4,4,3,4,5,1,2,2,2,2,1},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3,3,0,3,4,4,3,4,5,2,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0,0,0,0,0,0,4,4,5,2,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0,0,4,4,3,0,4,4,5,2,0,0,0,0,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0,0,0,0,4,0,3,3,5,1,2,2,2,2,2},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0,0,0,0,4,0,0,0,5,0,0,0,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0,0,0,0,4,0,0,0,5,0,0,0,0,0,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0,0,0,0,4,0,3,3,5,1,2,2,2,2,2},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0,0,4,4,3,0,4,4,5,2,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0,0,0,0,0,0,4,4,5,2,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3,3,0,3,4,4,3,4,5,2,0,0,0,0,0},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4,4,0,3,4,4,3,4,5,1,2,2,2,2,1},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,5,5,2},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3,3,4,4,3,5,4,4,5,3,4,4,3,5,2},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4,4,4,4,3,5,3,3,5,3,4,4,3,5,2},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,2},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3,3,5,3,4,4,4,3,5,3,4,4,3,5,2},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4,4,5,4,0,0,0,4,5,4,0,0,4,6,2},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4,4,5,3,4,4,4,3,5,3,4,4,3,5,2},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4,4,5,5,5,5,5,5,5,5,5,5,5,5,2},
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7,7,2,2,2,2,2,2,2,2,2,2,2,2,1}
    };

    private Vector2Int gridPosition;    // PacStudent's current position in the grid
    private Vector2Int lastInput;       
    private Vector2Int currentInput;  
    private float moveDuration = 0.3f;
    private Vector3 gridOrigin = new Vector3(-14f, 15f, 0f); // Needed to be these coords to align with tilemap
    private float cellSize = 1f;

 void Start()
    {
        tweener = GetComponent<Tweener>();
        audioSource = GetComponent<AudioSource>();
        animator = pacStudent.GetComponent<Animator>();
        particleSystem = pacStudent.GetComponentInChildren<ParticleSystem>();

        gridPosition = new Vector2Int(1, 1); // Start PacStudent at top left corner.
        pacStudent.transform.position = GridToWorldPosition(gridPosition);
    }

    void Update()
    {  
        // Handle input
        if(Input.GetKeyDown(KeyCode.W))
        {
            lastInput = new Vector2Int(0, -1); // Up
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            lastInput = new Vector2Int(0, 1); // Down
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            lastInput = new Vector2Int(-1, 0); // Left
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            lastInput = new Vector2Int(1, 0); // Right
        }

        // If not currently tweening, attempt movement
        if(tweener.IsTweenComplete())
        {
            bool moved = TryMove(lastInput);

            if(!moved)
            {
                // If lastInput didn't result in movement, try currentInput
                TryMove(currentInput);
            }

            if(audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
        else 
        {
            if(!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        ToggleParticleSystem();
        UpdateAnimationParam();
    }

    bool TryMove(Vector2Int direction)
    {
        if (direction == Vector2Int.zero) return false;

        Vector2Int nextGridPosition = gridPosition + direction;

        if (IsWalkable(nextGridPosition))
        {
            // Check for teleporters
            if (IsTeleporter(nextGridPosition))
            {
                HandleTeleport(nextGridPosition);
            }
            else
            {
                // Move normally
                MoveToPosition(nextGridPosition);
            }

            currentInput = direction;
            return true;
        }

        return false;
    }

    void MoveToPosition(Vector2Int targetGridPosition)
    {
        gridPosition = targetGridPosition;
        Vector3 targetPosition = GridToWorldPosition(gridPosition);
        tweener.AddTween(pacStudent.transform, pacStudent.transform.position, targetPosition, moveDuration);
    }

    void ToggleParticleSystem()
    {
        bool isMoving = !tweener.IsTweenComplete();

        if(isMoving)
        {
            if(!particleSystem.isEmitting)
            {
                particleSystem.Play();
            }
        }
        else
        {
            if(particleSystem.isEmitting)
            {
                particleSystem.Stop();
            }
        }
    }

    void UpdateAnimationParam()
    {
        bool isMoving = !tweener.IsTweenComplete();

        animator.SetBool("IsMoving", isMoving);

        if(isMoving)
        {
            animator.SetFloat("MoveX", currentInput.x);
            animator.SetFloat("MoveY", -currentInput.y);
        }
        else
        {
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", 0f);
        }
    }

    bool IsTeleporter(Vector2Int position)
    {
        // Left Teleporter Positions
        if (position.x == 0 && (position.y == 9 || position.y == 19))
            return true;
        // Right Teleporter Positions
        if (position.x == fullLevelMap.GetLength(1) - 1 && (position.y == 9 || position.y == 19))
            return true;
        return false;
    }

    void HandleTeleport(Vector2Int position) // These don't work I can't make them work.
    {
        Vector2Int targetPosition;
        if (position.x == 0)
        {
            // Teleport to the right side
            targetPosition = new Vector2Int(fullLevelMap.GetLength(1) - 2, position.y);
        }
        else if (position.x == fullLevelMap.GetLength(1) - 1)
        {
            // Teleport to the left side
            targetPosition = new Vector2Int(1, position.y);
        }
        else
        {
            // Not a teleporter, move normally
            MoveToPosition(position);
            return;
        }

        // Update PacStudent's grid position
        gridPosition = targetPosition;

        // Move PacStudent to the new position instantly
        pacStudent.transform.position = GridToWorldPosition(gridPosition);

        // Start moving in the same direction
        Vector2Int nextGridPosition = gridPosition + currentInput;
        if (IsWalkable(nextGridPosition))
        {
            MoveToPosition(nextGridPosition);
        }
    }

    bool IsWalkable(Vector2Int position)
    {
        int x = position.x;
        int y = position.y;

        // Check if movement is within bounds
        if (y < 0 || y >= fullLevelMap.GetLength(0) || x < 0 || x >= fullLevelMap.GetLength(1))
            return false;

        int tile = fullLevelMap[y, x];
        // Walkable tiles are 0 (empty), 5 (standard pellet), and 6 (power pellet)
        return tile == 0 || tile == 5 || tile == 6;
    }

    Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        float x = gridOrigin.x + gridPos.x * cellSize + cellSize / 2f;
        float y = gridOrigin.y - gridPos.y * cellSize - cellSize / 2f;
        return new Vector3(x, y, 0f);
    }
}
