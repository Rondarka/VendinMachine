using Microsoft.Win32;
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
using System.IO;

namespace VendingMachines.pages
{
    /// <summary>
    /// Логика взаимодействия для newDrink.xaml
    /// </summary>
    public partial class newDrink : Page
    {
        int venid;
        public newDrink(int VendingMachineId)
        {
            InitializeComponent();
            venid = VendingMachineId;
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
                picpath.Text = System.IO.Path.GetFileName(ImageFileDialog.FileName);
                img = File.ReadAllBytes(ImageFileDialog.FileName);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VendingMachinesEntities database = new VendingMachinesEntities();
            Drinks drinkadd = new Drinks { Name = DrinkName.Text, Image = img, Cost = Convert.ToDecimal(DrinkCost.Text) };
            database.Drinks.Add(drinkadd);
            int drkid = database.Drinks.Max(id => id.Id);
            VendingMachineDrinks drinkcount = new VendingMachineDrinks { VendingMachineId = venid, DrinksId = drkid + 1, Count = Convert.ToInt32(DrinkCount.Text) };

            database.VendingMachineDrinks.Add(drinkcount);

            Report reportadd = new Report { VendingMachineId = venid, DrinkId = drkid + 1, AfterUpdate = Convert.ToInt32(DrinkCount.Text), Profit = 0 };
            database.Report.Add(reportadd);

            database.SaveChanges();
            MessageBox.Show("Напиток успешно добавлен.");
        }
    }
}
