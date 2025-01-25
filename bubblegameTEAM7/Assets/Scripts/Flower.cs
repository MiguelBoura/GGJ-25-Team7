using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public enum FlowerState
    {
        Safe,
        Warning,
        Danger
    }

    [Header("Timings")]
    [SerializeField] private float safeDuration = 3f;
    [SerializeField] private float warningDuration = 2f;
    [SerializeField] private float dangerDuration = 2f;

    [Header("Layers")]
    [SerializeField] private string safeLayer = "SafeObject";
    [SerializeField] private string dangerLayer = "DangerObject";

    [Header("States")]
    private bool isSafe;
    private bool isWarning;
    private bool isDanger;

    private FlowerState currentState = FlowerState.Safe;
    private float stateTimer;

    public Animator anim;

    private void Start()
    {
        StartCoroutine(flowerAI());
    }
    private IEnumerator flowerAI()
    {
            stateTimer -= Time.deltaTime;

            if (stateTimer <= 0)
            {
                switch (currentState)
                {
                    case FlowerState.Warning:
                        isSafe = false;
                        isWarning = true;
                        isDanger = false;
                        stateTimer = warningDuration;
                    stateTimer -= Time.deltaTime;
                    if (stateTimer <= 0)
                        {
                            currentState = FlowerState.Danger;
                        }
                        break;
                    case FlowerState.Danger:
                        isSafe = false;
                        isWarning = false;
                        isDanger = true;
                        stateTimer = dangerDuration;
                        gameObject.layer = LayerMask.NameToLayer(dangerLayer);
                    stateTimer -= Time.deltaTime;
                    if (stateTimer <= 0)
                        {
                            currentState = FlowerState.Safe;
                        }
                        break;

                    case FlowerState.Safe:
                        stateTimer = safeDuration;
                        isSafe = true;
                        isWarning = false;
                        isDanger = false;
                        gameObject.layer = LayerMask.NameToLayer(safeLayer);
                    stateTimer -= Time.deltaTime;
                    if (stateTimer <= 0)
                        {
                            currentState = FlowerState.Warning;
                        }
                        break;
                }
            }

        yield return null;
        }
}
