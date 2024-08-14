using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

namespace Scenes.LoginScene.Scripts
{
    public class UserRegistrationManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField usernameInput; 
        [SerializeField] private TMP_InputField passwordInput;
        
        public TMP_InputField UsernameInput => usernameInput;
        public TMP_InputField PasswordInput => passwordInput;
    
        
        
        
        
        public void OnRegisterButtonClicked()
        {
            // remove eventual spaces before/after username
            string username = usernameInput.text.Trim(); 
            string password = passwordInput.text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Debug.LogError("Username and password cannot be empty.");
                return;
            }
            
            Debug.Log("User register: " + username);
            RegisterUser(username, password);
            
            usernameInput.text = "";
            passwordInput.text = "";
        }


        public void RegisterUser(string username, string password)
        {
            string userDataPath = Path.Combine(Application.dataPath, "CORE", "UserData");
            string path = Path.Combine(userDataPath, "users.txt");

            // Create a salt and hash the username to check against existing entries.
            string salt = GenerateHashManager.GenerateSalt(16); 
            string hashedUsername = GenerateHashManager.GenerateHash(username, salt);

            if (UserExists(hashedUsername, path))
            {
                Debug.LogError("User already exists.");
                return;
            }
            
            // if user doesnt exist we can Hash password with ´salt
            string hashedPassword = GenerateHashManager.GenerateHash(password, salt);
            
            SaveUserToTxtFile(hashedUsername, hashedPassword, salt);
        }
        
        public void ClearInputFields()
        {
            usernameInput.text = "";
            passwordInput.text = "";
        }

        private void SaveUserToTxtFile(string hashedUsername, string hashedPassword, string salt)
        {            
            string userDataPath = Path.Combine(Application.dataPath, "CORE", "UserData");
            string path = Path.Combine(userDataPath, "users.txt");
            string userData = hashedUsername + ";" + hashedPassword + ";" + salt + "\n";
            File.AppendAllText(path, userData);
            Debug.Log("User saved successfully.");
        }
        
        private bool UserExists(string hashedUsername, string path)
        {
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    string[] data = line.Split(';');
                    if (data.Length == 3 && data[0] == hashedUsername)
                    {
                        return true; // User exists
                    }
                }
            }
            return false; // No user found
        }
    }
}