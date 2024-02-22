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
    /*
     Страница для добавления или редактирования бюджетной информации.
     */
    public partial class AddEditBudgetPage : Page
    {
        // Идентификатор бюджета.
        private int budgetId;
        // Идентификатор здания.
        private int buildingId;
        // Действие (добавление или редактирование).
        public int action;
        // Название здания.
        public string buildingName;
        public AddEditBudgetPage(int budgetId, int action, int buildingId)
        {
            InitializeComponent();
            // Присваивание переданных параметров полям класса.
            this.budgetId = budgetId;
            this.buildingId = buildingId;
            this.action = action;
            // Загрузка интерфейса.
            LoadInterface();
        }

        // Метод для загрузки интерфейса страницы.
        private void LoadInterface()
        {
            // Проверка действия.
            if (action == 1)
            {
                // Если действие - редактирование.
                // Установка заголовков и текстов кнопок.
                BudgetActionTB.Text = "ИЗМЕНИТЬ БЮДЖЕТ";
                AddEditTB.Text = "Изменить";

                // Подключение к базе данных и выполнение SQL-запроса для получения информации о бюджете.
                
                string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
                
                string sqlQuery = "SELECT b.Budget_name, b.Revenue, bd.Building_name " +
                  "FROM Budget b " +
                  "INNER JOIN Building bd ON b.Building_id = bd.Id_building " +
                  "WHERE b.Id_budget = @BudgetId";

                List<BudgetInfo> budgetList = new List<BudgetInfo>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@BudgetId", budgetId);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        // Создание списка объектов BudgetInfo для хранения результатов запроса.
                        while (reader.Read())
                        {
                            BudgetInfo budget = new BudgetInfo
                            {
                                BudgetName = reader["Budget_name"].ToString(),
                                Revenue = Convert.ToDecimal(reader["Revenue"]),
                                BuildingName = reader["Building_name"].ToString()
                            };

                            buildingName = budget.BuildingName.ToString();

                            budgetList.Add(budget);

                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading budget data: " + ex.Message);
                    }
                }

                // Заполнение полей интерфейса данными из списка.
                foreach (var budget in budgetList)
                {
                    BudgetNameTB.Text = budget.BudgetName.ToString();
                    BudgetTB.Text = budget.Revenue.ToString();
                }


                List<BudgetInfo> buildingList = new List<BudgetInfo>();

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
                            BudgetInfo budget = new BudgetInfo
                            {
                                BuildingName = reader["Building_name"].ToString()
                            };

                            buildingList.Add(budget);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading budget data: " + ex.Message);
                    }
                }

                // Заполнение полей интерфейса данными из списка.
                var uniqueBuildingNames = buildingList.Select(b => b.BuildingName).Distinct();

                foreach (var name in uniqueBuildingNames)
                {
                    BudgetComboBox.Items.Add(name);
                    
                }

                // Выбор текущего элемента.
                var selectedItem = BudgetComboBox.Items.Cast<string>().FirstOrDefault(item => item == buildingName);

                if (selectedItem != null)
                {
                    BudgetComboBox.SelectedItem = selectedItem;
                }


            }
            else if (action == 0)
            {
                // Если действие - добавление.
                List<BudgetInfo> buildingList = new List<BudgetInfo>();

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
                            BudgetInfo budget = new BudgetInfo
                            {
                                BuildingName = reader["Building_name"].ToString()
                            };

                            buildingList.Add(budget);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading budget data: " + ex.Message);
                    }
                }

                // Заполнение выпадающего списка зданиями.
                var uniqueBuildingNames = buildingList.Select(b => b.BuildingName).Distinct();

                foreach (var name in uniqueBuildingNames)
                {
                    BudgetComboBox.Items.Add(name);

                }
            }
        }

        // Обработчик события нажатия кнопки "Добавить/Изменить бюджет".
        private void BtnAddEditBudget_Click(object sender, RoutedEventArgs e)
        {
            // Проверка выбранного действия.
            if (action == 1)
            {
                // Выполнение соответствующего действия (обновление или добавление записи в базу данных).
                if (!string.IsNullOrEmpty(BudgetNameTB.Text) 
                    && !string.IsNullOrEmpty(BudgetTB.Text) 
                    && BudgetComboBox.SelectedItem != null)
                {
                    // Получение значений для обновления записи
                    string newBudgetName = BudgetNameTB.Text;
                    decimal newRevenue = decimal.Parse(BudgetTB.Text); // Преобразуйте текст в десятичное число
                    string buildingName = BudgetComboBox.SelectedItem.ToString();
                    int newBuildingId = GetBuildingIdByName(buildingName); // Предполагается, что BudgetComboBox связан с объектами типа Building

                    string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
                    string sqlQuery = "UPDATE Budget " +
                                      "SET Budget_name = @NewBudgetName, Revenue = @NewRevenue, Building_id = @NewBuildingId " +
                                      "WHERE Id_budget = @BudgetId";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@NewBudgetName", newBudgetName);
                        command.Parameters.AddWithValue("@NewRevenue", newRevenue);
                        command.Parameters.AddWithValue("@NewBuildingId", newBuildingId);
                        command.Parameters.AddWithValue("@BudgetId", budgetId); // Предполагается, что budgetId определен где-то ранее

                        try
                        {
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Запись успешно обновлена.");

                                ViewBudgetFrame viewBudgetFrame = new ViewBudgetFrame(newBuildingId);
                                ViewBudgetFrame.Content = viewBudgetFrame;
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
            // Проверка выбранного действия.
            else if (action == 0)
            {
                // Выполнение соответствующего действия (обновление или добавление записи в базу данных).
                if (!string.IsNullOrEmpty(BudgetNameTB.Text)
                    && !string.IsNullOrEmpty(BudgetTB.Text)
                    && BudgetComboBox.SelectedItem != null)
                {
                    string newBudgetName = BudgetNameTB.Text;
                    decimal newRevenue = decimal.Parse(BudgetTB.Text); 
                    string buildingName = BudgetComboBox.SelectedItem.ToString();
                    int newBuildingId = GetBuildingIdByName(buildingName); 

                    string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
                    string sqlQuery = "INSERT INTO Budget (Budget_name, Revenue, Building_id) " +
                        "OUTPUT INSERTED.Id_budget " +
                        "VALUES(@NewBudgetName, @NewRevenue, @NewBuildingId)";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@NewBudgetName", newBudgetName);
                        command.Parameters.AddWithValue("@NewRevenue", newRevenue);
                        command.Parameters.AddWithValue("@NewBuildingId", newBuildingId);

                        try
                        {
                            connection.Open();

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Отображение сообщений об успешном или неудачном выполнении операции.
                                MessageBox.Show("Запись успешно добавлена.");


                                ViewBudgetFrame viewBudgetFrame = new ViewBudgetFrame(newBuildingId);
                                ViewBudgetFrame.Content = viewBudgetFrame;
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

        // Обработчик события нажатия кнопки "Назад".
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            // Переход на предыдущую страницу.
            ViewBudgetFrame buildingInfoPage = new ViewBudgetFrame(buildingId);

            ViewBudgetFrame.Content = buildingInfoPage;
        }
    }

    // Класс для хранения информации о бюджете.
    public class BudgetInfo
    {
        // Название бюджета.
        public string BudgetName { get; set; }
        // Доход.
        public decimal Revenue { get; set; }
        // Название здания.
        public string BuildingName { get; set; }
    }
}
