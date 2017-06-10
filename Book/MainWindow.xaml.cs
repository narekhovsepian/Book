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
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows.Media.Animation;
using System.Configuration;

namespace Book
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public static SqlConnection sqlConnection;
        public static SqlCommand sqlCommand;
        public static SqlCommandBuilder sqlCommandBuilder;
        public static SqlDataReader sqlDataReader;
        public static DataTable dataTable;
        public static SqlDataAdapter sqlDataAdapter;

        public MainWindow()
        {
            InitializeComponent();

        }

        private async void LoadTableBooks()
        {
            sqlConnection = new SqlConnection(LoginWindow.connectionSString);
            await sqlConnection.OpenAsync();
            sqlDataReader = null;
            sqlCommand = new SqlCommand("Select * FROM [Books]", sqlConnection);
            try
            {
                sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
                {
                    listbox.Items.Add(Convert.ToString(sqlDataReader["Id"]) + "   " + Convert.ToString(sqlDataReader["BookName"]) + "   " + Convert.ToString(sqlDataReader["BookAuthor"]) + "  " + Convert.ToString(sqlDataReader["PublishingHouse"]));
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();


            }

        }

        private void Windows_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTableBooks();
           // PapulateUsers();

            /*   Book.DataBaseBookDataSet dataBaseBookDataSet = ((Book.DataBaseBookDataSet)(this.FindResource("dataBaseBookDataSet")));
               // Load data into the table Books. You can modify this code as needed.
               Book.DataBaseBookDataSetTableAdapters.BooksTableAdapter dataBaseBookDataSetBooksTableAdapter = new Book.DataBaseBookDataSetTableAdapters.BooksTableAdapter();
               dataBaseBookDataSetBooksTableAdapter.Fill(dataBaseBookDataSet.Books);
               System.Windows.Data.CollectionViewSource booksViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("booksViewSource")));
               booksViewSource.View.MoveCurrentToFirst();
               */


        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(textbox1.Text) && string.IsNullOrWhiteSpace(textbox1.Text) &&
                string.IsNullOrEmpty(textbox2.Text) && string.IsNullOrWhiteSpace(textbox2.Text) &&
                string.IsNullOrEmpty(textbox3.Text) && string.IsNullOrWhiteSpace(textbox3.Text))

            { MessageBox.Show("Complete all the lines"); }

            if (!string.IsNullOrEmpty(textbox1.Text) && !string.IsNullOrWhiteSpace(textbox1.Text) &&
            !string.IsNullOrEmpty(textbox2.Text) && !string.IsNullOrWhiteSpace(textbox2.Text) &&
            !string.IsNullOrEmpty(textbox3.Text) && !string.IsNullOrWhiteSpace(textbox3.Text))

            {
                sqlConnection = new SqlConnection(LoginWindow.connectionSString);
                await sqlConnection.OpenAsync();
                SqlCommand command = new SqlCommand("INSERT INTO Books(BookName,BookAuthor,PublishingHouse)VALUES(@BookName,@BookAuthor,@PublishingHouse)", sqlConnection);
                command.Parameters.AddWithValue("BookName", textbox1.Text);
                command.Parameters.AddWithValue("BookAuthor", textbox2.Text);
                command.Parameters.AddWithValue("PublishingHouse", textbox3.Text);

                await command.ExecuteNonQueryAsync();

                MessageBox.Show("Book Added  Sucessfuly");
                listbox.Items.Clear();
                LoadTableBooks();

            }
        }



        private async void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textbox4.Text) && !string.IsNullOrWhiteSpace(textbox4.Text))
            {
                sqlCommand = new SqlCommand("DELETE FROM [Books] Where [BookName]=@BookName", sqlConnection);
                sqlCommand.Parameters.AddWithValue("BookName", textbox4.Text);
                await sqlCommand.ExecuteNonQueryAsync();
                listbox.Items.Clear();
                textbox4.Clear();
                LoadTableBooks();
            }
            else
            {
                Status.IsHitTestVisible = true;
                Status.Content = "Be sure to fill in the name of the book";


            }



        }
        private void PapulateBooks()
        {
            string query = "SELECT a.Name FROM Users a " + "INNER JOIN USersBooks b ON a.Id=b.BookID" +
                   "WHERE b.BookID=@BookID ";
            using (sqlConnection = new SqlConnection(LoginWindow.connectionSString))
            using (sqlCommand = new SqlCommand(query, sqlConnection))
            using (sqlDataAdapter = new SqlDataAdapter(sqlCommand))
            {
                sqlCommand.Parameters.AddWithValue("UserID", listboxUsers.SelectedValue);
                dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                listboxBooks.DataContext = dataTable;


            }

        }
        private void PapulateUsers()
        {
            using (sqlConnection = new SqlConnection(LoginWindow.connectionSString))
            using (sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Users", sqlConnection))
            {
                dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                listboxUsers.DataContext = dataTable;
            }
        }

        private void listboxUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PapulateBooks();
        }
    }
}
