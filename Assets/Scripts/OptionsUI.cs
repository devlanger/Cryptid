using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : ViewUI
{
    public GameOptions options = new GameOptions();

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle[] qualityToggles;
    [SerializeField] private Toggle[] fogToggles;

    private void Awake()
    {
        volumeSlider.onValueChanged.AddListener(SetVolumeLevel);
        LoadSettings();
        LoadUI();
    }

    private void LoadUI()
    {
        qualityToggles[options.qualityLevel].SetIsOnWithoutNotify(true);
        fogToggles[options.fogLevel].SetIsOnWithoutNotify(true);
        volumeSlider.SetValueWithoutNotify(options.volumeLevel);
    }

    public void SetVolumeLevel(float level)
    {
        options.volumeLevel = level;
        ApplyOptions();
    }

    public void SetQualityLevel(int level)
    {
        options.qualityLevel = level;
        ApplyOptions();
    }

    public void SetFogLevel(int level)
    {
        options.fogLevel = level;
        ApplyOptions();
    }

    public void LoadSettings()
    {
        options.volumeLevel = AudioListener.volume;
        options.qualityLevel = QualitySettings.GetQualityLevel();
        options.fogLevel = RenderSettings.fog ? 1 : 0;
    }

    public void ApplyOptions()
    {
        AudioListener.volume = options.volumeLevel;
        QualitySettings.SetQualityLevel(options.qualityLevel);
        RenderSettings.fog = options.fogLevel == 1;
    }
}

public class GameOptions
{
    public int fogLevel;
    public int qualityLevel;
    internal float volumeLevel;
}
