using UnityEngine;
using System.Collections;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 input;
    private float turnVel;

    public float moveForce = 50f;
    public float gravity = 30f;
    public float slamForce = 80f;

    public float groundCheckDistance = 0.3f;
    public LayerMask groundMask;
    public bool isGrounded;

    public Camera cam;
    public Vector3 cameraOffset = new Vector3(0f, 5f, -7f);
    public float cameraSmoothTime = 0.15f;

    public bool showDebug = true;
    public Animator animator;

    private Vector3 camVelocity;
    private bool slamPressed;
    public int multiplier;
    public TextMeshProUGUI multiplierText;
    public Animator multAnim;
    bool slammedInAir;
    bool rolledAfterSlam;
    bool trickLocked = false;
    public playerData PD;
    public float runSoundInterval = 0.45f;
    float nextRunSoundTime;


    Coroutine trickBuffRoutine;
    AudioSource runSFX;

    void Awake()
    {
        multiplier = 1;
        multiplierText.text = "";
        rb = GetComponent<Rigidbody>();
        runSFX = GetComponent<AudioSource>();
        UpdateMultiplier();
    }

    void Update()
    {
        input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        slamPressed = Input.GetKeyDown(KeyCode.Space);

        if (!isGrounded && slamPressed)
        {
            slammedInAir = true;
            rolledAfterSlam = false;
            animator.Play("Slam");
            rb.AddForce(Vector3.down * slamForce, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isGrounded) animator.Play("trick");
            else animator.Play("trickGround");

            StartCoroutine(ApplyTrickMultiplier());
        }
    }

    void FixedUpdate()
    {
        CheckGround();
        var st = animator.GetCurrentAnimatorStateInfo(0);
        float speed = rb.linearVelocity.magnitude;

        if (speed < 25f && !PD.wait15sec)
            PD.ClockSpeed = 2.3f;
        else if (speed < 50f && !PD.wait15sec)
            PD.ClockSpeed = 1.7f;
        else if (speed < 75f && !PD.wait15sec)
            PD.ClockSpeed = 1f;
        else if (!PD.wait15sec)
            PD.ClockSpeed = 0.75f;

        if (!isGrounded)
            rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        rb.AddForce(input * moveForce);

        if (input.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg;
            float smooth = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnVel, 0.1f);
            transform.rotation = Quaternion.Euler(0f, smooth, 0f);
        }

        if (st.IsName("trick") || st.IsName("trickGround"))
            return;

        if (!isGrounded)
        {
            StopRunAudio();

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Fly") &&
                !animator.GetCurrentAnimatorStateInfo(0).IsName("Slam") &&
                !animator.GetCurrentAnimatorStateInfo(0).IsName("trick") &&
                !animator.GetCurrentAnimatorStateInfo(0).IsName("midFly"))
            {
                animator.Play("Fly");
            }
        }
        else
        {
            if (slammedInAir && !rolledAfterSlam)
            {
                rolledAfterSlam = true;
                animator.Play("Roll");
                StopRunAudio();
                return;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
            {
                StopRunAudio();
                return;
            }

            slammedInAir = false;

            if (input.sqrMagnitude == 0f)
            {
                animator.Play("Idle");
                StopRunAudio();
            }
            else
            {
                animator.Play("Running");
                PlayRunAudio();
            }
        }
    }

    void LateUpdate()
    {
        if (!cam) return;

        Vector3 targetPos = transform.position + cameraOffset;
        cam.transform.position = Vector3.SmoothDamp(
            cam.transform.position,
            targetPos,
            ref camVelocity,
            cameraSmoothTime
        );

        cam.transform.LookAt(transform.position);
    }

    void PlayRunAudio()
    {
        if (!runSFX) return;

        if (Time.time < nextRunSoundTime) return;

        runSFX.pitch = Random.Range(0.9f, 1.2f);
        runSFX.Play();
        nextRunSoundTime = Time.time + runSoundInterval;
    }


    void StopRunAudio()
    {
        if (runSFX && runSFX.isPlaying)
            runSFX.Stop();
    }

    IEnumerator ApplyTrickMultiplier()
    {
        if (trickBuffRoutine != null)
            StopCoroutine(trickBuffRoutine);

        if (!trickLocked)
        {
            trickLocked = true;
            multiplier += 1;
            multAnim.Play("Add");
            UpdateMultiplier();

            trickBuffRoutine = StartCoroutine(RemoveTrickBonusAfter());
            yield return new WaitForSeconds(1.95f);
            trickLocked = false;
        }
    }

    IEnumerator RemoveTrickBonusAfter()
    {
        yield return new WaitForSeconds(1.75f);
        multiplier = 1;
        UpdateMultiplier();
        trickBuffRoutine = null;
    }

    public void SpeedBoost(int amount)
    {
        Vector3 boostDir = transform.forward * amount;
        rb.AddForce(boostDir, ForceMode.Impulse);
    }

    public void UpdateMultiplier()
    {
        if (multiplierText)
            multiplierText.text = multiplier + "x";

        if (multiplier <= 1)
        {
            multiplier = 1;
            multiplierText.text = "";
        }
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(
            transform.position + Vector3.up * 0.1f,
            Vector3.down,
            groundCheckDistance,
            groundMask
        );
    }
}
