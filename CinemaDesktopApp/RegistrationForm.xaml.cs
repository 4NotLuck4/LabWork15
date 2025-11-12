using AuthLibrary.Contexts;
using AuthLibrary.Services;
using System.Windows;
using System.Windows.Controls;

namespace CinemaDesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class RegistrationForm : Window
    {
        private readonly AuthService _authService;
        public RegistrationForm()
        {
            InitializeComponent();
            _authService = new AuthService(new DatabaseLibrary());
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            string login = TextBoxLogin.Text.Trim();
            string password = TextBoxPassword.Text.Trim();
            string confirmPassword = TextBoxConfirmPassword.Text.Trim();

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совподают");
                return;
            }
            try
            {
                bool success = _authService.Register(login, password);
                if (success)
                {
                    MessageBox.Show("Регистрация успешна");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Логин уже существует");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBoxPassword_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBoxConfirmPassword_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}