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
    /// Класс страницы редактирования названия здания.
    /// </summary>
    public partial class EditBuildingNamePage : Page
    {
        private int buildingId;

        /// <summary>
        /// Конструктор класса EditBuildingNamePage.
        /// </summary>
        /// <param name="buildingId">Идентификатор здания для редактирования.</param>
        public EditBuildingNamePage(int buildingId)
        {
            InitializeComponent();
            this.buildingId = buildingId;
            LoadInterface();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Добавить название".
        /// </summary>
        /// <param name="sender">Объект-источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void BtnAddName_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(BuildingNameTB.Text))
            {
                // Получение нового названия здания.
                string newBuildingName = BuildingNameTB.Text;

                string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
                string sqlQuery = "UPDATE Building " +
                  "SET Building_name = @NewBuildingName " +
                  "WHERE Id_building = @BuildingId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@NewBuildingName", newBuildingName);
                    command.Parameters.AddWithValue("@BuildingId", buildingId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Запись успешно обновлена.");

                            BuildingInfoPage buildingInfoPage = new BuildingInfoPage(buildingId);
                            ViewBuildingFrame.Content = buildingInfoPage;
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить запись.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при обновлении записи: " + ex.Message);
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
            // Возврат к странице информации о здании.
            BuildingInfoPage buildingInfoPage = new BuildingInfoPage(buildingId);

            ViewBuildingFrame.Content = buildingInfoPage;
        }

        /// <summary>
        /// Метод для загрузки интерфейса страницы.
        /// </summary>
        private void LoadInterface()
        {
            // Получение текущего названия здания для отображения в текстовом поле.
            string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";

            string sqlQuery = "SELECT Building_name " +
              "FROM Building " +
              "WHERE Id_building = @BuildingId";

            BuildingInfo statusInfo = new BuildingInfo();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@BuildingId", buildingId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        statusInfo = new BuildingInfo
                        {
                            Building_name = reader["Building_name"].ToString()
                        };
                    }

                    reader.Close();

                    BuildingNameTB.Text = statusInfo.Building_name.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading building data: " + ex.Message);
                }
            }
        }
    }

    /// <summary>
    /// Класс для хранения информации о здании.
    /// </summary>
    public class BuildingInfo
    {
        /// <summary>
        /// Название здания.
        /// </summary>
        public string Building_name { get; set; }
    }
}