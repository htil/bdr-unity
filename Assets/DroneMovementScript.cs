using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class DroneMovementScript : MonoBehaviour {
    public EditorPathScript PathToFollow;
    public int current = 0;
    public float speed;
    public float maxSpeed = 80.0f;
    private float reach = 8.0f;
    public float rotationSpeed = 10.0f;
    public string pathName;

    Vector3 prevPos;
    Vector3 currPos;
    public Rigidbody drone;
    private Vector3 velocity = Vector3.zero;
    private float distance;

    public GameObject GameOver;
    public GameObject StartButton;
    public Text timerText;
    public Text gameOverText;
    private float startTime;
    private float finalTime;
    private bool isCompleted = false;
    private bool hasStarted = false;
    private bool hasRestarted = false;
    private bool notSetYet = true;
    private bool hasEnded = false;
    private int lapsLeft = 2;
    private int trialNum = 1;

    [DllImport("__Internal")]
    private static extern void SaveTime(float time);

    [DllImport("__Internal")]
    private static extern void SetState(bool started, bool ended, bool restarted);

    void Awake() {
        Time.timeScale = 0;
        startTime = Time.time;
        hasStarted = false;
        hasRestarted = false;
        isCompleted = false;
        notSetYet = true;

        SetState(hasStarted, hasEnded, hasRestarted);
    }
    void Start() {
        PathToFollow = GameObject.Find(pathName).GetComponent<EditorPathScript>();
        prevPos = transform.position;
        drone = GetComponent<Rigidbody>();
    }
    void FixedUpdate() {
        if (Time.timeScale == 1 && notSetYet) {
            hasStarted = true;
            hasRestarted = true;
            SetState(hasStarted, isCompleted, hasRestarted);
            notSetYet = false;
        }  

        if (!isCompleted) {
            float t = Time.time - startTime;
            string minutes = Mathf.Floor(t / 60).ToString("00");
            string seconds = Mathf.Floor(t % 60).ToString("00");
            timerText.text = minutes + ":" + seconds;

            distance = Vector3.Distance(PathToFollow.path_objs[current].position, transform.position);
            transform.position = Vector3.MoveTowards(transform.position, PathToFollow.path_objs[current].position, Time.deltaTime * speed);

            var rotation = Quaternion.LookRotation(PathToFollow.path_objs[current].position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
        else if (!hasEnded) {
            Invoke("EndGame", 4);
        }

        if (current >= PathToFollow.path_objs.Count - 1) {
            if(lapsLeft > 0) {
                lapsLeft--;
                current = 0;
            }
            else if (!isCompleted) {
                isCompleted = true;
                hasRestarted = false;
                finalTime = Time.time - startTime;
                SaveTime(finalTime);
                SetState(hasStarted, isCompleted, hasRestarted);
            }
        }
        else {
            if (distance <= reach) current++;
        }
    }
    void Update() {
        if (Input.anyKey) {
            StartButton.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void EndGame() {
        hasEnded = true;
        float t = finalTime;
        string minutes = ((int) t / 60).ToString("00");
        string seconds = (t % 60).ToString("00");
        gameOverText.text = minutes + ":" + seconds;
        GameOver.SetActive(true);
    }

    void SetSpeed(float engagement) {
        speed = maxSpeed * engagement;
        Mathf.Clamp(speed, 0.0f, maxSpeed);
    }
}
