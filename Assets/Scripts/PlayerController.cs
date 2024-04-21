using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    #region Fields

    public float Speed = 5f;
    public float RotationSpeed = 720f; // Degrees per second

    private IInputProvider _inputProvider;

    private void Awake()
    {
        _inputProvider = GetComponent<IInputProvider>();
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            HandleMovement();
        }
    }

    void HandleMovement()
    {
        //To avoid sending data constantly, a check to see if the input has changed is implemeneted
        if (_inputProvider.MovementInput.magnitude > 0)
        {
            // The server is sent the movement and information about the players collision
            // I have opted for RayCast based collisions as they are more performant and responsive than Physics based systems in Networking
            // This controller also uses Server-authoritative movement
            // The transform is then syncronized on all observers using the NetworkTransform component
            CmdMove(_inputProvider.MovementInput, Physics.Raycast(transform.position, transform.forward, 1));
        }

    }

    #endregion

    [Command]
    void CmdMove(Vector3 movementInput, bool isBlocked)
    {
        Vector3 direction = movementInput.normalized;
        float distance = Speed * Time.deltaTime;

        if (!isBlocked)
        {
            // No collision detected, safe to move
            transform.Translate(direction * distance, Space.World);
        }
        else
        {
            // Handle collision
            // For simplicity, we just don't move the player in this example,
            // but you could implement sliding along the wall or stopping before the obstacle.
        }

        //This rotates the player according to movement input
        Quaternion toRotation = Quaternion.LookRotation(movementInput, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, RotationSpeed * Time.deltaTime);
    }
}