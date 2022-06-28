using System.Windows;


namespace Messenger
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        AppContext db;

        public RegistrationWindow()
        {
            InitializeComponent();
            db = new AppContext();

        }


        private void Button_Registration_Click(object sender, RoutedEventArgs ee)
        {
            string login = field_Login.Text;
            string password_1 = field_password.Text;
            string password_2 = field_password2.Text;

            if (!password_1.Equals(password_2))
            {
                MessageBox.Show("Пароли не совпдают!");
                field_password.Text = "";
                field_password2.Text = "";
            }
            else if (login == "" || password_1 == ""){
                MessageBox.Show("Нельзя оставлять поля пустыми!");
            }
            else { 
                User user = new User(login, password_1);
                db.Users.Add(user);
                db.SaveChanges();
            }

          
     

        }
    }
}
