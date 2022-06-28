using System.Diagnostics;
using System.Linq;
using System.Windows;


namespace Messenger
{
    /// <summary>
    /// Логика взаимодействия для AutorizationWindow.xaml
    /// </summary>
    public partial class AutorizationWindow : Window
    {
        public AutorizationWindow()
        {
            InitializeComponent();
        }

        private void Button_Autorization_Click(object sender, RoutedEventArgs e)
        {
            string login = field_Login_Aut.Text;
            string password = field_password_Aut.Text;

            User authUser = null;
            using (AppContext context = new AppContext()) { 
                authUser = context.Users.Where(b=> b.Login == login && b.Password == password).FirstOrDefault();
            }

            ChatWindow chatWindow = new ChatWindow();
            if (authUser != null)
            {
                this.Close();
                chatWindow.nameSet(authUser.ToString());
                Debug.WriteLine("\t"+authUser.ToString()+"\t");
                chatWindow.Show();
            }
            //MessageBox.Show("соси сука");
            else
                MessageBox.Show("Данные введены неверно!");
        }

    }
}
