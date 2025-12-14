using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _1Demo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Password.Password;

            string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=3hoursEliseev;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT        dbo.Пользователи$.ФИО, dbo.Роли$.название, dbo.Пользователи$.логин, dbo.Пользователи$.пароль
                        FROM            dbo.Пользователи$ INNER JOIN
                         dbo.Роли$ ON dbo.Пользователи$.Роль_id = dbo.Роли$.роль_id
                                    WHERE логин = '" + login + "' and пароль = '" + password + "'";
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User user = new User
                            {
                                fullName = reader.GetValue(0) as string,
                                role = reader.GetValue(1) as string,
                                login = reader.GetValue(2) as string,
                                password = reader.GetValue(3) as string
                            };
                            connection.Close();
                            MessageBox.Show("Успешная авторизация");
                            Staff staff = new Staff(user, this);
                            this.Hide();
                            staff.Show();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка в логине или пароле");
                        }
                    }
                }
            }
        }

        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            User user = new User
            {
                fullName = "Guest",
                login = null,
                password = null,
                role = "Guest"
            };
            MessageBox.Show("Вы вошли как Гость");
            this.Hide();
            new Staff(user, this).Show();
        }
    }
}
