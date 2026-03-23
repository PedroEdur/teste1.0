using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Input")]
    [Tooltip("Assign the 'Player/Move' action from your Input Actions asset (drag the action here).")]
    public InputActionReference moveAction;

    [Header("Movement")]
    [Tooltip("Force multiplier applied every FixedUpdate.")]
    public float moveSpeed = 10f;
    [Tooltip("Ignore small inputs below this magnitude.")]
    public float deadzone = 0.1f;
    [Tooltip("If true, clamp the horizontal velocity to maxSpeed.")]
    public bool clampMaxSpeed = true;
    [Tooltip("Maximum horizontal speed when clamp is enabled.")]
    public float maxSpeed = 8f;

    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
            _rb = gameObject.AddComponent<Rigidbody>();
        // Ensure sensible defaults for a rolling ball
        // Use the newer damping/velocity properties
        _rb.angularDamping = 0.05f;
        _rb.linearDamping = 0.1f;
        _rb.mass = 1f;
    }

    void OnEnable()
    {
        if (moveAction != null && moveAction.action != null)
            moveAction.action.Enable();
    }

    void OnDisable()
    {
        if (moveAction != null && moveAction.action != null)
            moveAction.action.Disable();
    }

    void FixedUpdate()
    {
        // Read keyboard WASD input only (no jump)
        var kb = Keyboard.current;
        if (kb == null)
            return; // no keyboard available

        float x = 0f, y = 0f;
        if (kb.aKey.isPressed) x -= 1f;
        if (kb.dKey.isPressed) x += 1f;
        if (kb.wKey.isPressed) y += 1f;
        if (kb.sKey.isPressed) y -= 1f;

        Vector2 input = new Vector2(x, y);

        // Apply deadzone
        if (input.sqrMagnitude < deadzone * deadzone)
            return;

        Vector3 move = new Vector3(input.x, 0f, input.y);

        // Apply force in world-space (not camera-relative)
        _rb.AddForce(move * moveSpeed, ForceMode.Force);

        // Optional: clamp horizontal speed so the ball doesn't accelerate forever
        if (clampMaxSpeed)
        {
            Vector3 horizontalVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            if (horizontalVel.magnitude > maxSpeed)
            {
                Vector3 clamped = horizontalVel.normalized * maxSpeed;
                _rb.linearVelocity = new Vector3(clamped.x, _rb.linearVelocity.y, clamped.z);
            }
        }
    }

    // Public helper to adjust speed at runtime
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}

