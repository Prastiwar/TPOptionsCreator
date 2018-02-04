using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TP.Options
{
    public class TPOptionsLayout : MonoBehaviour
    {
        public bool isMusicOn;
        public bool isFXOn;

        [HideInInspector] public Dropdown resDropdown;
        [HideInInspector] public Dropdown qualityDropdown;
        [HideInInspector] public Dropdown aliasingDropdown;
        [HideInInspector] public Dropdown shadowQualDropdown;
        [HideInInspector] public Dropdown shadowDropdown;
        [HideInInspector] public Dropdown textureDropdown;

        [HideInInspector] public Toggle fullscreenToggle;
        [HideInInspector] public Toggle vSyncToggle;
        [HideInInspector] public Toggle anisotropicToggle;

        [HideInInspector] public Button fxButton;
        [HideInInspector] public Button musicButton;
        [HideInInspector] public Sprite fxImageOn;
        [HideInInspector] public Sprite fxImageOff;
        [HideInInspector] public Sprite musicImageOn;
        [HideInInspector] public Sprite musicImageOff;
        [HideInInspector] public Slider fxSlider;
        [HideInInspector] public Slider musicSlider;
        [HideInInspector] public string mixerFXText;
        [HideInInspector] public string mixerMusicText;
        [HideInInspector] public UnityEngine.Audio.AudioMixer AudioMixer;

        TPOptionsCreator creator;
        Canvas canvas;
        Image fxImage;
        Image musicImage;

        [SerializeField] float musicValue;
        [SerializeField] float fxValue;

        int CustomQualityIndex;
        int previousLevel;
        [SerializeField] int currentResolutionIndex = 0;
        [SerializeField] int currentQualityIndex = 0;
        [SerializeField] int currentAliasingIndex = 0;
        [SerializeField] int currentShadowQualIndex = 0;
        [SerializeField] int currentShadowIndex = 0;
        [SerializeField] int currentTextureIndex = 0;

        List<string> resOptions = new List<string>();
        List<string> qualityOptions = new List<string>();
        List<string> shadowQualOptions = new List<string>();
        List<string> shadowOptions = new List<string>();
        List<string> aliasingOptions = new List<string>();
        List<string> textureOptions = new List<string>();

        delegate void Action(int i);
        Action qualityAction;
        Action resAction;
        Action shadowQualAction;
        Action shadowAction;
        Action antialiasingAction;
        Action textureAction;


        void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (creator == null) creator = FindObjectOfType<TPOptionsCreator>();
            if (canvas == null) canvas = GetComponent<Canvas>();
            if (fxImage == null && fxButton) fxImage = fxButton.GetComponent<Image>();
            if (musicImage == null && musicButton) musicImage = musicButton.GetComponent<Image>();

            int length = QualitySettings.names.Length;
            for (int i = 0; i < length; i++)
            {
                if (QualitySettings.names[i] == "Custom")
                {
                    CustomQualityIndex = i;
                    break;
                }

                if (i == length - 1)
                {
                    Debug.LogError("No 'Custom' quality level found. Create one!");
                    return;
                }
            }

            isMusicOn = true;
            isFXOn = true;

            InitializeAllDropdowns();
            InitializeAllToggles();
            AddListeners();
        }

        void InitializeAllDropdowns()
        {
            if (resAction == null)
                resAction = InitializeResolution;
            if (qualityAction == null)
                qualityAction = InitializeQuality;
            if (shadowQualAction == null)
                shadowQualAction = InitializeShadowsQuality;
            if (shadowAction == null)
                shadowAction = InitializeShadows;
            if (antialiasingAction == null)
                antialiasingAction = InitializeAliasingQuality;
            if (textureAction == null)
                textureAction = InitializeTextureQuality;
            
            InitializeDropdown(resAction,
                ref resDropdown, ref resOptions, ref currentResolutionIndex, Screen.resolutions.Length);

            InitializeDropdown(qualityAction,
                ref qualityDropdown, ref qualityOptions, ref currentQualityIndex, QualitySettings.names.Length);

            InitializeDropdown(shadowQualAction,
                ref shadowQualDropdown, ref shadowQualOptions, ref currentShadowQualIndex, 4);

            InitializeDropdown(shadowAction,
                ref shadowDropdown, ref shadowOptions, ref currentShadowIndex, 3);

            InitializeDropdown(antialiasingAction,
                ref aliasingDropdown, ref aliasingOptions, ref currentAliasingIndex, 4);

            InitializeDropdown(textureAction,
                ref textureDropdown, ref textureOptions, ref currentTextureIndex, 4);
        }

        void InitializeAllToggles()
        {
            if (fullscreenToggle && fullscreenToggle.isOn != Screen.fullScreen)
                fullscreenToggle.isOn = Screen.fullScreen;
            if (vSyncToggle && vSyncToggle.isOn != (QualitySettings.vSyncCount == 0 ? false : true))
                vSyncToggle.isOn = QualitySettings.vSyncCount == 0 ? false : true;
            if (anisotropicToggle && anisotropicToggle.isOn != ((int)QualitySettings.anisotropicFiltering == 0 ? false : true))
                anisotropicToggle.isOn = (int)QualitySettings.anisotropicFiltering == 0 ? false : true;
        }

        void AddListeners()
        {
            if (resDropdown && resDropdown.onValueChanged.GetPersistentEventCount() == 0)
                resDropdown.onValueChanged.AddListener(SetResolution);
            if (qualityDropdown && qualityDropdown.onValueChanged.GetPersistentEventCount() == 0)
                qualityDropdown.onValueChanged.AddListener(SetQuality);
            if (aliasingDropdown && aliasingDropdown.onValueChanged.GetPersistentEventCount() == 0)
                aliasingDropdown.onValueChanged.AddListener(SetAntiAliasing);
            if (shadowQualDropdown && shadowQualDropdown.onValueChanged.GetPersistentEventCount() == 0)
                shadowQualDropdown.onValueChanged.AddListener(SetShadowsQuality);
            if (shadowDropdown && shadowDropdown.onValueChanged.GetPersistentEventCount() == 0)
                shadowDropdown.onValueChanged.AddListener(SetShadows);
            if (textureDropdown && textureDropdown.onValueChanged.GetPersistentEventCount() == 0)
                textureDropdown.onValueChanged.AddListener(SetTexture);

            if (fullscreenToggle && fullscreenToggle.onValueChanged.GetPersistentEventCount() == 0)
                fullscreenToggle.onValueChanged.AddListener(SetFullScreen);
            if (vSyncToggle && vSyncToggle.onValueChanged.GetPersistentEventCount() == 0)
                vSyncToggle.onValueChanged.AddListener(SetVSync);
            if (anisotropicToggle && anisotropicToggle.onValueChanged.GetPersistentEventCount() == 0)
                anisotropicToggle.onValueChanged.AddListener(SetAnisotropic);

            if (fxSlider)
                fxSlider.onValueChanged.AddListener(SetFXVolume);
            if (musicSlider)
                musicSlider.onValueChanged.AddListener(SetMusicVolume);

            if (fxButton)
                fxButton.onClick.AddListener(SetFX);
            if (musicButton)
                musicButton.onClick.AddListener(SetMusic);
        }


        // *** Initializers *** //

        void InitializeTextureQuality(int i)
        {
            switch (i)
            {
                case 0:
                    textureOptions.Add("Very High");
                    break;
                case 1:
                    textureOptions.Add("High");
                    break;
                case 2:
                    textureOptions.Add("Medium");
                    break;
                case 3:
                    textureOptions.Add("Low");
                    break;
                default:
                    break;
            }

            if (i == QualitySettings.masterTextureLimit)
                currentTextureIndex = i;
        }

        void InitializeResolution(int i)
        {
            string option = Screen.resolutions[i].width + " x " + Screen.resolutions[i].height;
            resOptions.Add(option);

            if (Screen.resolutions[i].width == Screen.currentResolution.width &&
                Screen.resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        void InitializeQuality(int i)
        {
            string option = QualitySettings.names[i];
            qualityOptions.Add(option);

            if (i == QualitySettings.GetQualityLevel())
                currentQualityIndex = i;
        }

        void InitializeShadowsQuality(int i)
        {
            string option = ((ShadowResolution)i).ToString();
            shadowQualOptions.Add(option);

            if (i == (int)QualitySettings.shadowResolution)
                currentShadowQualIndex = i;
        }

        void InitializeShadows(int i)
        {
            string option = ((ShadowQuality)i).ToString();
            shadowOptions.Add(option);

            if (i == (int)QualitySettings.shadows)
                currentShadowIndex = i;
        }

        void InitializeAliasingQuality(int i)
        {
            string option = "";
            switch (i)
            {
                case 0:
                    option = "Disabled";
                    break;
                case 1:
                case 2:
                    option = (i * 2).ToString() + "x Multi Sampling";
                    break;
                case 3:
                    option = "8x Multi Sampling";
                    break;
                default:
                    break;
            }
            aliasingOptions.Add(option);

            if (i == QualitySettings.antiAliasing)
                currentAliasingIndex = i;
        }

        void InitializeDropdown(Action action, ref Dropdown dropdown, ref List<string> options, ref int currentIndex, int length)
        {
            options.Clear();
            if (!dropdown)
                return;

            for (int i = 0; i < length; i++)
            {
                action(i);
            }

            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.ClearOptions();
            dropdown.AddOptions(options);
            dropdown.value = currentIndex;
            dropdown.RefreshShownValue();
        }


        // *** Setters *** ///

        void SetMusic()
        {
            isMusicOn = !isMusicOn;
            musicImage.sprite = !isMusicOn ? musicImageOff : musicImageOn;

            if (!isMusicOn)
                musicValue = GetFloat(mixerMusicText);

            AudioMixer.SetFloat(mixerMusicText, isMusicOn ? musicValue : -80);
        }


        void SetFX()
        {
            isFXOn = !isFXOn;
            fxImage.sprite = !isFXOn ? fxImageOff : fxImageOn;

            if (!isFXOn)
                fxValue = GetFloat(mixerFXText);

            AudioMixer.SetFloat(mixerFXText, isFXOn ? fxValue : -80);
        }

        float GetFloat(string param)
        {
            float value;
            bool result = AudioMixer.GetFloat(param, out value);
            if (result)
                return value;
            else
                return 0f;
        }

        void SetResolution(int index)
        {
            Resolution resolution = Screen.resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        void SetQuality(int index)
        {
            bool _full = Screen.fullScreen;
            Resolution _res = Screen.currentResolution;
            bool _vsync = QualitySettings.vSyncCount == 0 ? false : true;

            QualitySettings.SetQualityLevel(index);
            
            Screen.SetResolution(_res.width, _res.height, Screen.fullScreen);
            QualitySettings.vSyncCount = _vsync ? 1 : 0;
            Initialize();
        }

        void SetAntiAliasing(int index)
        {
            SetToCustom(() => SetAntiAliasing(index));
            switch (index)
            {
                case 0:
                    QualitySettings.antiAliasing = index;
                    break;
                case 1:
                    QualitySettings.antiAliasing = 2;
                    break;
                case 2:
                    QualitySettings.antiAliasing = 4;
                    break;
                case 3:
                    QualitySettings.antiAliasing = 8;
                    break;
                default:
                    break;
            }
            aliasingDropdown.value = index;
        }

        void SetShadowsQuality(int index)
        {
            SetToCustom(() => SetShadowsQuality(index));
            QualitySettings.shadowResolution = (ShadowResolution)index;
        }

        void SetShadows(int index)
        {
            SetToCustom(() => SetShadows(index));
            QualitySettings.shadows = (ShadowQuality)index;
        }

        void SetTexture(int index)
        {
            SetToCustom(() => SetTexture(index));
            QualitySettings.masterTextureLimit = index;
        }

        void SetFullScreen(bool boolean)
        {
            Screen.fullScreen = boolean;
        }

        void SetFXVolume(float value)
        {
            AudioMixer.SetFloat(mixerFXText, value);
            if (value <= -30)
                AudioMixer.SetFloat(mixerFXText, -80);
        }

        void SetMusicVolume(float value)
        {
            AudioMixer.SetFloat(mixerMusicText, value);
            if(value <= -30)
                AudioMixer.SetFloat(mixerMusicText, -80);
        }

        void SetVSync(bool boolean)
        {
            QualitySettings.vSyncCount = boolean ? 1 : 0;
        }

        void SetAnisotropic(bool boolean)
        {
            SetToCustom(()=> SetAnisotropic(boolean));
            QualitySettings.anisotropicFiltering = (AnisotropicFiltering)(boolean ? 2 : 0);
        }

        void SetToCustom(UnityEngine.Events.UnityAction action)
        {
            previousLevel = QualitySettings.GetQualityLevel();
            if (previousLevel == CustomQualityIndex)
                return;

            int _tex = QualitySettings.masterTextureLimit;
            int _shadRes = (int)QualitySettings.shadowResolution;
            int _shad = (int)QualitySettings.shadows;
            int _ani = (int)QualitySettings.anisotropicFiltering;
            int _anti = QualitySettings.antiAliasing;
            Resolution _res = Screen.currentResolution;
            bool _vsync = QualitySettings.vSyncCount == 0 ? false : true;
            bool _full = Screen.fullScreen;

            QualitySettings.SetQualityLevel(CustomQualityIndex);
            Screen.fullScreen = _full;
            Screen.SetResolution(_res.width, _res.height, Screen.fullScreen);
            QualitySettings.masterTextureLimit = _tex;
            QualitySettings.shadowResolution = (ShadowResolution)_shadRes;
            QualitySettings.shadows = (ShadowQuality)_shad;
            QualitySettings.anisotropicFiltering = (AnisotropicFiltering)_ani;
            QualitySettings.antiAliasing = _anti;
            QualitySettings.vSyncCount = _vsync ? 1 : 0;

            action();
            Initialize();
        }

    }
}