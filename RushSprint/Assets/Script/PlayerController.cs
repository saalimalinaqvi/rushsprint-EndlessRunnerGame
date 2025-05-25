using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float jumpForce = 10f;
    public float gravity = -20f;
    public float slideDuration = 0.8f;

    [Header("Bullet")]
    private int obstacleHitCount = 0;
    public float bulletSpeed = 20f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public int maxBullets = 10;
    public int currentBullets = 8;
    public int bulletsInGun = 2; // UI: 2/8
    public TextMeshProUGUI bulletUIText;

    [Header("Boost")]
    private bool isBoosted = false;
    private float normalSpeed;
    private bool obstacleDisabled = false;

    private CharacterController controller;
    private Vector3 moveDirection;
    private int lane = 1;
    private bool isJumping = false;
    private bool isSliding = false;

    private Animator anim;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private bool swipeUp, swipeDown, swipeLeft, swipeRight;

    private float lastTapTime = 0f;
    private float doubleTapThreshold = 0.3f; // Max time between taps for a double tap
    private float nextAllowedTapTime = 0f;   // Cooldown tracker

    #region Monobehaviour Methods
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        UpdateBulletUI();

        SoundManager.Instance.Pause(AudioType.BG);
        SoundManager.Instance.Play(AudioType.RUNNING);
    }

    void Update()
    {
        moveDirection.z = forwardSpeed;
        DetectSwipe();

        if ((Input.GetKeyDown(KeyCode.LeftArrow) || swipeLeft) && lane > 0)
        {
            lane--;
            swipeLeft = false;
        }
        if ((Input.GetKeyDown(KeyCode.RightArrow) || swipeRight) && lane < 2)
        {
            lane++;
            swipeRight = false;
        }

        float targetX = (lane - 1) * laneDistance;
        moveDirection.x = (targetX - transform.position.x) * 10f;

        if (controller.isGrounded)
        {
            if ((Input.GetKeyDown(KeyCode.Space) || swipeUp) && !isSliding)
            {
                swipeUp = false;
                StartCoroutine(Jump());
                SoundManager.Instance.PlayAudio(SoundType.JUMP);
            }
        }
        else
        {
            moveDirection.y += gravity * Time.deltaTime;
        }

        if ((Input.GetKeyDown(KeyCode.DownArrow) || swipeDown) && controller.isGrounded && !isJumping && !isSliding)
        {
            swipeDown = false;
            StartCoroutine(Slide());
            SoundManager.Instance.PlayAudio(SoundType.SLIDE);
        }

        controller.Move(moveDirection * Time.deltaTime);

        if (Time.time < nextAllowedTapTime)
            return; // Still in cooldown, don't allow tapping

        if (Input.GetMouseButtonDown(0) && !isBoosted)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                float currentTime = Time.time;

                if (currentTime - lastTapTime < doubleTapThreshold)
                {
                    Shoot();

                    nextAllowedTapTime = Time.time + 0.5f;
                    lastTapTime = 0f; // Reset
                }
                else
                {
                    // First tap
                    lastTapTime = currentTime;
                }
            }
        }
    }

    #endregion

    #region Custom Methods

    IEnumerator Jump()
    {
        isJumping = true;
        anim.SetBool("isJumping", true);
        moveDirection.y = jumpForce;

        yield return new WaitForSeconds(0.2f);
        while (!controller.isGrounded)
        {
            yield return null;
        }

        isJumping = false;
        anim.SetBool("isJumping", false);
        anim.SetTrigger("RunTrigger");
    }

    IEnumerator Slide()
    {
        isSliding = true;
        anim.SetBool("isSliding", true);
        controller.height = 0.5f;

        yield return new WaitForSeconds(slideDuration);

        controller.height = 2.0f;
        isSliding = false;
        anim.SetBool("isSliding", false);
        anim.SetTrigger("RunTrigger");
    }

    void DetectSwipe()
    {
        swipeUp = swipeDown = swipeLeft = swipeRight = false;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                touchEndPos = touch.position;
                Vector2 swipeDelta = touchEndPos - touchStartPos;

                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    if (swipeDelta.x > 50) swipeRight = true;
                    else if (swipeDelta.x < -50) swipeLeft = true;
                }
                else
                {
                    if (swipeDelta.y > 50) swipeUp = true;
                    else if (swipeDelta.y < -50) swipeDown = true;
                }
            }
        }
    }

    public void Shoot()
    {
        if (bulletsInGun <= 0 || bulletPrefab == null) return;

        SoundManager.Instance.PlayAudio(SoundType.SHOOT);

        if (bulletsInGun > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.forward * bulletSpeed; // Move bullet forward
            }
            else
            {
                Debug.LogError("Bullet prefab is missing a Rigidbody!");
            }

            bulletsInGun--;
            UpdateBulletUI();

            if (bulletsInGun == 0)
            {
                StartCoroutine(AutoReload());
            }

            Destroy(bullet.gameObject, 3f);
        }
    }

    IEnumerator AutoReload()
    {
        yield return new WaitForSeconds(2f); // Simulating reload time

        if (currentBullets >= 2)
        {
            bulletsInGun = 2;
            currentBullets -= 2;
        }
        else
        {
            bulletsInGun = currentBullets;
            currentBullets = 0;
        }

        UpdateBulletUI();
    }

    public void CollectBullets(int amount)
    {
        currentBullets = Mathf.Min(currentBullets + amount, maxBullets);
        UpdateBulletUI();
    }

    void UpdateBulletUI()
    {
        //bulletUIText.text = currentBullets + "/" + maxBullets + " Total: " + totalBullets;
        bulletUIText.text = bulletsInGun + "/" + currentBullets;
    }

    public void ActivateSpeedBoost(float boostAmount, float duration)
    {
        if (!isBoosted)
        {
            isBoosted = true;
            normalSpeed = forwardSpeed;
            forwardSpeed += boostAmount;
            obstacleDisabled = true;

            StartCoroutine(ResetSpeedAfterDelay(duration));
        }
    }

    IEnumerator ResetSpeedAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        forwardSpeed = normalSpeed;
        isBoosted = false;
        obstacleDisabled = false;

        SoundManager.Instance.StopBoostSound();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            SoundManager.Instance.PlayAudio(SoundType.COIN_COLLECT);
        }
        else if (other.CompareTag("Gem"))
        {
            SoundManager.Instance.PlayAudio(SoundType.GEM_COLLECT);
        }
        else if (other.CompareTag("BoostSpeed"))
        {
            SoundManager.Instance.PlayAudio(SoundType.BOOST);
        }
        else if (other.CompareTag("BulletCollect"))
        {
            SoundManager.Instance.PlayAudio(SoundType.BULLET_COLLECT);
        }
        else if (other.CompareTag("Obstacle"))
        {
            if (!isBoosted)
            {
                SoundManager.Instance.Pause(AudioType.RUNNING);
                SoundManager.Instance.PlayAudio(SoundType.GAMEOVER);
                GameManager.instance.PlayerHit(other.gameObject);
            }
            else
            {
                other.gameObject.SetActive(false); // Smash during boost
            }
        }
    }

    #endregion
}