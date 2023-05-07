using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RebindingButton : MonoBehaviour
{
    [Header("Input Action")]
    [SerializeField] private InputActionReference inputAction;

    [Header("Visual Aspects")]
    [SerializeField] private string waitingText = "Waiting...";
    [SerializeField] private string duplicateText = "Binding is taken...";

    private TMP_Text _text;
    private Button _button;

    private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _text = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        ResetButtonState();
    }

    private void OnEnable()
    {
        InputManager.rebindingsReset += ResetButtonState;

        ResetButtonState();
    }

    private void OnDisable()
    {
        InputManager.rebindingsReset -= ResetButtonState;

        if (_rebindingOperation != null)
        {
            RebindCanceled();
        }
    }

    public void StartRebinding()
    {
        _button.interactable = false;
        _text.text = waitingText;

        _rebindingOperation = inputAction.action.PerformInteractiveRebinding()
            .WithCancelingThrough("<Keyboard>/escape")
            .WithControlsExcluding("<Mouse>/press")
            .WithControlsExcluding("<Mouse>/leftButton")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .OnCancel(operation => RebindCanceled())
            .Start();
    }

    private void RebindComplete()
    {
        _rebindingOperation.Dispose();

        int bingingIndex = inputAction.action.GetBindingIndexForControl(inputAction.action.controls[0]);

        if(CheckForDuplicateBinding(bingingIndex))
        {
            inputAction.action.RemoveBindingOverride(bingingIndex);

            _text.text = duplicateText;
            StartCoroutine(ResetButtonStateWithDelay(2f));
        }
        else
        {
            ResetButtonState();
        }
    }

    private bool CheckForDuplicateBinding(int bingingIndex)
    {
        InputBinding newBinding = inputAction.action.bindings[bingingIndex];
        foreach(InputBinding inputBinding in InputManager.instance.inputActions.bindings)
        {
            if(inputBinding.action == newBinding.action)
            {
                continue;
            }
            if(inputBinding.effectivePath == newBinding.effectivePath)
            {
                Debug.Log("Duplicate binding found: " + newBinding.effectivePath);
                return true;
            }
        }
        return false;
    }

    IEnumerator ResetButtonStateWithDelay(float delay = 2f)
    {
        yield return new WaitForSeconds(delay);
        ResetButtonState();
    }

    private void ResetButtonState()
    {
        int bingingIndex = inputAction.action.GetBindingIndexForControl(inputAction.action.controls[0]);
        _button.interactable = true;
        _text.text = InputControlPath.ToHumanReadableString(
            inputAction.action.bindings[bingingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    private void RebindCanceled()
    {
        _rebindingOperation.Dispose();
        ResetButtonState();
    }
}
