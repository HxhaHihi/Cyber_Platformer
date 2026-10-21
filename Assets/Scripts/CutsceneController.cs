using System.Collections;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    [Header("Камеры")]
    public Transform camera1;
    public Transform camera2;

    [Header("Путь 1-й камеры (облет уровня)")]
    public Transform[] cam1Waypoints;
    public float cam1Duration = 6f;

    [Header("Точки 2-й камеры (к порталу)")]
    public Transform cam2Start;
    public Transform cam2End;
    public float cam2Duration = 3f;

    [Header("UI Текст")]
    public GameObject titleText;
    public float textDelayAfterCam2 = 0f;
    public float textDuration = 3f;

    [Header("Музыка")]
    public AudioClip cutsceneMusic;
    [Range(0f, 1f)] public float musicVolume = 0.5f;

    [Header("Камера игрока (для возврата управления)")]
    public GameObject playerCamera;

    private AudioSource audioSource;

    private void Awake()
    {
        if (titleText != null) titleText.SetActive(false);

        // Автоматически ищем камеру игрока, если она не задана
        if (playerCamera == null)
        {
            GameObject mainCam = GameObject.FindWithTag("MainCamera");
            if (mainCam != null) playerCamera = mainCam;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = musicVolume;
        if (cutsceneMusic != null)
        {
            audioSource.clip = cutsceneMusic;
        }
    }

    private void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        if (playerCamera != null) playerCamera.SetActive(false);

        if (camera1 != null) camera1.gameObject.SetActive(true);
        if (camera2 != null) camera2.gameObject.SetActive(false);
        if (titleText != null) titleText.SetActive(false);

        if (camera1 != null && cam1Waypoints != null && cam1Waypoints.Length > 1)
        {
            yield return StartCoroutine(MoveAlongPath(camera1, cam1Waypoints, cam1Duration));
        }

        if (camera1 != null) camera1.gameObject.SetActive(false);
        if (camera2 != null) camera2.gameObject.SetActive(true);

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.volume = musicVolume;
            audioSource.Play();
        }

        StartCoroutine(HandleTextLifetime());

        if (camera2 != null && cam2Start != null && cam2End != null)
        {
            yield return StartCoroutine(MoveObject(camera2, cam2Start, cam2End, cam2Duration));
        }

        if (camera2 != null) camera2.gameObject.SetActive(false);

        // Возвращаем управление камере игрока
        if (playerCamera != null)
        {
            playerCamera.SetActive(true);
        }
        else
        {
            GameObject fallbackCam = GameObject.FindWithTag("MainCamera");
            if (fallbackCam != null)
            {
                fallbackCam.SetActive(true);
            }
        }
    }

    private IEnumerator MoveAlongPath(Transform obj, Transform[] waypoints, float duration)
    {
        if (obj == null || waypoints == null || waypoints.Length < 2) yield break;

        float elapsed = 0f;
        int segments = waypoints.Length - 1;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            t = Mathf.SmoothStep(0f, 1f, t);

            float scaledT = t * segments;
            int currentIndex = Mathf.FloorToInt(scaledT);
            if (currentIndex >= segments) currentIndex = segments - 1;

            float localT = scaledT - currentIndex;

            Transform start = waypoints[currentIndex];
            Transform end = waypoints[currentIndex + 1];

            if (start != null && end != null)
            {
                obj.position = Vector3.Lerp(start.position, end.position, localT);
                obj.rotation = Quaternion.Slerp(start.rotation, end.rotation, localT);
            }

            yield return null;
        }

        Transform finalPoint = waypoints[waypoints.Length - 1];
        if (finalPoint != null)
        {
            obj.position = finalPoint.position;
            obj.rotation = finalPoint.rotation;
        }
    }

    private IEnumerator MoveObject(Transform obj, Transform start, Transform end, float duration)
    {
        if (obj == null || start == null || end == null) yield break;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = Mathf.SmoothStep(0f, 1f, t);

            obj.position = Vector3.Lerp(start.position, end.position, t);
            obj.rotation = Quaternion.Slerp(start.rotation, end.rotation, t);

            yield return null;
        }

        obj.position = end.position;
        obj.rotation = end.rotation;
    }

    private IEnumerator HandleTextLifetime()
    {
        if (textDelayAfterCam2 > 0f)
        {
            yield return new WaitForSeconds(textDelayAfterCam2);
        }

        if (titleText != null) titleText.SetActive(true);
        yield return new WaitForSeconds(textDuration);
        if (titleText != null) titleText.SetActive(false);
    }
}