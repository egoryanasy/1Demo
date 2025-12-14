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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _1Demo
{
    /// <summary>
    /// Логика взаимодействия для productCard.xaml
    /// </summary>
    public partial class productCard : UserControl
    {
        Product product_;
        public productCard(Product product)
        {
            InitializeComponent();
            product_ = product;
            title.Text = product.category + " | " + product.naimenovanie;
            opisanie.Text = "Описание товара: " + product.text;
            manufacturer.Text = "Поставщик: " + product.manufacturer;
            postavshik.Text = "Поставщик: " + product.postavshik;
            if (product.discount != 0)
            {
                if (product.discount > 15)
                {
                    this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E8B57"));
                }
                price.Text = product.price.ToString();
                price.TextDecorations = TextDecorations.Strikethrough;
                price.Foreground = Brushes.Red;
                priceWithDiscount.Text = (product.price * (1 - (product.discount / 100.0))).ToString();
            }
            else
            {
                price.Text = "Цена: " + product.price.ToString();
            }
            ed.Text = "Единица измерения: " + product.ed;
            count.Text = "Количество на сколаде: " + product.count.ToString();
            discount.Text = "Действующая скидка:\n" + product.discount.ToString()+"%";
            
            if (product.photoPath != null)
            {
                Photo.Source = new BitmapImage(new Uri(@"/photoes/" + product.photoPath, UriKind.RelativeOrAbsolute));
            }
            else
            {
                Photo.Source = new BitmapImage(new Uri(@"/photoes/picture.png", UriKind.RelativeOrAbsolute));
            };
            
        }

        public bool hasMatches(string query)
        {
            if (product_.text.Contains(query)||
                product_.naimenovanie.Contains(query)||
                product_.manufacturer.Contains(query)||
                product_.postavshik.Contains(query)||
                product_.article.Contains(query)||
                product_.category.Contains(query))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public Product getProduct()
        {
            return product_;
        }
    }
}
