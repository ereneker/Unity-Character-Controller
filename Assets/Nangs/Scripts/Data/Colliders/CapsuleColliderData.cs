using UnityEngine;

public class CapsuleColliderData
{
    public CapsuleCollider capsuleCollider { get; private set; }
    public Vector3 colliderCenterInLocalSpace { get; private set; }
    public Vector3 colliderVerticalExtents { get; private set; }

    public void Initialize(GameObject colliderObject)
    {
        if (capsuleCollider != null)
        {
            return;
        }

        capsuleCollider = colliderObject.GetComponent<CapsuleCollider>();
        
        UpdateColliderData();
    }

    public void UpdateColliderData()
    {
        colliderCenterInLocalSpace = capsuleCollider.center;

        colliderVerticalExtents = new Vector3(0, capsuleCollider.bounds.extents.y, 0);
    }
}
