using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Extensione.Window
{
    public class WindowMaster : MonoBehaviour
    {
        private static WindowMaster _instance = null;
        public static WindowMaster Instance
        {
            get
            {
                return _instance;
            }
            private set
            {
                if (_instance == null)
                {
                    _instance = value;
                    _instance.Hide();

                    DontDestroyOnLoad(_instance);
                }
                else
                {
                    Destroy(value.gameObject);
                }
            }
        }

        public Image vignetteImage;
        public Image popupImage;
        public Text titleText;
        public Text subtitleText;
        public Button closeButton;

        public bool autoFade = false;

        [Range(0.0f, 1.0f)] public float fadeRatio = 0.2f;
        public float fadeCountdown = 5;

        private float fadeTimer = 5;

        private bool isHiding = false;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            if (closeButton)
            {
                closeButton.onClick.AddListener(Hide);
            }
        }

        private void Update()
        {
            if (fadeTimer >= 0 && !isHiding)
            {
                fadeTimer -= Time.unscaledDeltaTime;
                if (autoFade)
                {
                    Fading();
                }
            }
            else
            {
                Hide();
                isHiding = true;
            }
        }

        public void Show()
        {
            Activate(true);

            isHiding = false;
            fadeTimer = fadeCountdown;
        }

        public void Show(string title)
        {
            Activate(true);
            if (titleText) titleText.text = title;

            isHiding = false;
            fadeTimer = fadeCountdown;
        }

        public void Show(string title, string subtitle)
        {
            Activate(true);
            if (titleText) titleText.text = title;
            if (subtitleText) subtitleText.text = subtitle;

            isHiding = false;
            fadeTimer = fadeCountdown;
        }

        public void Hide()
        {
            Activate(false);

            isHiding = true;
        }

        private void Activate(bool active)
        {
            vignetteImage?.gameObject.SetActive(active);
            popupImage?.gameObject.SetActive(active);
            titleText?.gameObject.SetActive(active);
            subtitleText?.gameObject.SetActive(active);
            closeButton?.gameObject.SetActive(active);

            ChangeOpacity(active ? 1 : 0);
        }

        private void ChangeOpacity(float opacity)
        {
            if (vignetteImage) { Color temp = vignetteImage.color; temp.a = opacity; vignetteImage.color = temp; }
            if (popupImage) { Color temp = popupImage.color; temp.a = opacity; popupImage.color = temp; }
            if (titleText) { Color temp = titleText.color; temp.a = opacity; titleText.color = temp; }
            if (subtitleText) { Color temp = subtitleText.color; temp.a = opacity; subtitleText.color = temp; }
            if (closeButton && closeButton.image) { Color temp = closeButton.image.color; temp.a = opacity; closeButton.image.color = temp; }
        }

        private void Fading()
        {
            if (vignetteImage) { Color temp = vignetteImage.color; temp.a = Mathf.Lerp(temp.a, 0, fadeRatio); vignetteImage.color = temp; }
            if (popupImage) { Color temp = popupImage.color; temp.a = Mathf.Lerp(temp.a, 0, fadeRatio); popupImage.color = temp; }
            if (titleText) { Color temp = titleText.color; temp.a = Mathf.Lerp(temp.a, 0, fadeRatio); titleText.color = temp; }
            if (subtitleText) { Color temp = subtitleText.color; temp.a = Mathf.Lerp(temp.a, 0, fadeRatio); subtitleText.color = temp; }
            if (closeButton && closeButton.image) { Color temp = closeButton.image.color; temp.a = Mathf.Lerp(temp.a, 0, fadeRatio); closeButton.image.color = temp; }
        }
    }
}