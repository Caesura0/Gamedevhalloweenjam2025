using UnityEngine;
using TMPro;
using System; // Required for TimeSpan
using UnityEngine.Events; // Required for UnityEvent

public class TimerUI : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private float timeRemaining;
    private bool isRunning = false;

    [Tooltip("Event triggered when the timer reaches zero.")]
    public UnityEvent OnTimerEnd;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        // Set the initial display text
        timerText.text = "00:00:00";
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        // Only process the timer if it's running
        if (isRunning)
        {
            if (timeRemaining > 0)
            {
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
                OnTimerEnd.Invoke(); // Trigger the event for other scripts
            }
        }
    }

    /// <summary>
    /// Updates the text display with the formatted remaining time.
    /// </summary>
    private void UpdateTimerDisplay()
    {
        // Use TimeSpan to easily format the seconds into MM:SS:ff
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
        timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
            timeSpan.Minutes,
            timeSpan.Seconds,
            timeSpan.Milliseconds / 10);
    }

    /// <summary>
    /// Starts the countdown from a specific time in seconds.
    /// Call this method from another script to begin the timer.
    /// </summary>
    /// <param name="durationInSeconds">The time to count down from.</param>
    public void BeginCountdown(float durationInSeconds)
    {
        timeRemaining = durationInSeconds;
        isRunning = true;
    }
}
