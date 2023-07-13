using System;
using UnityEngine;

public abstract class UniqueGameObject : MonoBehaviour
{
    protected Guid InstanceID { get; } = Guid.NewGuid();
}