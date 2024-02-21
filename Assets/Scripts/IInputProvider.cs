using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An interface used by all input method scripts
public interface IInputProvider
{
    public Vector3 MovementInput { get; }
    public bool IsAttacking { get; }
    float Horizontal { get; }
    float Vertical { get; }
    bool AttackPressed { get; }
}
