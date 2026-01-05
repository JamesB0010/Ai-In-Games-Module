using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateWorld : MonoBehaviour
{
    private TextMeshProUGUI states;

    private void Start()
    {
        this.states = GetComponent<TextMeshProUGUI>();
    }

    private void LateUpdate()
    {
        Dictionary<string, int> worldStates = World.Instance.GetWorld().States;
        this.states.text = "";

        foreach (var state in worldStates)
        {
            this.states.text += state.Key + ", " + state.Value + "\n";
        }
    }
}
