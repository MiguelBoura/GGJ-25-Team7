using System.Collections;
using UnityEngine;

// Custom ReadOnly attribute
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ReadOnlyAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false; // Disable editing
        EditorGUI.PropertyField(position, property, label);
        GUI.enabled = true; // Re-enable editing
    }
}
#endif

public class Flower : MonoBehaviour
{
    public enum FlowerState
    {
        Safe,
        Warning,
        Danger,
    }

    [Header("Timings")]
    [SerializeField] private float safeDuration = 3f;
    [SerializeField] private float warningDuration = 2f;
    [SerializeField] private float dangerDuration = 2f;

    [Header("Layers")]
    [SerializeField] private string safeLayer = "SafeObject";
    [SerializeField] private string dangerLayer = "DangerObject";

    [Header("Animator")]
    public Animator anim;

    [ReadOnly]
    public string currentStateDisplay; // Display state and timer in Inspector

    private FlowerState currentState;
    private float stateTimer;

    private void Start()
    {
        // Initialize the flower state
        currentState = FlowerState.Safe;
        stateTimer = safeDuration;
        gameObject.layer = LayerMask.NameToLayer(safeLayer);

        // Start the state machine
        StartCoroutine(FlowerAI());
    }

    private IEnumerator FlowerAI()
    {
        while (true) // Infinite loop for state transitions
        {
            // Decrement the timer
            stateTimer -= Time.deltaTime;

            // Update the display string
            UpdateStateDisplay();

            // Check if the state should change
            if (stateTimer <= 0)
            {
                switch (currentState)
                {
                    case FlowerState.Safe:
                        TransitionToState(FlowerState.Warning, warningDuration);
                        break;
                    case FlowerState.Warning:
                        TransitionToState(FlowerState.Danger, dangerDuration);
                        gameObject.layer = LayerMask.NameToLayer(dangerLayer);
                        break;
                    case FlowerState.Danger:
                        TransitionToState(FlowerState.Safe, safeDuration);
                        gameObject.layer = LayerMask.NameToLayer(safeLayer);
                        break;
                }
            }

            // Yield execution until the next frame
            yield return null;
        }
    }

    private void TransitionToState(FlowerState newState, float duration)
    {
        currentState = newState;
        stateTimer = duration;

        // Optionally, trigger animations
        if (anim != null)
        {
            anim.SetTrigger(newState.ToString());
        }
    }

    private void UpdateStateDisplay()
    {
        currentStateDisplay = $"State: {currentState}, Timer: {stateTimer:F2} seconds";
    }
}
