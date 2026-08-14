using Zenject;

namespace PiratesOnline.Infrastructure.Input
{
    public class InputManager: IInitializable
    {
        public static InputManager Instance { get; private set; }
        public InputSystem_Actions Actions { get; private set; }

        public enum InputType
        {
            Player,
            UI
        }

        public void Initialize()
        {
            if (Instance != null)
                return;
            Actions = new InputSystem_Actions();
            Actions.Enable();
            Instance = this;
            ChangeInputMap(InputType.Player);
        }

        public void ChangeInputMap(InputType inputType)
        {
            switch (inputType)
            {
                case InputType.Player:
                    Actions.Player.Enable();
                    Actions.UI.Disable();
                    break;
                case InputType.UI:
                    Actions.UI.Enable();
                    Actions.Player.Disable();
                    break;
            }
        }
    }
}