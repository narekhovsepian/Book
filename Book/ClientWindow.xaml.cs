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
using System.Data.Sql;
using System.Data.SqlClient;

namespace Book
{
    /// <summary>
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        public ClientWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
          
         //   MainWindow.sqlCommand.CommandText = "Select * from Books Where BookName='" + textboxBookSerarch.Text + "'  ";
           MainWindow.sqlConnection = new SqlConnection(LoginWindow.connectionSString);
            await MainWindow.sqlConnection.OpenAsync();
           MainWindow.sqlDataReader = null;
            MainWindow.sqlCommand = new SqlCommand("Select * from Books Where BookName='" + textboxBookSerarch.Text + "'  ",MainWindow.sqlConnection);
            try
            {
               MainWindow.sqlDataReader = await MainWindow.sqlCommand.ExecuteReaderAsync();
                while (await MainWindow.sqlDataReader.ReadAsync())
                {
                    listboxSerach.Items.Add(Convert.ToString(MainWindow.sqlDataReader["Id"]) + "   " + Convert.ToString(MainWindow.sqlDataReader["BookName"]) + "   " + Convert.ToString(MainWindow.sqlDataReader["BookAuthor"]) + "  " + Convert.ToString(MainWindow.sqlDataReader["PublishingHouse"]));
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (MainWindow.sqlDataReader != null)
                    MainWindow.sqlDataReader.Close();


            }


        }



    }
}
