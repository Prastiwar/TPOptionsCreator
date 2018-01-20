using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TP_Menu
{
    public class TPOptionsLayout : MonoBehaviour
    {
        TPMenuCreator creator;
        Canvas canvas;

        int currentResolutionIndex = 0;
        int currentQualityIndex = 0;
        int currentAliasingIndex = 0;
        int currentShadowQualIndex = 0;
        int currentShadowIndex = 0;

        List<string> resOptions = new List<string>();
        List<string> qualityOptions = new List<string>();
        List<string> shadowQualOptions = new List<string>();
        List<string> shadowOptions = new List<string>();
        List<string> aliasingOptions = new List<string>();

        [SerializeField] Dropdown resDropdown;
        [SerializeField] Dropdown qualityDropdown;
        [SerializeField] Dropdown aliasingDropdown;
        [SerializeField] Dropdown shadowQualDropdown;
        [SerializeField] Dropdown shadowDropdown;

        [SerializeField] Toggle fullscreenToggle;
        [SerializeField] Toggle vSyncToggle;

        [SerializeField] Slider fxSlider;
        [SerializeField] Slider musicSlider;
        [SerializeField] string mixerFXText;
        [SerializeField] string mixerMusicText;
        [SerializeField] UnityEngine.Audio.AudioMixer AudioMixer;

        delegate void Action(int i);
        Action qualityAction;
        Action resAction;
        Action shadowQualAction;
        Action shadowAction;
        Action antialiasingAction;

        void Awake()
        {
            Refresh();
        }

        public void Refresh()
        {
            if (creator == null) creator = FindObjectOfType<TPMenuCreator>();
            if (canvas == null) canvas = GetComponent<Canvas>();
            
            resAction = InitializeResolution;
            qualityAction = InitializeQuality;
            shadowQualAction = InitializeShadowsQuality;
            shadowAction = InitializeShadows;
            antialiasingAction = InitializeAliasingQuality;

            ClearLists();

            InitializeDropdown(resAction,
                resDropdown, resOptions, ref currentResolutionIndex, Screen.resolutions.Length);

            InitializeDropdown(qualityAction,
                qualityDropdown, qualityOptions, ref currentQualityIndex, QualitySettings.names.Length);

            InitializeDropdown(shadowQualAction,
                shadowQualDropdown, shadowQualOptions, ref currentShadowQualIndex, 4);

            InitializeDropdown(shadowAction,
                shadowDropdown, shadowOptions, ref currentShadowIndex, 3);

            InitializeDropdown(antialiasingAction,
                aliasingDropdown, aliasingOptions, ref currentAliasingIndex, 4);

            AddListeners();
        }

        void ClearLists()
        {
            if(resOptions.Count > 0)
                resOptions.Clear();
            if (qualityOptions.Count > 0)
                qualityOptions.Clear();
            if (shadowQualOptions.Count > 0)
                shadowQualOptions.Clear();
            if (shadowOptions.Count > 0)
                shadowOptions.Clear();
            if (aliasingOptions.Count > 0)
                aliasingOptions.Clear();
        }

        void AddListeners()
        {
            if (resDropdown)
                resDropdown.onValueChanged.AddListener(SetResolution);
            if (qualityDropdown)
                qualityDropdown.onValueChanged.AddListener(SetQuality);
            if (aliasingDropdown)
                aliasingDropdown.onValueChanged.AddListener(SetAntiAliasing);
            if (shadowQualDropdown)
                shadowQualDropdown.onValueChanged.AddListener(SetShadowsQuality);
            if (shadowDropdown)
                shadowDropdown.onValueChanged.AddListener(SetShadows);

            if (fullscreenToggle)
                fullscreenToggle.onValueChanged.AddListener(SetFullScreen);
            if (vSyncToggle)
                vSyncToggle.onValueChanged.AddListener(SetVSync);

            if (fxSlider)
                fxSlider.onValueChanged.AddListener(SetFXVolume);
            if (musicSlider)
                musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        // *** Initializers *** //

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
                    option = (8).ToString() + "x Multi Sampling";
                    break;
                default:
                    break;
            }
            aliasingOptions.Add(option);

            if (i == QualitySettings.antiAliasing)
                currentAliasingIndex = i;
        }

        void InitializeDropdown(Action action, Dropdown dropdown, List<string> options, ref int currentIndex, int length)
        {
            for (int i = 0; i < length; i++)
            {
                action(i);
            }

            dropdown.AddOptions(options);
            dropdown.value = currentIndex;
            dropdown.RefreshShownValue();
        }

        // *** Setters *** ///

        void SetResolution(int index)
        {
            Resolution resolution = Screen.resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        void SetQuality(int index)
        {
            QualitySettings.SetQualityLevel(index);
        }

        void SetAntiAliasing(int index)
        {
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
        }

        void SetShadowsQuality(int index)
        {
            QualitySettings.shadowResolution = (ShadowResolution)index;
        }

        void SetShadows(int index)
        {
            QualitySettings.shadows = (ShadowQuality)index;
        }

        void SetFullScreen(bool boolean)
        {
            Screen.fullScreen = boolean;
        }

        void SetFXVolume(float value)
        {
            AudioMixer.SetFloat("FXVolume", value);
        }

        void SetMusicVolume(float value)
        {
            AudioMixer.SetFloat("MusicVolume", value);
        }

        void SetVSync(bool boolean)
        {
            QualitySettings.vSyncCount = boolean ? 1 : 0;
        }

    }
}