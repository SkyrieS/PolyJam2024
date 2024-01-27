using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private InputKeybindData keyboardControlls;
    [SerializeField] private InputKeybindData joysticControlls;

    private bool enableInputs = true;

    private InputKeybindData _firstPlayerKeybinds;
    private InputKeybindData _secondPlayerKeybinds;

    private InputEvents _firstPlayerEvents;
    private InputEvents _secondPlayerEvents;

    private bool firstPlayerControllerConnected;
    private bool secondPlayerControllerConnected;

    private void Start()
    {
        _firstPlayerKeybinds = keyboardControlls;
        _secondPlayerKeybinds = joysticControlls;
        //StartCoroutine(CheckForControllers());
    }

    public void InitializeInputs(InputEvents firstPlayerEvents, InputEvents secondPlayerEvents)
    {
        _firstPlayerEvents = firstPlayerEvents;
        _secondPlayerEvents = secondPlayerEvents;
    }

    public void ToggleInputs(bool enable)
    {
        enableInputs = enable;
    }

    private void Update()
    {
        if (!enableInputs)
            return;

        if(Input.GetKeyDown(_firstPlayerKeybinds.pickLeftKeyCode))
        {
            _firstPlayerEvents?.onPickLeft.Invoke(true);
        }

        if (Input.GetKeyDown(_firstPlayerKeybinds.pickRightKeyCode))
        {
            _firstPlayerEvents?.onPickRight.Invoke(true);
        }

        if (Input.GetKeyDown(_secondPlayerKeybinds.pickLeftKeyCode))
        {
            _secondPlayerEvents?.onPickLeft.Invoke(true);
        }

        if (Input.GetKeyDown(_secondPlayerKeybinds.pickRightKeyCode))
        {
            _secondPlayerEvents?.onPickRight.Invoke(true);
        }

        if (Input.GetKeyUp(_firstPlayerKeybinds.pickLeftKeyCode))
        {
            _firstPlayerEvents?.onPickLeft.Invoke(false);
        }

        if (Input.GetKeyUp(_firstPlayerKeybinds.pickRightKeyCode))
        {
            _firstPlayerEvents?.onPickRight.Invoke(false);
        }

        if (Input.GetKeyUp(_secondPlayerKeybinds.pickLeftKeyCode))
        {
            _secondPlayerEvents?.onPickLeft.Invoke(false);
        }

        if (Input.GetKeyUp(_secondPlayerKeybinds.pickRightKeyCode))
        {
            _secondPlayerEvents?.onPickRight.Invoke(false);
        }

        float firstPlayerLeftHandHorizontalInput = Input.GetAxis(_firstPlayerKeybinds.horizontalLeftHandInputAxis);
        float firstPlayerLeftHandVerticalInput = Input.GetAxis(_firstPlayerKeybinds.verticalLeftHandInputAxis);

        _firstPlayerEvents?.onMovementLeftHand.Invoke(new Vector2(firstPlayerLeftHandHorizontalInput, firstPlayerLeftHandVerticalInput));

        float firstPlayerRightHandHorizontalInput = Input.GetAxis(_firstPlayerKeybinds.horizontalRightHandInputAxis);
        float firstPlayerRightHandVerticalInput = Input.GetAxis(_firstPlayerKeybinds.verticalRightHandInputAxis);

        _firstPlayerEvents?.onMovementRightHand.Invoke(new Vector2(firstPlayerRightHandHorizontalInput, firstPlayerRightHandVerticalInput));

        float secondPlayerLeftHandHorizontalInput = Input.GetAxis(_secondPlayerKeybinds.horizontalLeftHandInputAxis);
        float secondPlayerLeftHandVerticalInput = Input.GetAxis(_secondPlayerKeybinds.verticalLeftHandInputAxis);

        _secondPlayerEvents?.onMovementLeftHand.Invoke(new Vector2(secondPlayerLeftHandHorizontalInput, secondPlayerLeftHandVerticalInput));

        float secondPlayerRightHandHorizontalInput = Input.GetAxis(_secondPlayerKeybinds.horizontalRightHandInputAxis);
        float secondPlayerRightHandVerticalInput = Input.GetAxis(_secondPlayerKeybinds.verticalRightHandInputAxis);

        _secondPlayerEvents?.onMovementRightHand.Invoke(new Vector2(secondPlayerRightHandHorizontalInput, secondPlayerRightHandVerticalInput));
    }


    private void OnDestroy()
    {
        StopAllCoroutines();   
    }

    private IEnumerator CheckForControllers()
    {
        var controllers = Input.GetJoystickNames();
        Debug.Log(controllers.Length);

        if (!firstPlayerControllerConnected && controllers.Length > 0)
        {
            firstPlayerControllerConnected = true;
            _firstPlayerKeybinds = joysticControlls;
            Debug.Log("First controller connected");

        }
        else if (firstPlayerControllerConnected && controllers.Length == 0)
        {
            firstPlayerControllerConnected = false;
            _firstPlayerKeybinds = keyboardControlls;
            Debug.Log("First controller disconnected");
        }

        if (!secondPlayerControllerConnected && controllers.Length > 1)
        {
            secondPlayerControllerConnected = true;
            _secondPlayerKeybinds = joysticControlls;
            Debug.Log("Second controller connected");

        }
        else if (secondPlayerControllerConnected && controllers.Length <= 1)
        {
            secondPlayerControllerConnected = false;
            _secondPlayerKeybinds = keyboardControlls;
            Debug.Log("Second controller disconnected");
        }

        yield return new WaitForSeconds(1f);
    }
}

[System.Serializable]
public class InputKeybindData
{
    public string horizontalLeftHandInputAxis = "Horizontal";
    public string verticalLeftHandInputAxis = "Vertical";
    public KeyCode pickLeftKeyCode;

    public string horizontalRightHandInputAxis = "Horizontal";
    public string verticalRightHandInputAxis = "Vertical";
    public KeyCode pickRightKeyCode;
}

public class InputEvents
{
    public UnityAction<Vector2> onMovementLeftHand;
    public UnityAction<Vector2> onMovementRightHand;
    public UnityAction<bool> onPickLeft;
    public UnityAction<bool> onPickRight;

    public InputEvents(UnityAction<Vector2> onMovementLeftHand, UnityAction<Vector2> onMovementRightHand, UnityAction<bool> onPickLeft, UnityAction<bool> onPickRight)
    {
        this.onMovementLeftHand = onMovementLeftHand;
        this.onMovementRightHand = onMovementRightHand;
        this.onPickLeft = onPickLeft;
        this.onPickRight = onPickRight;
    }
}