using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
   [SerializeField] private AudioMixer masterMixer;
   [SerializeField] private Slider masterVolumeSlider;
   [SerializeField] private Slider ambientVolumeSlider;
   [SerializeField] private Slider effectVolumeSlider;
   
   
   public void InitializeSoundSettings()
   {
      if (PlayerPrefs.HasKey("MasterVolume") && PlayerPrefs.HasKey("AmbientVolume") && PlayerPrefs.HasKey("SFXVolume"))
      {
         LoadVolume();
      }
      else
      {
         SetDefaultVolumes();
      }
   }

   private void SetDefaultVolumes()
   {
      SetMasterVolume();
      SetAmbientVolume();
      SetSFXVolume();
   }
   public void SetMasterVolume()
   {
      float volume = masterVolumeSlider.value;
      masterMixer.SetFloat("MasterVolume", MathF.Log10(volume)*20);
      PlayerPrefs.SetFloat("MasterVolume", volume );
   }
   public void SetAmbientVolume()
   {
      float volume = ambientVolumeSlider.value;
      masterMixer.SetFloat("AmbientVolume", MathF.Log10(volume)*20);
      PlayerPrefs.SetFloat("AmbientVolume",  volume);
   }
   public void SetSFXVolume()
   {
      float volume = effectVolumeSlider.value;
      masterMixer.SetFloat("SFXVolume", MathF.Log10(volume)*20);
      PlayerPrefs.SetFloat("SFXVolume",  volume);
   }


   private void LoadVolume()
   {
      masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
      ambientVolumeSlider.value = PlayerPrefs.GetFloat("AmbientVolume");
      effectVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
   }

}
