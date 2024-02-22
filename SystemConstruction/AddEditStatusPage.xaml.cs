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
    public partial class AddEditStatusPage : Page
    {
        // Идентификатор статуса.
        private int statusId;
        // Действие (добавление или редактирование).
        public int action;
        // Конструктор класса.
        public AddEditStatusPage(int statusId, int action)
        {
            InitializeComponent();
            // Присваивание переданных параметров полям класса.
            this.statusId = statusId;
            this.action = action;
            // Загрузка интерфейса.
            LoadInterface();
        }

        // Обработчик события нажатия кнопки "Добавить/Изменить статус".
        private void BtnAddEditStatus_Click(object sender, RoutedEventArgs e)
        {
            // Проверка выбранного действия.
            // Выполнение соответствующего действия (обновление или добавление записи в базу данных).
            if (action == 1)
            {
                if (!string.IsNullOrEmpty(StatusNameTB.Text))
                {
                    string newStatusName = StatusNameTB.Text;

                    string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
                    string sqlQuery = "UPDATE Status " +
                      "SET Status_name = @NewStatusName " +
                      "WHERE Id_status = @StatusId";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@NewStatusName", newStatusName);
                        command.Parameters.AddWithValue("@StatusId", statusId);

                        try
                        {
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Отображение сообщений об успешном или неудачном выполнении операции.
                                MessageBox.Show("Запись успешно обновлена.");

                                SelectBuildingPage viewStatusFrame = new SelectBuildingPage();
                                ViewStatusFrame.Content = viewStatusFrame;
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

            // Проверка выбранного действия.
            // Выполнение соответствующего действия (обновление или добавление записи в базу данных).
            else if (action == 0)
            {
                if (!string.IsNullOrEmpty(StatusNameTB.Text))
                {
                    string newStatusName = StatusNameTB.Text;

                    string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
                    string sqlQuery = "INSERT INTO Status (Status_name) " +
                        "OUTPUT INSERTED.Id_status " +
                        "VALUES(@NewStatusName)";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@NewStatusName", newStatusName);

                        try
                        {
                            connection.Open();

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Отображение сообщений об успешном или неудачном выполнении операции.
                                MessageBox.Show("Запись успешно добавлена.");

                                SelectBuildingPage viewStatusFrame = new SelectBuildingPage();
                                ViewStatusFrame.Content = viewStatusFrame;
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

        // Метод для загрузки интерфейса страницы.
        private void LoadInterface()
        {
            // Проверка действия.
            
            if (action == 1)
            {
                // Загрузка интерфейса для редактирования статуса.
                StatusActionTB.Text = "ИЗМЕНИТЬ СТАТУС";
                AddEditTB.Text = "Изменить";

                string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";

                string sqlQuery = "SELECT Status_name " +
                  "FROM Status " +
                  "WHERE Id_status = @StatusId";

                List<StatusInfo> statusList = new List<StatusInfo>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@StatusId", statusId);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            StatusInfo status = new StatusInfo
                            {
                                Status_name = reader["Status_name"].ToString()
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

                foreach (var status in statusList)
                {
                    StatusNameTB.Text = status.Status_name.ToString();
                }
            }
        }

        // Обработчик события нажатия кнопки "Назад".
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            // Переход на предыдущую страницу.
            ViewStatusesPage viewStatusesPage = new ViewStatusesPage();

            ViewStatusFrame.Content = viewStatusesPage;
        }
    }

    // Класс для хранения информации о статусе.
    public class StatusInfo
    {
        // Название статуса.
        public string Status_name { get; set; }
    }
}