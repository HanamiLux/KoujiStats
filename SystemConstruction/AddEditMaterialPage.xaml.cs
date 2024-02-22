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
    public partial class AddEditMaterialPage : Page
    {
        // Идентификатор материала.
        private int materialId;
        // Идентификатор здания.
        private int buildingId;
        // Действие (добавление или редактирование).
        public int action;
        // Название здания.
        public string buildingName;

        // Конструктор класса.
        public AddEditMaterialPage(int materialId, int action, int buildingId)
        {
            InitializeComponent();
            // Присваивание переданных параметров полям класса.
            this.materialId = materialId;
            this.buildingId = buildingId;
            this.action = action;
            // Загрузка интерфейса.
            LoadInterface();
        }

        // Обработчик события нажатия кнопки уменьшения количества материала.
        private void MinusMaterialBtn_Click(object sender, RoutedEventArgs e)
        {
            // Уменьшение количества материала на единицу и обновление соответствующего поля интерфейса.
            if (int.TryParse(MaterialCountTB.Text, out int materialCount))
            {
                if (materialCount == 0)
                {
                    MaterialCountTB.Text = materialCount.ToString();
                }
                else
                {
                    materialCount--;
                    MaterialCountTB.Text = materialCount.ToString();
                }
            }
            else
            {
                materialCount = 0;
                MaterialCountTB.Text = materialCount.ToString();
            }
        }

        // Обработчик события нажатия кнопки увеличения количества материала.
        private void PlusMaterialBtn_Click(object sender, RoutedEventArgs e)
        {
            // Увеличение количества материала на единицу и обновление соответствующего поля интерфейса.
            if (int.TryParse(MaterialCountTB.Text, out int materialCount))
            {
                materialCount++;
                MaterialCountTB.Text = materialCount.ToString();
            }
            else
            {
                materialCount = 1;
                MaterialCountTB.Text = materialCount.ToString();
            }
        }

        // Обработчик события нажатия кнопки "Добавить/Изменить материал".
        private void BtnAddEditMaterial_Click(object sender, RoutedEventArgs e)
        {
            // Проверка выбранного действия.
            // Выполнение соответствующего действия (обновление или добавление записи в базу данных).
            if (action == 1)
            {
                if (!string.IsNullOrEmpty(MaterialNameTB.Text) 
                    && !string.IsNullOrEmpty(MaterialCountTB.Text)
                    && !string.IsNullOrEmpty(MaterialPricePerOneTB.Text)
                    && MaterialComboBox.SelectedItem != null)
                {
                    // Получение значений для обновления записи
                    string newMaterialName = MaterialNameTB.Text;
                    int newMaterialCount = int.Parse(MaterialCountTB.Text);
                    decimal newPricePerOne = decimal.Parse(MaterialPricePerOneTB.Text); 
                    string buildingName = MaterialComboBox.SelectedItem.ToString();
                    int newBuildingId = GetBuildingIdByName(buildingName);

                    // Подключение к базе данных и выполнение SQL-запроса.
                    string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
                    string sqlQuery = "UPDATE Materials " +
                  "SET Material_name = @NewMaterialName, Material_count = @NewMaterialCount, Price_per_one = @NewPricePerOne, Building_id = @NewBuildingId " +
                  "WHERE Id_materials = @MaterialId";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@NewMaterialName", newMaterialName);
                        command.Parameters.AddWithValue("@NewMaterialCount", newMaterialCount);
                        command.Parameters.AddWithValue("@NewPricePerOne", newPricePerOne);
                        command.Parameters.AddWithValue("@NewBuildingId", newBuildingId);
                        command.Parameters.AddWithValue("@MaterialId", materialId); 

                        try
                        {
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Отображение сообщений об успешном или неудачном выполнении операции.
                                MessageBox.Show("Запись успешно обновлена.");

                                MaterialsFrame viewMaterialFrame = new MaterialsFrame(newBuildingId);
                                ViewMaterialFrame.Content = viewMaterialFrame;
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
                if (!string.IsNullOrEmpty(MaterialNameTB.Text)
                    && !string.IsNullOrEmpty(MaterialCountTB.Text)
                    && !string.IsNullOrEmpty(MaterialPricePerOneTB.Text)
                    && MaterialComboBox.SelectedItem != null)
                {
                    string newMaterialName = MaterialNameTB.Text;
                    int newMaterialCount = int.Parse(MaterialCountTB.Text);
                    decimal newPricePerOne = decimal.Parse(MaterialPricePerOneTB.Text);
                    string buildingName = MaterialComboBox.SelectedItem.ToString();
                    int newBuildingId = GetBuildingIdByName(buildingName);


                    // Подключение к базе данных и выполнение SQL-запроса.
                    string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
                    string sqlQuery = "INSERT INTO Materials (Material_name, Material_count, Price_per_one, Building_id) " +
    "OUTPUT INSERTED.Id_materials " +
    "VALUES(@NewMaterialName, @NewMaterialCount, @NewPricePerOne, @NewBuildingId)";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@NewMaterialName", newMaterialName);
                        command.Parameters.AddWithValue("@NewMaterialCount", newMaterialCount);
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

                                MaterialsFrame viewMaterialFrame = new MaterialsFrame(newBuildingId);
                                ViewMaterialFrame.Content = viewMaterialFrame;
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
            // Инициализация переменной для хранения идентификатора здания.
            int buildingId = 1;

            // Подключение к базе данных и выполнение SQL-запроса для получения идентификатора здания по его имени.
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
                        // Присвоение результата переменной buildingId.
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
                // Если действие - редактирование.
                // Установка заголовков и текстов кнопок.
                MaterialActionTB.Text = "ИЗМЕНИТЬ МАТЕРИАЛ";
                AddEditTB.Text = "Изменить";


                // Подключение к базе данных и выполнение SQL-запроса для получения информации о бюджете.
                string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
                
                string sqlQuery = "SELECT m.Material_name, m.Material_count, m.Price_per_one, bd.Building_name " +
                  "FROM Materials m " +
                  "INNER JOIN Building bd ON m  .Building_id = bd.Id_building " +
                  "WHERE m.Id_materials = @MaterialId";

                List<MaterialInfo> materialsList = new List<MaterialInfo>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaterialId", materialId);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        // Создание списка объектов BudgetInfo для хранения результатов запроса.
                        while (reader.Read())
                        {
                            MaterialInfo material = new MaterialInfo
                            {
                                Material_name = reader["Material_name"].ToString(),
                                Material_count = Convert.ToInt16((reader["Material_count"])),
                                Price_per_one = Convert.ToDecimal(reader["Price_per_one"]),
                                BuildingName = reader["Building_name"].ToString()
                            };

                            buildingName = material.BuildingName.ToString();

                            materialsList.Add(material);

                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading material data: " + ex.Message);
                    }
                }

                // Заполнение полей интерфейса данными из списка.
                foreach (var material in materialsList)
                {
                    MaterialNameTB.Text = material.Material_name.ToString();
                    MaterialCountTB.Text = material.Material_count.ToString();
                    MaterialPricePerOneTB.Text = material.Price_per_one.ToString();
                }


                List<MaterialInfo> buildingList = new List<MaterialInfo>();

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
                            MaterialInfo building = new MaterialInfo
                            {
                                BuildingName = reader["Building_name"].ToString()
                            };

                            buildingList.Add(building);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading budget data: " + ex.Message);
                    }
                }

                // Используем Distinct() для выбора уникальных имен зданий
                var uniqueBuildingNames = buildingList.Select(b => b.BuildingName).Distinct();

                foreach (var name in uniqueBuildingNames)
                {
                    MaterialComboBox.Items.Add(name);

                }

                var selectedItem = MaterialComboBox.Items.Cast<string>().FirstOrDefault(item => item == buildingName);

                // Если элемент найден, выберите его
                if (selectedItem != null)
                {
                    MaterialComboBox.SelectedItem = selectedItem;
                }


            }

            // Если действие - добавление.
            else if (action == 0)
            {
                List<MaterialInfo> buildingList = new List<MaterialInfo>();

                // Подключение к базе данных и выполнение SQL-запроса для получения списка зданий.
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
                            MaterialInfo building = new MaterialInfo
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

                // Заполнение выпадающего списка зданиями.
                var uniqueBuildingNames = buildingList.Select(b => b.BuildingName).Distinct();

                foreach (var name in uniqueBuildingNames)
                {
                    MaterialComboBox.Items.Add(name);

                }
            }
        }

        // Обработчик события нажатия кнопки "Назад".
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            // Переход на предыдущую страницу.
            MaterialsFrame buildingInfoPage = new MaterialsFrame(buildingId);

            ViewMaterialFrame.Content = buildingInfoPage;
        }
    }

    public class MaterialInfo
    {
        public string Material_name { get; set; }
        public int Material_count { get; set; }
        public decimal Price_per_one { get; set; }
        public string BuildingName { get; set; }
    }
}
