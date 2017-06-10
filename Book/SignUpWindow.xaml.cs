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
using System.Data.SqlTypes;

namespace Book
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window

    {
        public SignUpWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            genderComboBox.Items.Add("Male");
            genderComboBox.Items.Add("Female");

        }

        private void comboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {


        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(firstNameTextBox.Text) && string.IsNullOrWhiteSpace(firstNameTextBox.Text) &&
               string.IsNullOrEmpty(surnameTextBox.Text) && string.IsNullOrWhiteSpace(surnameTextBox.Text) &&
               string.IsNullOrEmpty(birthdayTextBox.Text) && string.IsNullOrWhiteSpace(birthdayTextBox.Text) &&
               string.IsNullOrEmpty(genderComboBox.Text) && string.IsNullOrWhiteSpace(genderComboBox.Text))


            { MessageBox.Show("Complete all the lines"); }

            if (!string.IsNullOrEmpty(firstNameTextBox.Text) && !string.IsNullOrWhiteSpace(firstNameTextBox.Text) &&
                 !string.IsNullOrEmpty(surnameTextBox.Text) && !string.IsNullOrWhiteSpace(surnameTextBox.Text) &&
               !string.IsNullOrEmpty(birthdayTextBox.Text) && !string.IsNullOrWhiteSpace(birthdayTextBox.Text) &&
                !string.IsNullOrEmpty(genderComboBox.Text) && !string.IsNullOrWhiteSpace(genderComboBox.Text))


            {
                MainWindow.sqlConnection = new SqlConnection(LoginWindow.connectionSString);
                await MainWindow.sqlConnection.OpenAsync();
                SqlCommand command = new SqlCommand("INSERT INTO Users(FirstName,Surname,Birthday,Gender,Password)VALUES(@FirstName,@Surname,@Birthday,@Gender,@Password)", MainWindow.sqlConnection);
                command.Parameters.AddWithValue("FirstName", firstNameTextBox.Text);
                command.Parameters.AddWithValue("Surname", surnameTextBox.Text);
                command.Parameters.AddWithValue("Birthday", Convert.ToDateTime(birthdayTextBox.Text));
                command.Parameters.AddWithValue("Gender", Convert.ToString(genderComboBox.Text));
                command.Parameters.AddWithValue("Password", Encryption.GetHashString(passwordBox.Password));

                await command.ExecuteNonQueryAsync();


                MessageBox.Show("You have enrolled in the program book");

                LoginWindow show = new LoginWindow();
                show.Show();
                this.Close();
            }
        }
    }
}
