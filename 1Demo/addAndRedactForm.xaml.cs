using System;
using System.Collections.Generic;
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
        public addAndRedactForm(Product product = null, int? type = null)
        {
            InitializeComponent();
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
    }
}
