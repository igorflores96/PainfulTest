using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUiManager : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _backMenuButton;
    [SerializeField] private GameObject _panelOptions;
    [SerializeField] private GameObject _startButtons;


    private void OnEnable() 
    {
        _playButton.onClick.AddListener(PlayButtonClicked);
        _optionsButton.onClick.AddListener(OptionsButtonClicked);
        _backMenuButton.onClick.AddListener(BackButtonClicked);
    }

    private void OnDisable() 
    {
        _playButton.onClick.RemoveListener(PlayButtonClicked);
        _optionsButton.onClick.RemoveListener(OptionsButtonClicked);
        _backMenuButton.onClick.RemoveListener(BackButtonClicked);   
    }

    private void PlayButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OptionsButtonClicked()
    {
        _panelOptions.SetActive(true);
        _startButtons.SetActive(false);

    }

    private void BackButtonClicked()
    {
        _panelOptions.SetActive(false);
        _startButtons.SetActive(true);
    }
}
