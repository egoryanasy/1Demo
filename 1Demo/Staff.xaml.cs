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
using System.Windows.Shapes;

namespace _1Demo
{
    /// <summary>
    /// Логика взаимодействия для Staff.xaml
    /// </summary>
    public partial class Staff : Window
    {
        MainWindow main_;
        public Staff(User user, MainWindow main)
        {
            InitializeComponent();
            loginName.Text = user.fullName;
            main_ = main;
            load();
            if (user.role == "Guest")
            {
                find.Visibility = Visibility.Hidden;
                sortComboBox.Visibility = Visibility.Hidden;
                sortByManufacturerCB.Visibility = Visibility.Hidden;
            }
            
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            main_.Show();
        }
        void load()
        {
            List<string> manufacturers = new List<string>();
            string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=3hoursEliseev;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT        dbo.Товары$.артикул, dbo.Наименование$.наименование, dbo.Товары$.ед_измерения, dbo.Товары$.цена, dbo.Поставщики$.Название, dbo.Производители$.название, dbo.Категории$.название AS Expr1, 
                         dbo.Товары$.скидка, dbo.Товары$.количество, dbo.Товары$.описание, dbo.Товары$.фото
FROM            dbo.Категории$ INNER JOIN
                         dbo.Товары$ ON dbo.Категории$.категория_id = dbo.Товары$.категория_id INNER JOIN
                         dbo.Наименование$ ON dbo.Товары$.наименование_id = dbo.Наименование$.наименование_id INNER JOIN
                         dbo.Поставщики$ ON dbo.Товары$.поставщик_id = dbo.Поставщики$.поставщик_id INNER JOIN
                         dbo.Производители$ ON dbo.Товары$.производитель_id = dbo.Производители$.производитель_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                article = reader.GetValue(0) as string,
                                naimenovanie = reader.GetValue(1) as string,
                                ed = reader.GetValue(2) as string,
                                price = reader.GetInt32(3),
                                postavshik = reader.GetValue(4) as string,
                                manufacturer = reader.GetValue(5) as string,
                                category = reader.GetValue(6) as string,
                                discount = reader.GetInt32(7),
                                count = reader.GetInt32(8),
                                text = reader.GetValue(9) as string,
                                photoPath = reader.GetValue(10) as string
                            };
                            productCard productCard = new productCard(product);
                            stackPanel.Children.Add(productCard);
                            manufacturers.Add(product.manufacturer);
                        }
                    }
                }
            }
            manufacturers = manufacturers.Distinct().ToList();
            sortByManufacturerCB.Items.Add("Все производители");
            foreach (var item in manufacturers)
            {
                sortByManufacturerCB.Items.Add(item);
            }
            
        }

        private void find_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = ((TextBox)sender).Text;
            foreach (var child in stackPanel.Children)
            {
                if (child is productCard card)
                {
                    if (card.hasMatches(query))
                    {
                        card.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        card.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            switch (((ComboBox)sender).SelectedIndex)
            {
                case 0:
                    List<productCard> cards = new List<productCard>();
                    foreach (var item in stackPanel.Children)
                    {
                        if (item is productCard card)
                            cards.Add(card);
                    }
                    cards = cards.OrderBy(p => p.getProduct().count).ToList();
                    stackPanel.Children.Clear();
                    foreach (var item in cards)
                    {
                        stackPanel.Children.Add(item);
                    }
                    break;
                case 1:
                    List<productCard> cardsdesc = new List<productCard>();
                    foreach (var item in stackPanel.Children)
                    {
                        if (item is productCard card)
                            cardsdesc.Add(card);
                    }
                    cards = cardsdesc.OrderByDescending(p => p.getProduct().count).ToList();
                    stackPanel.Children.Clear();
                    foreach (var item in cards)
                    {
                        stackPanel.Children.Add(item);
                    }
                    break;
            }
        }

        private void sortByManufacturerCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            switch (((ComboBox)sender).SelectedIndex)
            {
                case 0:
                    foreach (var item in stackPanel.Children)
                    {
                        ((productCard)item).Visibility = Visibility.Visible;
                    }
                    break;
                default:
                    string query = ((ComboBox)sender).SelectedValue.ToString();

                    foreach (var child in stackPanel.Children)
                    {
                        if (child is productCard card)
                        {
                            if (card.hasMatches(query))
                            {
                                card.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                card.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                    break;

            }
        }
    }

}
