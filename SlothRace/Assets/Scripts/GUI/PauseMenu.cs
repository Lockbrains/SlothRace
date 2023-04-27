using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject backButton;
    public GameObject bgmSlider;

    public Button btn_back;
    public Button btn_quit;
    public Button btn_restart;

    public Slider slider_bgm;
    public Slider slider_sfx;

    public Text bgmVolume_text;
    public Text sfxVolume_text;

    public bool isOnPause;

    // Start is called before the first frame update

    void OnEnable()
    {

        if (isOnPause) EventSystem.current.SetSelectedGameObject(backButton);
        
        else EventSystem.current.SetSelectedGameObject(bgmSlider);
    }

    // Update is called once per frame
    void Update()
    {
        slider_bgm.interactable = !isOnPause;
        slider_sfx.interactable = !isOnPause;
        btn_back.interactable = isOnPause;
        btn_quit.interactable = isOnPause;
        btn_restart.interactable = isOnPause;

        SoundManager.S.bgmVolume = slider_bgm.value;
        SoundManager.S.sfxVolume = slider_sfx.value;

        bgmVolume_text.text = ((int)(slider_bgm.value * 100)).ToString();
        sfxVolume_text.text = ((int)(slider_sfx.value * 100)).ToString();
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Beta Test");
    }
}
