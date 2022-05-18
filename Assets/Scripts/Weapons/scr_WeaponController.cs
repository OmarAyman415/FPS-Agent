using UnityEngine;
using static scr_Models;
using System.Collections.Generic;
using System.Linq;

public class scr_WeaponController : MonoBehaviour
{
    [Header("Weapon Attributes")]
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;


    private scr_CharacterController characterController;

    [Header("References")]
    public Animator weaponAnimator;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public GameObject Crosshair;

    [Header("Settings")]
    public WeaponSettingsModel settings;

    bool isInitialised;

    Vector3 newWeaponRotation;
    Vector3 newWeaponRotationVelocity;

    Vector3 targetWeaponRotation;
    Vector3 targetWeaponRotationVelocity;


    Vector3 newWeaponMovementRotation;
    Vector3 newWeaponMovementRotationVelocity;

    Vector3 targetWeaponMovementRotation;
    Vector3 targetWeaponMovementRotationVelocity;

    private bool isGroundedTrigger;

    public float fallingDelay;

    [Header("Weapon Sway")]
    public Transform weaponSwayObject;

    public float swayAmountA = 1;
    public float swayAmountB = 2;
    public float swayScale = 600;
    public float swayLerpSpeed = 14;

    private float swayTime;
    private Vector3 swayPosition;


    [Header("Sights")]
    public Transform sightTarget;
    public float sightOffset;
    public float aimingInTime;
    private Vector3 weaponSwayPosition;
    private Vector3 weaponSwayPositionVelocity;

    [Header("Shooting")]
    public float shootCooldown = 0.5f;
    private float timeStamp = 0.5f; 
    public List<WeaponFireType> allowedFireTypes;
    public WeaponFireType currentFireType;
    [HideInInspector]
    public bool isShooting;

    [HideInInspector]
    public bool isAimingIn;

    #region - Start -
    private void Start() {
        newWeaponRotation = transform.localRotation.eulerAngles;

        currentFireType = allowedFireTypes.First();
    }

    #endregion

    #region - initialization -
    public void Initialise(scr_CharacterController CharacterController)
    {
        characterController = CharacterController;
        isInitialised = true;
    }

    #endregion

    #region - Update -

    private void Update()
    {
        if(!scr_EnemyManager.instance.isPaused)
        {
            if (!isInitialised)
            {
                return;
            }

            //Prevent TimeStamp from overflow
            if (timeStamp > 6.3f)
            {
                timeStamp = 0.5f;
            }

            timeStamp += Time.deltaTime;

            CalculateWeaponRotation();
            SetWeaponAnimations();
            CalculateWeaponSway();
            CalculateAimingIn();
            CalculateShooting();
        }
    }

    #endregion

    #region  - shooting -

    private void Shoot()
    {
        if (!characterController.isSprinting)
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawn);

            muzzleFlash.Play();

            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);

                scr_EnemyController target = hit.transform.GetComponent<scr_EnemyController>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }

                if(hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }

                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);

            }
        }
        // load bullet settings
    }

    
    private void CalculateShooting()
    {
        if(isShooting)
        {   
            if(timeStamp >= shootCooldown){
                Shoot();
                timeStamp = 0;
            }
            

            if(currentFireType == WeaponFireType.SemiAuto)
            {
                isShooting = false;
            }
        }
    }

    #endregion

    #region - Aiming -

    private void CalculateAimingIn()
    {
        var targetPosition = transform.position;

        if (isAimingIn)
        {
            Crosshair.SetActive(false);
            targetPosition = characterController.cameraHolder.transform.position + (weaponSwayObject.transform.position - sightTarget.position) + (characterController.cameraHolder.transform.forward * sightOffset);
        }
        else
        {
            Crosshair.SetActive(true);
        }

        weaponSwayPosition = weaponSwayObject.transform.position;
        weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPositionVelocity, aimingInTime);
        weaponSwayObject.transform.position = weaponSwayPosition + swayPosition;
    }

    #endregion

    #region - Jumping -

    public void TriggerJump()
    {
        isGroundedTrigger = false;
        weaponAnimator.SetTrigger("Jump");
    }

    #endregion

    #region - Weapon Rotation -

    private void CalculateWeaponRotation()
    {
        targetWeaponRotation.y += (isAimingIn ? settings.SwayAmount / 3 : settings.SwayAmount) * (settings.SwayXInverted ? -characterController.input_View.x : characterController.input_View.x) * Time.deltaTime;
        targetWeaponRotation.x += (isAimingIn ? settings.SwayAmount / 3 : settings.SwayAmount) * (settings.SwayYInverted ? characterController.input_View.y : -characterController.input_View.y) * Time.deltaTime;

        targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -settings.SwayClampX, settings.SwayClampX);
        targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -settings.SwayClampY, settings.SwayClampY);
        targetWeaponRotation.z = isAimingIn ? 0 : targetWeaponRotation.y;

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, settings.SwayResetSmoothing);
        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, settings.SwaySmoothing);

        targetWeaponMovementRotation.z = (isAimingIn ? settings.MovementSwayX / 3 : settings.MovementSwayX) * (settings.MovementSwayXInverted ? -characterController.input_Movement.x : characterController.input_Movement.x);
        targetWeaponMovementRotation.x = (isAimingIn ? settings.MovementSwayY / 3 : settings.MovementSwayY) * (settings.MovementSwayYInverted ? -characterController.input_Movement.y : characterController.input_Movement.y);

        targetWeaponMovementRotation = Vector3.SmoothDamp(targetWeaponMovementRotation, Vector3.zero, ref targetWeaponMovementRotationVelocity, settings.MovementSwaySmoothing);
        newWeaponMovementRotation = Vector3.SmoothDamp(newWeaponMovementRotation, targetWeaponMovementRotation, ref newWeaponMovementRotationVelocity, settings.MovementSwaySmoothing);


        transform.localRotation = Quaternion.Euler(newWeaponRotation + newWeaponMovementRotation);
    }

    #endregion

    #region - Weapon Animation

    private void SetWeaponAnimations()
    {
        if(!characterController.isPaused)
        {
            if (isGroundedTrigger)
            {
                fallingDelay = 0;
            }
            else
            {
                fallingDelay = Time.deltaTime;
            }

            if (characterController.isGrounded && !isGroundedTrigger && fallingDelay < 0.1f)
            {
                weaponAnimator.SetTrigger("Land");
                isGroundedTrigger = true;
            }

            else if (!characterController.isGrounded && isGroundedTrigger)
            {
                weaponAnimator.SetTrigger("Falling");
                isGroundedTrigger = false;
            }

            if (!characterController.isSprinting && isGroundedTrigger && timeStamp >= shootCooldown)
            {
                weaponAnimator.SetBool("IsShooting", isShooting);
            }

            weaponAnimator.SetBool("IsSprinting", characterController.isSprinting);
            weaponAnimator.SetFloat("WeaponAnimationSpeed", characterController.weaponAnimationSpeed);
        }
    }

    #endregion

    #region - Weapon Sway -

    private void CalculateWeaponSway()
    {
        if (!characterController.isPaused)
        {
            var targetPosition = LissajousCurve(swayTime, swayAmountA, swayAmountB) / (isAimingIn ? swayScale * 2 : swayScale);

            swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * swayLerpSpeed);
            swayTime += Time.deltaTime;

            if (swayTime > 6.3f)
            {
                swayTime = 0;
            }
        }
    }

    #endregion

    #region - LissajousCurve -

    private Vector3 LissajousCurve(float Time, float A, float B)
    {
        return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time + Mathf.PI));
    }

    #endregion
}
