using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDamage : MonoBehaviour
{
    protected GameObject _parentGameObject;
    protected BaseData _data;
    protected Rigidbody _rb;
    protected Scoreboard _scoreboard;

    public int currentHealth { get; set; }

    protected virtual void Awake()
    {
        // Set current health for this game object as it's stored max health
        _data = GetComponent<BaseData>();
        _parentGameObject = GameObject.FindWithTag("CreateAtRuntime");
        _rb = GetComponent<Rigidbody>();
        _scoreboard = FindObjectOfType<Scoreboard>();
    }

    protected virtual void Start()
    {
        currentHealth = _data.maxHealth;
    }

    public abstract void ChangeHealth(int value);

    public abstract void ProcessHealthState(int health);

    protected abstract void OnTriggerEnter(Collider other);
}
