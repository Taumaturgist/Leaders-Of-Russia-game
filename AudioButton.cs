using UnityEngine;
using UnityEngine.UI;

public class AudioButton : MonoBehaviour
{
    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;
    [SerializeField] private Image icon;

    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioSource musicSource;

    private bool _isSoundOn = true;
    

    private void Awake()
    {
        if (PlayerPrefs.HasKey("sound"))
        {
            var sound = PlayerPrefs.GetInt("sound");
            switch (sound)
            {
                case 0:
                    _isSoundOn = true;
                    break;
                case 1:
                    _isSoundOn= false;
                    break;
            }
            
            SwitchSoundOnOff();
        }
    }

    public void SwitchSoundOnOff()
    {
        _isSoundOn = !_isSoundOn;

        if (_isSoundOn)
        {
            icon.sprite = soundOn;
            musicSource.volume = 0.1f;
            soundSource.volume = 1f;
            PlayerPrefs.SetInt("sound", 1);            
        }
        else
        {
            icon.sprite = soundOff;
            musicSource.volume = 0f;
            soundSource.volume = 0f;
            PlayerPrefs.SetInt("sound", 0);            
        }
    }
}
