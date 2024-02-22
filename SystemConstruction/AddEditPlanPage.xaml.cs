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
    public partial class AddEditPlanPage : Page
    {
        // Идентификатор плана строительства.
        private int planId;
        // Идентификатор здания.
        private int buildingId;
        // Действие (добавление или редактирование).
        public int action;
        // Название здания.
        public string buildingName;
        // Название статуса.
        public string statusName;
        public AddEditPlanPage(int planId, int action, int buildingId)
        {
            InitializeComponent();
            // Присваивание переданных параметров полям класса.
            this.planId = planId;
            this.buildingId = buildingId;
            this.action = action;
            // Загрузка интерфейса.
            LoadInterface();
        }

        // Обработчик события нажатия кнопки "Добавить/Изменить план".
        private void BtnAddEditPlan_Click(object sender, RoutedEventArgs e)
        {
            // Проверка выбранного действия.
            if (action == 1)
            {
                if (!string.IsNullOrEmpty(PlanNameTB.Text)
                    && !string.IsNullOrEmpty(DateStartPicker.Text)
                    && !string.IsNullOrEmpty(DateEndPicker.Text)
                    && StatusComboBox.SelectedItem != null
                    && BuildingComboBox.SelectedItem != null)
                {
                    // Получение значений для обновления записи
                    string newTaskName = PlanNameTB.Text;
                    DateTime newDateStart = Convert.ToDateTime(DateStartPicker.Text);
                    DateTime newDateEnd = Convert.ToDateTime(DateEndPicker.Text);
                    if (newDateStart <= newDateEnd)
                    {
                        string statusName = StatusComboBox.SelectedItem.ToString();
                        string buildingName = BuildingComboBox.SelectedItem.ToString();
                        int newStatusId = GetStatusIdByName(statusName);
                        int newBuildingId = GetBuildingIdByName(buildingName);

                        // Выполнение соответствующего действия (обновление или добавление записи в базу данных).

                        string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
                        string sqlQuery = "UPDATE Building_plan " +
                      "SET Task_name = @NewTaskName, Date_start = @NewDateStart, Date_end = @NewDateEnd, Status_id = @NewStatusId, Building_id = @NewBuildingId " +
                      "WHERE Id_building_plan = @PlanId";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlCommand command = new SqlCommand(sqlQuery, connection);
                            command.Parameters.AddWithValue("@NewTaskName", newTaskName);
                            command.Parameters.AddWithValue("@NewDateStart", newDateStart);
                            command.Parameters.AddWithValue("@NewDateEnd", newDateEnd);
                            command.Parameters.AddWithValue("@NewStatusId", newStatusId);
                            command.Parameters.AddWithValue("@NewBuildingId", newBuildingId);
                            command.Parameters.AddWithValue("@PlanId", planId);

                            try
                            {
                                connection.Open();
                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Отображение сообщений об успешном или неудачном выполнении операции.
                                    MessageBox.Show("Запись успешно обновлена.");

                                    ViewPlansFrame viewPlanFrame = new ViewPlansFrame(newBuildingId);
                                    ViewPlanFrame.Content = viewPlanFrame;
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
                    else
                    {
                        // Отображение сообщений об успешном или неудачном выполнении операции.
                        MessageBox.Show("Дата начала задачи не может быть позже даты окончания.");
                    }
                }


            }
            else if (action == 0)
            {
                if (!string.IsNullOrEmpty(PlanNameTB.Text)
                    && !string.IsNullOrEmpty(DateStartPicker.Text)
                    && !string.IsNullOrEmpty(DateEndPicker.Text)
                    && StatusComboBox.SelectedItem != null
                    && BuildingComboBox.SelectedItem != null)
                {
                    string newTaskName = PlanNameTB.Text;
                    DateTime newDateStart = Convert.ToDateTime(DateStartPicker.Text);
                    DateTime newDateEnd = Convert.ToDateTime(DateEndPicker.Text);
                    if (newDateStart <= newDateEnd)
                    {
                        string statusName = StatusComboBox.SelectedItem.ToString();
                        string buildingName = BuildingComboBox.SelectedItem.ToString();
                        int newStatusId = GetStatusIdByName(statusName);
                        int newBuildingId = GetBuildingIdByName(buildingName);

                        // Выполнение соответствующего действия (обновление или добавление записи в базу данных).

                        string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
                        string sqlQuery = "INSERT INTO Building_plan (Task_name, Date_start, Date_end, Status_id, Building_id) " +
        "OUTPUT INSERTED.Id_building_plan " +
        "VALUES(@NewTaskName, @NewDateStart, @NewDateEnd, @NewStatusId, @NewBuildingId)";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlCommand command = new SqlCommand(sqlQuery, connection);
                            command.Parameters.AddWithValue("@NewTaskName", newTaskName);
                            command.Parameters.AddWithValue("@NewDateStart", newDateStart);
                            command.Parameters.AddWithValue("@NewDateEnd", newDateEnd);
                            command.Parameters.AddWithValue("@NewStatusId", newStatusId);
                            command.Parameters.AddWithValue("@NewBuildingId", newBuildingId);

                            try
                            {
                                connection.Open();

                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Отображение сообщений об успешном или неудачном выполнении операции.
                                    MessageBox.Show("Запись успешно добавлена.");

                                    ViewPlansFrame viewPlanFrame = new ViewPlansFrame(newBuildingId);
                                    ViewPlanFrame.Content = viewPlanFrame;
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
                    else
                    {
                        MessageBox.Show("Дата начала задачи не может быть позже даты окончания.");
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

        // Метод для получения идентификатора статуса по его имени.
        private int GetStatusIdByName(string statusName)
        {
            // Получение идентификатора статуса из базы данных по его имени.
            int statusId = 1;

            string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
            string sqlQuery = "SELECT Id_status FROM Status WHERE Status_name = @StatusName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@StatusName", statusName);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null) // Проверяем, что результат не null
                    {
                        statusId = (int)result; // Преобразуем результат в int
                    }
                    else
                    {
                        MessageBox.Show("Не удалось найти статус с таким именем.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при получении идентификатора статуса: " + ex.Message);
                }
            }

            return statusId;
        }

        // Метод для загрузки интерфейса страницы.
        private void LoadInterface()
        {
            // Проверка действия.
            if (action == 1)
            {
                PlanActionTB.Text = "ИЗМЕНИТЬ ПЛАН СТРОЙКИ";
                AddEditTB.Text = "Изменить";

                // Загрузка интерфейса для редактирования или добавления плана строительства.

                string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";

                string sqlQuery = @"
                    SELECT bp.Task_name, bp.Date_start, bp.Date_end, s.Status_name, b.Building_name
                    FROM Building_plan bp
                    INNER JOIN Status s ON bp.Status_id = s.Id_status
                    INNER JOIN Building b ON bp.Building_id = b.Id_building
                    WHERE bp.Id_building_plan = @PlanId";


                List<PlanInfo> plansList = new List<PlanInfo>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@PlanId", planId);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            PlanInfo plan = new PlanInfo
                            {
                                Task_name = reader["Task_name"].ToString(),
                                DateStart = Convert.ToDateTime((reader["Date_start"])),
                                DateEnd = Convert.ToDateTime(reader["Date_end"]),
                                BuildingName = reader["Building_name"].ToString(),
                                StatusName = reader["Status_name"].ToString()
                            };

                            buildingName = plan.BuildingName.ToString();
                            statusName = plan.StatusName.ToString();

                            plansList.Add(plan);

                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading plan data: " + ex.Message);
                    }
                }

                foreach (var plan in plansList)
                {
                    PlanNameTB.Text = plan.Task_name.ToString();
                    DateStartPicker.Text = plan.DateStart.ToString();
                    DateEndPicker.Text = plan.DateEnd.ToString();
                }


                List<PlanInfo> buildingList = new List<PlanInfo>();

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
                            PlanInfo building = new PlanInfo
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

                var selectedBuilding = BuildingComboBox.Items.Cast<string>().FirstOrDefault(item => item == buildingName);

                // Если элемент найден, выберите его
                if (selectedBuilding != null)
                {
                    BuildingComboBox.SelectedItem = selectedBuilding;
                }



                List<PlanInfo> statusList = new List<PlanInfo>();

                string statusQuery = "SELECT Status_name FROM Status;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(statusQuery, connection);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            PlanInfo status = new PlanInfo
                            {
                                StatusName = reader["Status_name"].ToString()
                            };

                            statusList.Add(status);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading status data: " + ex.Message);
                    }
                }

                var uniqueStatusNames = statusList.Select(b => b.StatusName).Distinct();

                foreach (var name in uniqueStatusNames)
                {
                    StatusComboBox.Items.Add(name);

                }

                var selectedStatus = StatusComboBox.Items.Cast<string>().FirstOrDefault(item => item == statusName);

                if (selectedStatus != null)
                {
                    StatusComboBox.SelectedItem = selectedStatus;
                }


            }

            // Проверка действия.
            else if (action == 0)
            {
                List<PlanInfo> buildingList = new List<PlanInfo>();
                // Загрузка интерфейса для редактирования или добавления плана строительства.
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
                            PlanInfo building = new PlanInfo
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




                List<PlanInfo> statusList = new List<PlanInfo>();
                
                string statusQuery = "SELECT Status_name FROM Status;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(statusQuery, connection);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            PlanInfo status = new PlanInfo
                            {
                                StatusName = reader["Status_name"].ToString()
                            };

                            statusList.Add(status);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading status data: " + ex.Message);
                    }
                }

                var uniqueStatusNames = statusList.Select(b => b.StatusName).Distinct();

                foreach (var name in uniqueStatusNames)
                {
                    StatusComboBox.Items.Add(name);

                }
            }
        }

        // Обработчик события нажатия кнопки "Назад".
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            // Переход на предыдущую страницу.
            ViewPlansFrame buildingInfoPage = new ViewPlansFrame(buildingId);

            ViewPlanFrame.Content = buildingInfoPage;
        }
    }

    // Класс для хранения информации о плане строительства.
    public class PlanInfo
    {
        // Название задачи.
        public string Task_name { get; set; }
        // Дата начала.
        public DateTime DateStart { get; set; }
        // Дата окончания.
        public DateTime DateEnd { get; set; }
        // Название здания.
        public string BuildingName { get; set; }
        // Название статуса.
        public string StatusName { get; set; }
    }
}
