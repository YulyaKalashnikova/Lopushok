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

namespace Lopushok
{
    /// <summary>
    /// Логика взаимодействия для UpdateCostWindow.xaml
    /// </summary>
    public partial class UpdateCostWindow : Window
    {
        private List<Product> Products;
        public UpdateCostWindow(List<Product> products)
        {
            InitializeComponent();
            Products = products;
            TbCost.Text = Products.Average(x => x.MinCostForAgent).ToString();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(TbCost.Text, out decimal cost))
            {
                MessageBox.Show("Стоимость имеет неправильный формат!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            foreach (var item in Products)
            {
                item.MinCostForAgent = cost;
            }
            Helper.context.SaveChanges();
            Close();
        }
    }
}
