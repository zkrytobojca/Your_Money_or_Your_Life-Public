using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindingMovementDropdown : MonoBehaviour
{
    [Header("Input Action")]
    [SerializeField] private InputActionReference inputAction;

    private TMP_Dropdown _dropdown;

    private void Awake()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
    }

    private void OnEnable()
    {
        InputManager.rebindingsReset += ResetDropdownState;

        ResetDropdownState();
    }

    private void OnDisable()
    {
        InputManager.rebindingsReset -= ResetDropdownState;
    }

    private void ResetDropdownState()
    {
        string rebinds = inputAction.action.SaveBindingOverridesAsJson();

        if (string.IsNullOrEmpty(rebinds)) _dropdown.value = 0;
        else _dropdown.value = 1;
    }

    public void OnMovementChange(int chosenId)
    {
        switch (chosenId)
        {
            case 0:
                inputAction.action.RemoveAllBindingOverrides();
                break;
            case 1:
                inputAction.action.RemoveAllBindingOverrides();

                int bindingIndex = inputAction.action.GetBindingIndexForControl(inputAction.action.controls[0]);
                while (inputAction.action.bindings[bindingIndex].isPartOfComposite && bindingIndex > 0) bindingIndex--;
                if (inputAction.action.bindings[bindingIndex].isComposite)
                {
                    for(int i = bindingIndex + 1; i < inputAction.action.bindings.Count && inputAction.action.bindings[i].isPartOfComposite; i++)
                    {
                        RebingToArrow(i);
                    }
                }
                break;
        }
    }

    private void RebingToArrow(int bindingIndex)
    {
        string arrowPath = string.Empty;
        switch(bindingIndex)
        {
            case 1:
                arrowPath = "<Keyboard>/upArrow";
                break;
            case 2:
                arrowPath = "<Keyboard>/downArrow";
                break;
            case 3:
                arrowPath = "<Keyboard>/leftArrow";
                break;
            case 4:
                arrowPath = "<Keyboard>/rightArrow";
                break;
        }

        InputActionRebindingExtensions.ApplyBindingOverride(
                            inputAction.action,
                            bindingIndex,
                            arrowPath
                            );
    }
}
