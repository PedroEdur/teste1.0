using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
  
  private AudioManager systemSource;
  private List<AudioSource> activeSource;
  #region Singletion
  
  
  
  public static AudioManager Instance;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance =  this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
    
  }
   #endregion
}
