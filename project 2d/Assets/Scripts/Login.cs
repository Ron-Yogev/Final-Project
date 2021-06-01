using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{

    public InputField usernameInput;
    public InputField passwordInput;
    public Button loginBtn;
    public Button createBtn;
    private const string loginUrl = "http://localhost/UnityBackend/Login.php";

    // Start is called before the first frame update
    void Start()
    {
        loginBtn.onClick.AddListener(() =>
        {
            StartCoroutine(Main.instance.web.Login(loginUrl, usernameInput.text, passwordInput.text));
            StartCoroutine(Main.instance.web.getLevelVars("http://localhost/UnityBackend/retrieveVars.php"));
        });

        createBtn.onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Register");
        });
    }

    void Update()
    {
        
    }


}
