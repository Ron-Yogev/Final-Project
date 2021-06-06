using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RegisterUser : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public InputField confirmInput;
    public Button submitBtn;
    public Button backBtn;
    private const string registerUrl = "http://localhost/UnityBackend/RegisterUser.php";

    // Start is called before the first frame update
    void Start()
    {
        submitBtn.onClick.AddListener(() =>
        {
            string user = usernameInput.text;
            string pass = passwordInput.text;
            string confirm = confirmInput.text;

            if (user.Length<6)
            {
                Toast.Instance.Show("Username should contains at least 6 characters!", 3f, Toast.ToastColor.Red);
                return;
            }
            if (pass.Length < 6)
            {
                Toast.Instance.Show("Password should contains at least 6 characters!", 3f, Toast.ToastColor.Red);
                return;
            }
            if (pass != confirm)
            {
                Toast.Instance.Show("Wrond confirm password!", 3f, Toast.ToastColor.Red);
                return;
            }
            StartCoroutine(Main.instance.web.RegisterUser(registerUrl, usernameInput.text, passwordInput.text));
            Toast.Instance.Show("Creating user...", 3f, Toast.ToastColor.Green);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
            
        });

        backBtn.onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
        });
    }

}
