using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;  // Import the TextMeshPro namespace

public class ScreenshotHandler : MonoBehaviour
{
    public Button captureButton;
    public Image flashPanel;  // Assign FlashPanel here
    public AudioSource shutterSound; // Assign a shutter sound

    // Reference to the TextMeshProUGUI component for the confirmation message
    public TextMeshProUGUI saveConfirmationText;  // Assign this in the Inspector

    private string lastImagePath; // Stores last captured image path

    private void Start()
    {
        captureButton.onClick.AddListener(TakeScreenshot);
    }

    void TakeScreenshot()
    {
        StartCoroutine(CaptureAndSave());
    }

    IEnumerator CaptureAndSave()
    {
        // Start Flash Effect
        StartCoroutine(FlashEffect());

        // Play shutter sound
        if (shutterSound != null)
        {
            shutterSound.Play();
        }

        yield return new WaitForEndOfFrame();

        // Capture screen
        Texture2D screenImage = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        // Save to file
        string fileName = "FaceFilter_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(filePath, screenImage.EncodeToPNG());

        Destroy(screenImage);

        // Save to gallery
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(filePath, "FaceFilterApp", fileName);
        if (permission == NativeGallery.Permission.Granted)
        {
            Debug.Log("Saved to Gallery: " + filePath);
            lastImagePath = filePath; // Store the image path

            // Show the confirmation message
            StartCoroutine(ShowSaveConfirmation());
        }
        else
        {
            Debug.Log("Gallery Permission Denied");
        }
    }

    IEnumerator FlashEffect()
    {
        flashPanel.gameObject.SetActive(true);
        float duration = 0.2f; // Duration of the flash
        Color flashColor = flashPanel.color;

        // Ensure flash color is in the 0 to 1 range
        flashColor.r = flashColor.r / 255f;
        flashColor.g = flashColor.g / 255f;
        flashColor.b = flashColor.b / 255f;

        // Fade in (alpha transition from 0 to 1)
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            flashColor.a = Mathf.Lerp(0, 1, t / duration); // Lerp from 0 to 1
            flashPanel.color = flashColor;
            yield return null;
        }

        // Hold for a brief moment
        yield return new WaitForSeconds(0.1f);

        // Fade out (alpha transition from 1 to 0)
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            flashColor.a = Mathf.Lerp(1, 0, t / duration); // Lerp from 1 to 0
            flashPanel.color = flashColor;
            yield return null;
        }

        // Hide the flash panel after the effect
        flashPanel.gameObject.SetActive(false);
    }

    // Coroutine to show the save confirmation message
    IEnumerator ShowSaveConfirmation()
    {
        // Ensure the TextMeshPro is inactive initially
        saveConfirmationText.gameObject.SetActive(true);
        saveConfirmationText.text = "Screenshot saved to gallery!"; // Change text to confirmation message

        // Show the confirmation message for 2 seconds
        yield return new WaitForSeconds(2f);

        // Hide the confirmation message after 2 seconds
        saveConfirmationText.gameObject.SetActive(false);
    }
}
