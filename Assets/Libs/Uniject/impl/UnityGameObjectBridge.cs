using System;
using Uniject.Impl;
using UnityEngine;
using System.Collections;

public class UnityGameObjectBridge : MonoBehaviour
{
    private void Awake()
    {
    }

    public void OnDestroy()
    {
        wrapping.Destroy();
    }

    public void Update()
    {
        wrapping.Update();
    }

    public void OnCollisionEnter(Collision c)
    {
        UnityGameObjectBridge other = c.gameObject.GetComponent<UnityGameObjectBridge>();
        if (null != other)
        {
            Uniject.Collision testableCollision =
                new Uniject.Collision(c.relativeVelocity,
                    other.wrapping.transform,
                    other.wrapping,
                    c.contacts);
            wrapping.OnCollisionEnter(testableCollision);
        }
    }

    public void OnTriggerEnter(Collider c)
    {
        wrapping.OnTriggerEnter(c);
    }

    public UnityGameObject wrapping;

    internal void InvokeRepeating(float inTime, float repeatTime)
    {
        InvokeRepeating("InternalInvoke", inTime, repeatTime);
    }

    internal void CancelInvoke()
    {
        CancelInvoke("InternalInvoke");
    }

    private void InternalInvoke()
    {
        for (int i = 0; i < wrapping.Repeatables.Count; i++)
        {
            wrapping.Repeatables[i].Repeate();
        }
    }

    public bool IsRendererVisibleInCamera { private set; get; }

    private void OnBecameVisible()
    {
        IsRendererVisibleInCamera = true;
    }

    private void OnBecameInvisible()
    {
        IsRendererVisibleInCamera = false;
    }

    private void OnMouseDown()
    {
        wrapping.OnMouseDown();
    }
}