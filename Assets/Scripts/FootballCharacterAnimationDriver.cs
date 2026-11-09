using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animator bridge for imported, licensed humanoid footballer prefabs.
/// Assign an Animator with Speed/IsSprinting parameters and optional Kick/Tackle/Celebrate triggers.
/// The procedural placeholder remains active when no Animator is assigned.
/// </summary>
[DisallowMultipleComponent]
public sealed class FootballCharacterAnimationDriver : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string speedParameter = "Speed";
    [SerializeField] private string sprintParameter = "IsSprinting";
    [SerializeField] private string kickTrigger = "Kick";
    [SerializeField] private string tackleTrigger = "Tackle";
    [SerializeField] private string celebrateTrigger = "Celebrate";
    [SerializeField] private string passTrigger = "Pass";
    [SerializeField] private string headerTrigger = "Header";
    [SerializeField] private string diveTrigger = "Dive";
    [SerializeField] private string saveTrigger = "Save";
    [SerializeField] private string dribbleTrigger = "Dribble";
    [SerializeField] private string throwInTrigger = "ThrowIn";

    private readonly HashSet<int> floatParameters = new HashSet<int>();
    private readonly HashSet<int> boolParameters = new HashSet<int>();
    private readonly HashSet<int> triggerParameters = new HashSet<int>();
    private Vector3 previousPosition;

    public void Bind(Animator target)
    {
        animator = target;
        CacheParameters();
    }

    private void Awake()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
        CacheParameters();
        previousPosition = transform.position;
    }

    private void CacheParameters()
    {
        floatParameters.Clear();
        boolParameters.Clear();
        triggerParameters.Clear();
        if (animator == null) return;
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            int hash = parameter.nameHash;
            if (parameter.type == AnimatorControllerParameterType.Float) floatParameters.Add(hash);
            else if (parameter.type == AnimatorControllerParameterType.Bool) boolParameters.Add(hash);
            else if (parameter.type == AnimatorControllerParameterType.Trigger) triggerParameters.Add(hash);
        }
    }

    private void Update()
    {
        if (animator == null) return;
        float speed = (transform.position - previousPosition).magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
        previousPosition = transform.position;
        int speedHash = Animator.StringToHash(speedParameter);
        if (floatParameters.Contains(speedHash)) animator.SetFloat(speedHash, speed, 0.12f, Time.deltaTime);
        int sprintHash = Animator.StringToHash(sprintParameter);
        if (boolParameters.Contains(sprintHash)) animator.SetBool(sprintHash, speed > 5.5f);
    }

    public void PlayKick() { Trigger(kickTrigger); }
    public void PlayPass() { Trigger(passTrigger); }
    public void PlayHeader() { Trigger(headerTrigger); }
    public void PlayDive() { Trigger(diveTrigger); }
    public void PlaySave() { Trigger(saveTrigger); }
    public void PlayDribble() { Trigger(dribbleTrigger); }
    public void PlayThrowIn() { Trigger(throwInTrigger); }
    public void PlayTackle() { Trigger(tackleTrigger); }
    public void PlayCelebrate() { Trigger(celebrateTrigger); }

    private void Trigger(string parameter)
    {
        if (animator == null) return;
        int hash = Animator.StringToHash(parameter);
        if (triggerParameters.Contains(hash)) animator.SetTrigger(hash);
    }
}
