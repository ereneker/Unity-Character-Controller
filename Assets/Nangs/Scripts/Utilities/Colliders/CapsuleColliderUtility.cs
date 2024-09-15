using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CapsuleColliderUtility
{
    public CapsuleColliderData capsuleColliderData { get; private set; }
    [field: SerializeField] public DefaultColliderData defaultColliderData { get; private set; }
    [field: SerializeField] public SlopeData slopeData { get; private set; }

    public void Initialize(GameObject colliderObject)
    {
        if (capsuleColliderData != null)
        {
            return;
        }

        capsuleColliderData = new CapsuleColliderData();
        capsuleColliderData.Initialize(colliderObject);
    }

    public void CalculateCapsuleColliderDimensions()
    {
        SetCapsuleColliderHeight(defaultColliderData.Height * (1f - slopeData.stepHeightPercentage));
        SetCapsuleColliderRadius(defaultColliderData.Radius);
        RecalculateCapsuleColliderCenter();
        LimitCapsuleColliderRadius();

        capsuleColliderData.UpdateColliderData();
    }

    public void SetCapsuleColliderHeight(float height)
    {
        capsuleColliderData.capsuleCollider.height = height;
    }

    public void SetCapsuleColliderRadius(float radius)
    {
        capsuleColliderData.capsuleCollider.radius = radius;
    }

    public void RecalculateCapsuleColliderCenter()
    {
        float colliderHeightDifference = defaultColliderData.Height - capsuleColliderData.capsuleCollider.height;

        Vector3 newColliderCenter = new Vector3(0f, defaultColliderData.CenterY + (colliderHeightDifference / 2f), 0f);

        capsuleColliderData.capsuleCollider.center = newColliderCenter;
    }

    private void LimitCapsuleColliderRadius()
    {
        float halfColliderHeight = capsuleColliderData.capsuleCollider.height / 2f;

        if (halfColliderHeight < capsuleColliderData.capsuleCollider.radius)
        {
            SetCapsuleColliderRadius(halfColliderHeight);
        }
    }
}