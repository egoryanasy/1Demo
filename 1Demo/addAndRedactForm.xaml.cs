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
    /// Логика взаимодействия для addAndRedactForm.xaml
    /// </summary>
    public partial class addAndRedactForm : Window
    {
        Staff staff_;
        public addAndRedactForm(Product product = null, int? type = null, Staff staff = null)
        {
            InitializeComponent();
            staff_ = staff;
            if (type == 0)//редактирование
            {
                Add.Visibility = Visibility.Collapsed;
                if (product != null)
                {
                    fillAll(product);
                }
            }
            else//редактирование
            {
                Update.Visibility = Visibility.Collapsed;

            }
            
        }
        void fillAll(Product product)
        {
            article.Text = product.article;
            naimenovanie.Text = product.naimenovanie;
            ed.Text = product.ed;
            price.Text = product.price.ToString();
            postavshik.Text = product.postavshik;
            manufacturer.Text = product.manufacturer;
            category.Text = product.category;
            discount.Text = product.discount.ToString();
            count.Text = product.count.ToString();
            text.Text = product.text;
            photoPath.Text = product.photoPath;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = new MainWindow().getConnectionStr();

            // ⚠️ ОЧЕНЬ ОПАСНО - SQL-ИНЪЕКЦИИ! ⚠️
            string query = $@"
                UPDATE dbo.Товары$
                SET 
                    ед_измерения = '{ed.Text}',
                    цена = {Convert.ToDecimal(price.Text)},
                    поставщик_id = (SELECT поставщик_id FROM dbo.Поставщики$ WHERE Название = '{postavshik.Text}'),
                    производитель_id = (SELECT производитель_id FROM dbo.Производители$ WHERE название = '{manufacturer.Text}'),
                    категория_id = (SELECT категория_id FROM dbo.Категории$ WHERE название = '{category.Text}'),
                    скидка = {Convert.ToDecimal(discount.Text)},
                    количество = {Convert.ToInt32(count.Text)},
                    описание = '{text.Text.Replace("'", "''")}',
                    фото = '{photoPath.Text.Replace("'", "''")}',
                    наименование_id = (SELECT наименование_id FROM dbo.Наименование$ WHERE наименование = '{naimenovanie.Text.Replace("'", "''")}')
                WHERE артикул = '{article.Text.Replace("'", "''")}';
                ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        MessageBox.Show(rowsAffected > 0 ? "Товар обновлен!" : "Товар не найден!");
                        if (staff_ != null)
                        {
                            staff_.stackPanel.Children.Clear();

                            staff_.load();
                        }
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                }
            }
        }
    }
}
