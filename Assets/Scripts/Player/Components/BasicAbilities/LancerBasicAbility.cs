using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerBasicAbility : MonoBehaviour
{
    // Constants.
    private const float PROJECTILE_Y_OFFSET = 0.4f;
    private const float DAMAGE = 0.75f;

    // Private member variables.
    private PlayerBehavior parent;
    private CooldownBarBehavior cooldownBar;
    private string axis;

    private bool retrievedAxis;

    // Start is called before the first frame update
    void Start()
    {
        cooldownBar = parent.GetBasicAbilityCooldownBar();
        retrievedAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        RetrieveAxis();
        // Check if the ability is being activated.
        if (retrievedAxis && Input.GetAxis(axis) == 1.0f)
        {
            if (cooldownBar.ReadyToFire())
            {
                // Launch missile.
                Vector3 startPos = transform.position;
                startPos.y += PROJECTILE_Y_OFFSET;
                GameObject projectile = Instantiate(Resources.Load("Prefabs/LancerMissile") as GameObject,
                                                    startPos,
                                                    transform.rotation);

                LancerMissileBehavior missileBehavior = projectile.GetComponent<LancerMissileBehavior>();
                missileBehavior.SetParent(parent);

                // Trigger the reload.
                cooldownBar.TriggerCooldown();
            }
        }
    }

    public void SetParent(PlayerBehavior parent)
    {
        this.parent = parent;
    }

    // Private helpers.
    private void RetrieveAxis()
    {
        if (!retrievedAxis)
        {
            axis = parent.GetBasicAbilityAxis();
            if (axis != null && axis != "")
            {
                retrievedAxis = true;
            }
        }
    }
}
