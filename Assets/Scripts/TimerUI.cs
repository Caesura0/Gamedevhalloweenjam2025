using UnityEngine;
using TMPro;
using System; // Required for TimeSpan
using UnityEngine.Events; // Required for UnityEvent

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timeRemaining;
    private bool isRunning = false;

    public static TimerUI instance;


    [Tooltip("Event triggered when the timer reaches zero.")]
    public UnityEvent OnTimerEnd;

    /// Called when the script instance is being loaded.
    void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        // Set the initial display text
        timerText.text = "00:00:00";
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// Update is called once per frame.
    void Update()
    {
        // Only process the timer if it's running
        if (isRunning)
        {
            //Debug.Log("Timer running: " + timeRemaining);
            if (timeRemaining > 0)
            {
                //Debug.Log("Time remaining: " + timeRemaining);
                // Decrease the time and update the display
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                // Timer has finished
                timeRemaining = 0;
                isRunning = false;
                UpdateTimerDisplay();
                float temp = 6; // Placeholder for potions crafted to test GameOverUI
                GameOverUI.instance.ShowGameOver(temp); // Show game over screen and pass potions crafted count


                OnTimerEnd.Invoke(); // Trigger the event for other scripts
            }
        }
    }

    /// Updates the text display with the formatted remaining time.
    private void UpdateTimerDisplay()
    {
        // Use TimeSpan to easily format the seconds into MM:SS:ff
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
        timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
            timeSpan.Minutes,
            timeSpan.Seconds,
            timeSpan.Milliseconds / 10);
    }

    
    /// Starts the countdown from a specific time in seconds.
    /// Call this method from another script to begin the timer.
    public void BeginCountdown(float durationInSeconds)
    {
        Debug.Log("Starting countdown: " + durationInSeconds + " seconds");
        timeRemaining = durationInSeconds;
        isRunning = true;
    }
}
