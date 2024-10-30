using KBCore.Refs;
using UnityEngine;

public abstract class BaseState
{
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
}
