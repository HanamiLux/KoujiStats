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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SystemConstruction
{
    /// <summary>
    /// Класс страницы создания здания.
    /// </summary>
    public partial class CreateBuildingPage : Page
    {
        /// <summary>
        /// Конструктор класса CreateBuildingPage.
        /// </summary>
        public CreateBuildingPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Создать здание".
        /// </summary>
        /// <param name="sender">Объект-источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void BtnCreateBuilding_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NewBuildingTB.Text))
            {
                // Получение значений для обновления записи
                string newBuildingName = NewBuildingTB.Text;

                string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
                string sqlQuery = "INSERT INTO Building (Building_name) VALUES(@BuildingName); ";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@BuildingName", newBuildingName);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Запись успешно создана.");

                            SelectBuildingPage buildingPage = new SelectBuildingPage();
                            BuildingsFrame.Content = buildingPage;
                        }
                        else
                        {
                            MessageBox.Show("Не удалось создать запись.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при создании записи: " + ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Назад".
        /// </summary>
        /// <param name="sender">Объект-источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            // Возвращение к предыдущей странице выбора здания.
            SelectBuildingPage selectBuildingPage = new SelectBuildingPage();
            BuildingsFrame.Content = selectBuildingPage;
        }
    }
}