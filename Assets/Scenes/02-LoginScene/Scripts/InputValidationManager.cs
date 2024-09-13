using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace Scenes._02_LoginScene.Scripts
{
    /// <summary>
    /// Manages validation of user input for username and password fields and updates the UI accordingly.
    /// </summary>
    public class InputValidationManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField usernameInput;
        [SerializeField] private TextMeshProUGUI usernameFeedback;
        [SerializeField] private TMP_InputField passwordInput;
        [SerializeField] private TextMeshProUGUI passwordTooShortFeedback;
        [SerializeField] private TextMeshProUGUI passwordTooLongFeedback;
        [SerializeField] private TextMeshProUGUI passwordUppercaseFeedback;
        [SerializeField] private TextMeshProUGUI passwordLowercaseFeedback;
        [SerializeField] private TextMeshProUGUI passwordNumberFeedback;
        [SerializeField] private TextMeshProUGUI passwordSpecialFeedback;
        
        // Define username requirements as const
        
        private const int MinUsernameLength = 3;
        private const int MaxUsernameLength = 20;
        private const string UsernamePattern = @"^[a-z\d.\-@_]+$";
        
        // Define password requirements as const
        private const int MinPasswordLength = 8;
        private const int MaxPasswordLength = 30;
        private const string UppercasePattern = @"[A-Z]";
        private const string LowercasePattern = @"[a-z]";
        private const string NumberPattern = @"[\d]";
        private const string SpecialCharPattern = @"[\W_]";

        /// <summary>
        /// Initialize input field listeners on start.
        /// </summary>
        private void Start()
        {
            usernameInput.onValueChanged.AddListener(delegate { ValidateUsername(usernameInput.text); });
            passwordInput.onValueChanged.AddListener(delegate { ValidatePassword(passwordInput.text); });
        }

        /// <summary>
        /// Validates the username input against defined criteria.
        /// </summary>
        /// <param name="input">The username input from the user.</param>
        private void ValidateUsername(string input)
        {
            bool isLengthValid = input.Length is >= MinUsernameLength and <= MaxUsernameLength;
            bool isPatternValid = Regex.IsMatch(input, UsernamePattern, RegexOptions.IgnoreCase);
            bool isValid = isLengthValid && isPatternValid;

            usernameFeedback.text = isValid ? "<color=green>✔ Gyldigt Brugernavn</color>" : "<color=red>Brugernavn skal være 3-20 tegn og kun indeholde bogstaver, tal eller .-_@</color>";
        }

        /// <summary>
        /// Validates the password input against multiple complexity rules.
        /// </summary>
        /// <param name="input">The password input from the user.</param>
        private void ValidatePassword(string input)
        {
            passwordTooShortFeedback.text = input.Length >= MinPasswordLength ? "<color=green>✔ Længde over 8</color>" : "<color=red>✘ Længde minimum 8</color>";
            passwordTooLongFeedback.text = input.Length <= MaxPasswordLength ? "<color=green>✔ Længde under 30</color>" : "<color=red>✘ Længde Maximum 30</color>";
            passwordUppercaseFeedback.text = Regex.IsMatch(input, UppercasePattern) ? "<color=green>✔ Stort bogstav</color>" : "<color=red>✘ Stort bogstav Mangler</color>";
            passwordLowercaseFeedback.text = Regex.IsMatch(input, LowercasePattern) ? "<color=green>✔ Lille bogstav</color>" : "<color=red>✘ Lille bogstav Mangler</color>";
            passwordNumberFeedback.text = Regex.IsMatch(input, NumberPattern) ? "<color=green>✔ Tal</color>" : "<color=red>✘ Tal mangler</color>";
            passwordSpecialFeedback.text = Regex.IsMatch(input, SpecialCharPattern) ? "<color=green>✔ Specialtegn</color>" : "<color=red>✘ Specialtegn Mangler</color>";
        }
    }
}
