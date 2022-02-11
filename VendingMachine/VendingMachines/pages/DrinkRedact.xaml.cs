using System;
using System.Collections.Generic;
using System.IO;
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

namespace VendingMachines.pages
{
    /// <summary>
    /// Логика взаимодействия для DrinkRedact.xaml
    /// </summary>
    public partial class DrinkRedact : Page
    {
        int drkId;
        int drinkCount;
        public DrinkRedact(int VendingMachineId, int drinkId, string drinkName, int drinkCost)
        {
            InitializeComponent();
            lblName.Content = drinkName;
            txtCost.Text = drinkCost.ToString();
            drkId = drinkId;

            VendingMachinesEntities database = new VendingMachinesEntities();
            var u = database.VendingMachineDrinks.Single(a => a.DrinksId == drinkId);
            var id = u.Id;
            drinkCount = u.Count;
            txtCount.Text = Convert.ToString(u.Count);
        }

        byte[] img;
        private void PicLoad(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ImageFileDialog = new Microsoft.Win32.OpenFileDialog();
            ImageFileDialog.FileName = "Фото";
            ImageFileDialog.DefaultExt = ".png";
            ImageFileDialog.Filter = "Image files (.png)|*.png";
            Nullable<bool> result = ImageFileDialog.ShowDialog();
            if (result == true)
            {
                pathpic.Text = System.IO.Path.GetFileName(ImageFileDialog.FileName);
                img = File.ReadAllBytes(ImageFileDialog.FileName);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VendingMachinesEntities database = new VendingMachinesEntities();
            var reportdrk = database.Report.Single(a => a.DrinkId == drkId);
            reportdrk.AfterUpdate = Convert.ToInt32(txtCount.Text);

            var vnddrk = database.VendingMachineDrinks.Single(a => a.DrinksId == drkId);
            vnddrk.Count = Convert.ToInt32(txtCount.Text);

            var drk = database.Drinks.Single(a => a.Id == drkId);
            drk.Cost = Convert.ToInt32(txtCost.Text);
            drk.Image = img;

            MessageBox.Show("Данные успешно изменены");

            database.SaveChanges();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            VendingMachinesEntities database = new VendingMachinesEntities();
            var drk = database.Drinks.Single(a => a.Id == drkId);
            database.Drinks.Remove(drk);

            var vnddrk = database.VendingMachineDrinks.Single(a => a.DrinksId == drkId);
            database.VendingMachineDrinks.Remove(vnddrk);

            var rptDrink = database.Report.Single(a => a.DrinkId == drkId);
            database.VendingMachineDrinks.Remove(vnddrk);

            MessageBox.Show("Напиток был удален");

            database.SaveChanges();
        }
    }
}
