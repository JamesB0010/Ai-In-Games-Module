using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationRigFixer : MonoBehaviour
{
    private RigBuilder rigBuilder;
    void Start()
    {
        this.rigBuilder = GetComponent<RigBuilder>();

        StartCoroutine(nameof(enableRigBuilder));
    }

    IEnumerator enableRigBuilder()
    {
        yield return new WaitForSeconds(0);

        this.rigBuilder.enabled = true;
    }
}
