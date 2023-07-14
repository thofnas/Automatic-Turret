using System;
using UnityEngine;

public abstract class UniqueGameObject : MonoBehaviour
{
    public Guid InstanceID { get; } = Guid.NewGuid();
}