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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SystemConstruction
{
    public partial class AddEditResourcePage : Page
    {
        // Идентификатор ресурса.
        private int resourceId;
        // Идентификатор здания.
        private int buildingId;
        // Действие (добавление или редактирование).
        public int action;
        // Название здания.
        public string buildingName;
        // Конструктор класса.
        public AddEditResourcePage(int resourceId, int action, int buildingId)
        {
            InitializeComponent();
            // Присваивание переданных параметров полям класса.
            this.resourceId = resourceId;
            this.buildingId = buildingId;
            this.action = action;
            // Загрузка интерфейса.
            LoadInterface();
        }

        // Обработчик события нажатия кнопки "Уменьшить количество ресурса".
        private void MinusResourceBtn_Click(object sender, RoutedEventArgs e)
        {
            // Уменьшение количества ресурса на единицу.
            if (int.TryParse(ResourceCountTB.Text, out int resourceCount))
            {
                if (resourceCount == 0)
                {
                    ResourceCountTB.Text = resourceCount.ToString();
                }
                else
                {
                    resourceCount--;
                    ResourceCountTB.Text = resourceCount.ToString();
                }
            }
            else
            {
                resourceCount = 0;
                ResourceCountTB.Text = resourceCount.ToString();
            }
        }

        // Обработчик события нажатия кнопки "Увеличить количество ресурса".
        private void PlusResourceBtn_Click(object sender, RoutedEventArgs e)
        {
            // Увеличение количества ресурса на единицу.
            if (int.TryParse(ResourceCountTB.Text, out int resourceCount))
            {
                resourceCount++;
                ResourceCountTB.Text = resourceCount.ToString();
            }
            else
            {
                resourceCount = 1;
                ResourceCountTB.Text = resourceCount.ToString();
            }
        }

        // Обработчик события нажатия кнопки "Добавить/Изменить ресурс".
        private void BtnAddEditResource_Click(object sender, RoutedEventArgs e)
        {
            // Проверка выбранного действия.
            // Выполнение соответствующего действия (обновление или добавление записи в базу данных).
            if (action == 1)
            {
                if (!string.IsNullOrEmpty(ResourceNameTB.Text)
                    && !string.IsNullOrEmpty(ResourceCountTB.Text)
                    && !string.IsNullOrEmpty(ResourcePricePerOneTB.Text)
                    && BuildingComboBox.SelectedItem != null)
                {
                    // Получение значений для обновления записи
                    string newResourceName = ResourceNameTB.Text;
                    int newResourceCount = int.Parse(ResourceCountTB.Text);
                    decimal newPricePerOne = decimal.Parse(ResourcePricePerOneTB.Text);
                    string buildingName = BuildingComboBox.SelectedItem.ToString();
                    int newBuildingId = GetBuildingIdByName(buildingName);

                    // SQL-запрос к базе данных

                    string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
                    string sqlQuery = "UPDATE Resources " +
                  "SET Resource_name = @NewResourceName, Resource_count = @NewResourceCount, Price_per_one_month = @NewPricePerOne, Building_id = @NewBuildingId " +
                  "WHERE Id_resource = @ResourceId";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@NewResourceName", newResourceName);
                        command.Parameters.AddWithValue("@NewResourceCount", newResourceCount);
                        command.Parameters.AddWithValue("@NewPricePerOne", newPricePerOne);
                        command.Parameters.AddWithValue("@NewBuildingId", newBuildingId);
                        command.Parameters.AddWithValue("@ResourceId", resourceId);

                        try
                        {
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Отображение сообщений об успешном или неудачном выполнении операции.
                                MessageBox.Show("Запись успешно обновлена.");

                                ViewResourcesPage viewResourceFrame = new ViewResourcesPage(newBuildingId);
                                ViewResourceFrame.Content = viewResourceFrame;
                            }
                            else
                            {
                                // Отображение сообщений об успешном или неудачном выполнении операции.
                                MessageBox.Show("Не удалось обновить запись.");
                            }
                        }
                        catch (Exception ex)
                        {
                            // Отображение сообщений об успешном или неудачном выполнении операции.
                            MessageBox.Show("Ошибка при обновлении записи: " + ex.Message);
                        }
                    }
                }


            }
            else if (action == 0)
            {
                if (!string.IsNullOrEmpty(ResourceNameTB.Text)
                    && !string.IsNullOrEmpty(ResourceCountTB.Text)
                    && !string.IsNullOrEmpty(ResourcePricePerOneTB.Text)
                    && BuildingComboBox.SelectedItem != null)
                {
                    string newResourceName = ResourceNameTB.Text;
                    int newResourceCount = int.Parse(ResourceCountTB.Text);
                    decimal newPricePerOne = decimal.Parse(ResourcePricePerOneTB.Text);
                    string buildingName = BuildingComboBox.SelectedItem.ToString();
                    int newBuildingId = GetBuildingIdByName(buildingName);

                    // SQL-запрос к базе данных

                    string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
                    string sqlQuery = "INSERT INTO Resources (Resource_name, Resource_count, Price_per_one_month, Building_id) " +
                        "OUTPUT INSERTED.Id_resource " +
                        "VALUES(@NewResourceName, @NewResourceCount, @NewPricePerOne, @NewBuildingId)";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@NewResourceName", newResourceName);
                        command.Parameters.AddWithValue("@NewResourceCount", newResourceCount);
                        command.Parameters.AddWithValue("@NewPricePerOne", newPricePerOne);
                        command.Parameters.AddWithValue("@NewBuildingId", newBuildingId);

                        try
                        {
                            connection.Open();

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Отображение сообщений об успешном или неудачном выполнении операции.
                                MessageBox.Show("Запись успешно добавлена.");

                                ViewResourcesPage viewResourceFrame = new ViewResourcesPage(newBuildingId);
                                ViewResourceFrame.Content = viewResourceFrame;
                            }
                            else
                            {
                                // Отображение сообщений об успешном или неудачном выполнении операции.
                                MessageBox.Show("Не удалось добавить запись.");
                            }
                        }
                        catch (Exception ex)
                        {
                            // Отображение сообщений об успешном или неудачном выполнении операции.
                            MessageBox.Show("Ошибка при обновлении записи: " + ex.Message);
                        }
                    }
                }
            }
        }

        // Метод для получения идентификатора здания по его имени.
        private int GetBuildingIdByName(string buildingName)
        {
            // Получение идентификатора здания из базы данных по его имени.
            int buildingId = 1;

            string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
            string sqlQuery = "SELECT Id_building FROM Building WHERE Building_name = @BuildingName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@BuildingName", buildingName);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null) // Проверяем, что результат не null
                    {
                        buildingId = (int)result; // Преобразуем результат в int
                    }
                    else
                    {
                        MessageBox.Show("Не удалось найти здание с таким именем.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при получении идентификатора здания: " + ex.Message);
                }
            }

            return buildingId;
        }

        // Метод для загрузки интерфейса страницы.
        private void LoadInterface()
        {
            // Проверка действия.
            if (action == 1)
            {
                // Загрузка интерфейса для редактирования или добавления ресурса.
                ResourceActionTB.Text = "ИЗМЕНИТЬ РЕСУРС";
                AddEditTB.Text = "Изменить";


                // SQL-запрос к бд
                string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";

                string sqlQuery = "SELECT r.Resource_name, r.Resource_count, r.Price_per_one_month, bd.Building_name " +
                  "FROM Resources r " +
                  "INNER JOIN Building bd ON r.Building_id = bd.Id_building " +
                  "WHERE r.Id_resource = @ResourceId";


                List<ResourceInfo> resourcesList = new List<ResourceInfo>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@ResourceId", resourceId);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ResourceInfo resource = new ResourceInfo
                            {
                                Resource_name = reader["Resource_name"].ToString(),
                                Resource_count = Convert.ToInt16((reader["Resource_count"])),
                                Price_per_one = Convert.ToDecimal(reader["Price_per_one_month"]),
                                BuildingName = reader["Building_name"].ToString()
                            };

                            buildingName = resource.BuildingName.ToString();

                            resourcesList.Add(resource);

                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading resource data: " + ex.Message);
                    }
                }

                foreach (var resource in resourcesList)
                {
                    ResourceNameTB.Text = resource.Resource_name.ToString();
                    ResourceCountTB.Text = resource.Resource_count.ToString();
                    ResourcePricePerOneTB.Text = resource.Price_per_one.ToString();
                }


                List<ResourceInfo> buildingList = new List<ResourceInfo>();

                string buildingQuery = "SELECT Building_name FROM Building;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(buildingQuery, connection);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ResourceInfo building = new ResourceInfo
                            {
                                BuildingName = reader["Building_name"].ToString()
                            };

                            buildingList.Add(building);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading building data: " + ex.Message);
                    }
                }

                // Используем Distinct() для выбора уникальных имен зданий
                var uniqueBuildingNames = buildingList.Select(b => b.BuildingName).Distinct();

                foreach (var name in uniqueBuildingNames)
                {
                    BuildingComboBox.Items.Add(name);

                }

                var selectedBuilding = BuildingComboBox.Items.Cast<string>().FirstOrDefault(item => item == buildingName);

                // Если элемент найден, выберите его
                if (selectedBuilding != null)
                {
                    BuildingComboBox.SelectedItem = selectedBuilding;
                }


            }
            else if (action == 0)
            {
                List<ResourceInfo> buildingList = new List<ResourceInfo>();

                // SQL-запрос к бд
                string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
                string buildingQuery = "SELECT Building_name FROM Building;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(buildingQuery, connection);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ResourceInfo building = new ResourceInfo
                            {
                                BuildingName = reader["Building_name"].ToString()
                            };

                            buildingList.Add(building);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading building data: " + ex.Message);
                    }
                }

                var uniqueBuildingNames = buildingList.Select(b => b.BuildingName).Distinct();

                foreach (var name in uniqueBuildingNames)
                {
                    BuildingComboBox.Items.Add(name);

                }
            }
        }

        // Обработчик события нажатия кнопки "Назад".
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            // Переход на предыдущую страницу.
            ViewResourcesPage buildingInfoPage = new ViewResourcesPage(buildingId);

            ViewResourceFrame.Content = buildingInfoPage;
        }
    }
    // Класс для хранения информации о ресурсе.
    public class ResourceInfo
    {
        // Название ресурса.
        public string Resource_name { get; set; }
        // Количество ресурса.
        public int Resource_count { get; set; }
        // Цена за один ресурс в месяц.
        public decimal Price_per_one { get; set; }
        // Название здания.
        public string BuildingName { get; set; }
    }
}