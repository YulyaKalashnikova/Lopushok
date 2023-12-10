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

namespace Lopushok
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        /// <summary>
        /// Глобальные переменные
        /// </summary>
        private List<Product> Products;
        private List<ProductType> ProductTypes;
        private List<string> SortList = new List<string>()
        {
            "Наименование(Up)","Наименование(Down)",
            "Номер производственного цеха(Up)","Номер производственного цеха(Down)",
            "Минимальная стоимость для агента(Up)","Минимальная стоимость для агента(Down)"
        };
        private const int COUNT_TAKE = 20;
        private int CountTab = 0;
        private int CountSkip = 0;
        private int Step = 0;
        public MainPage()
        {
            InitializeComponent();
            Load();
            LoadData();
        }

        /// <summary>
        /// 1. Постраничный вывод
        /// 2. Фильтрация
        /// 3. Сортировка
        /// 4. Поиск
        /// </summary>
        private void LoadData()
        {
            Products = Helper.context.Product.ToList();
            CmbFilter.ItemsSource = ProductTypes;
            CmbSort.ItemsSource = SortList;
            Products = Products.Skip(CountSkip).Take(COUNT_TAKE).ToList();
            switch (CmbSort.SelectedIndex)
            {
                case 0:
                    Products = Products.OrderBy(x => x.Title).ToList();
                    break;
                case 1:
                    Products = Products.OrderByDescending(x => x.Title).ToList();
                    break;
                case 2:
                    Products = Products.OrderBy(x => x.ProductionWorkshopNumber).ToList();
                    break;
                case 3:
                    Products = Products.OrderByDescending(x => x.ProductionWorkshopNumber).ToList();
                    break;
                case 4:
                    Products = Products.OrderBy(x => x.MinCostForAgent).ToList();
                    break;
                case 5:
                    Products = Products.OrderByDescending(x => x.MinCostForAgent).ToList();
                    break;
            }
            if (CmbFilter.SelectedIndex != 0)
            {
                Products = Products.Where(x => x.ProductType == (CmbFilter.SelectedItem as ProductType)).ToList();
            }
            if (TbSearch.Text != string.Empty && TbSearch.Text != "Введите для поиска")
            {
                Products = Products.Where(x => x.Title.Contains(TbSearch.Text) || x.Description.Contains(TbSearch.Text)).ToList();
            }
            ProductsList.ItemsSource = Products;
        }

        /// <summary>
        /// Наполняем наши коллекции контентом
        /// </summary>
        private void Load()
        {
            Products = Helper.context.Product.ToList();
            ProductTypes = Helper.context.ProductType.ToList();
            ProductTypes.Insert(0, new ProductType { Title = "Все типы" });
            CountTab = Products.Count / COUNT_TAKE;
            if ((Products.Count % COUNT_TAKE) != 0)
            {
                CountTab += 1;
            }
            for (int i = 1; i <= CountTab; i++)
            {
                NavPanel.Items.Add(i);
            }
        }
        private void TbDown_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Step == 1)
            {
                return;
            }
            Step -= 1;
            Navigate();
        }

        private void TbNavPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Step = Convert.ToInt16((sender as TextBlock).Text);
            Navigate();
            LoadData();
        }

        private void Navigate()
        {
            if (Step == 1)
            {
                CountSkip = 0;
            }
            else
            {
                CountSkip = (Step - 1) * COUNT_TAKE;
            }
            LoadData();
        }

        private void TbUp_MouseDown(object sender, MouseButtonEventArgs e)
        {//fdgdfg
            if (CountTab == Step)
            {
                return;
            }
            Step += 1;
            Navigate();
        }

        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbSort.SelectedItem != null)
            {
                LoadData();
            }
        }

        private void CmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbFilter.SelectedItem != null)
            {
                LoadData();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TbSearch.Text != "Введите для поиска" && TbSearch.Text != string.Empty)
            {
                LoadData();
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TbSearch.Text == "Введите для поиска")
            {
                TbSearch.Text = "";
            }
        }

        private void BtnUpdateCost_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsList.SelectedItems.Count > 1)
            {
                List<Product> products = new List<Product>();
                foreach (Product product in ProductsList.SelectedItems)
                {
                    products.Add(product);
                }
                new UpdateCostWindow(products).ShowDialog();
            }
            LoadData();
        }
        //По заданию: Необходимо добавить возможность изменения минимальной стоимости продукции для агента сразу для
        //нескольких выбранных продуктов.
        //Выбирать элементы в ListView, через ctrl. За мульти выбор, отвечает SelectionMode="Extended"
        private void ProductsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnUpdateCost.Visibility = ProductsList.SelectedItems.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
