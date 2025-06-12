using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectController : MonoBehaviour
{
    [SerializeField] private Canvas _selectCanvas;
    [SerializeField] private Canvas _canvasUI;
    [SerializeField] private GameObject _player1;
    [SerializeField] private GameObject _player2;
    [SerializeField] private TMP_Dropdown _dropdownP1;
    [SerializeField] private TMP_Dropdown _dropdownP2;

    private Dictionary<InputDevice, bool> _inputDevicesbyUse = new Dictionary<InputDevice, bool>();
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        foreach (InputDevice device in InputSystem.devices)
        {
            if (device != null)
            {
                _inputDevicesbyUse.Add(device, false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (InputDevice device in InputSystem.devices)
        {
            if (device != null)
            {
                _inputDevicesbyUse[device] = device.enabled;
                Debug.Log(device.name);
            }
        }
    }

    public void Play()
    {
        _selectCanvas.gameObject.SetActive(false);
        _canvasUI.gameObject.SetActive(true);
        Time.timeScale = 1.0f;
    }

    public void DropdownSampleP1()
    {
        switch (_dropdownP1.value)
        {
            case 0:
                _player1.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Keyboard");

                break;

            case 1:
                _player1.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Controller");
                break;
        }
    }

    public void DropdownSampleP2()
    {
        switch (_dropdownP2.value)
        {
            case 0:
                _player2.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Keyboard");
                break;

            case 1:
                _player2.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Controller");
                break;
        }
    }
}
