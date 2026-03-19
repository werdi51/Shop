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

namespace Pract_SQL3.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddTovar.xaml
    /// </summary>
    public partial class AddTovar : Page
    {

        private Tovar _tovae = new Tovar();
        public AddTovar()
        {
            InitializeComponent();
            DataContext = _tovae;
        }

        private void Add_Tovar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_tovae.Name) && string.IsNullOrEmpty(_tovae.Description) && _tovae.Price == 0 && _tovae.Count == 0)
            {
                MessageBox.Show("заполните поля");
            }
            else
            {
                {
                    DB.Context.Tovar.Add(_tovae);

                    try
                    {
                        DB.Context.SaveChanges();
                        MessageBox.Show("Saved");
                        NavigationService.Navigate(new SellerPagr());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Error");
                    }
                }
            }
        }
    }
}
