using MaterialDesignThemes.Wpf;
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
    /// Класс представления для просмотра статусов.
    /// </summary>
    public partial class ViewStatusesPage : Page
    {
        SolidColorBrush RedBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#DC143C"));
        SolidColorBrush WhiteBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFAFA"));
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public ViewStatusesPage()
        {
            InitializeComponent();

            // Получение списка статусов и создание карточек для каждого статуса
            List<StatusData> statusesDataList = GetStatuses();

            foreach (var materialData in statusesDataList)
            {
                var card = CreateBuildingCard(materialData);
                MyListBox.Items.Add(card);
            }
        }

        /// <summary>
        /// Метод для получения списка статусов из базы данных.
        /// </summary>
        /// <returns>Список статусов.</returns>
        public List<StatusData> GetStatuses()
        {
            List<StatusData> statusDataList = new List<StatusData>();

            string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";

            string sqlQuery = "SELECT Id_status, Status_name FROM Status";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        StatusData statusData = new StatusData
                        {
                            Id_status = reader["Id_status"].ToString(),
                            Status_name = reader["Status_name"].ToString()
                        };

                        statusDataList.Add(statusData);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось получить данные" + ex.Message.ToString());
                }
            }

            return statusDataList;
        }

        /// <summary>
        /// Метод для создания карточки статуса.
        /// </summary>
        /// <param name="statusData">Данные о статусе.</param>
        /// <returns>Карточка статуса.</returns>
        private Card CreateBuildingCard(StatusData statusData)
        {
            var card = new Card
            {
                Width = 330,
                Height = 120,
                Margin = new Thickness(0, 0, 0, 50),
                Background = WhiteBrush,
                UniformCornerRadius = 20,
                Content = new StackPanel
                {
                    Margin = new Thickness(10),
                    Orientation = Orientation.Vertical,
                    Children =
            {
                new TextBlock
                {
                    Foreground = System.Windows.Media.Brushes.Black,
                    Text = statusData.Status_name,
                    FontWeight = FontWeights.Bold,
                    FontSize = 20,
                    Margin = new Thickness(30, 0, 0, 10),
                    HorizontalAlignment = HorizontalAlignment.Left
                },
                new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(10),
                    Children =
                    {
                        new Button
                        {
                            Height = 30,
                            Width = 120,
                            Content = "ИЗМЕНИТЬ",
                            FontSize = 16,
                            FontWeight = FontWeights.Medium,
                            Foreground = RedBrush,
                            Style = (Style)Application.Current.FindResource("MaterialDesignFlatButton"),
                            Background = WhiteBrush,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Padding = new Thickness(5, 0, 5, 0),
                            Margin = new Thickness(0, 10, 10, 0),
                            Tag = statusData.Id_status, // Хранение идентификатора статуса
                        },
                        new Button
                        {
                            Height = 30,
                            Width = 120,
                            Content = "УДАЛИТЬ",
                            FontSize = 16,
                            FontWeight = FontWeights.Medium,
                            Foreground = RedBrush,
                            Background = WhiteBrush,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Style = (Style)Application.Current.FindResource("MaterialDesignFlatButton"),
                            Padding = new Thickness(5, 0, 5, 0),
                            Margin = new Thickness(10, 10, 0, 0),
                            Tag = statusData.Id_status, // Хранение идентификатора статуса
                        }
                    }
                }
            }
                }
            };

            // Привязка обработчиков событий к кнопкам в карточке
            var content = card.Content;

            if (content is StackPanel rootStackPanel)
            {
                foreach (var child in rootStackPanel.Children)
                {
                    if (child is StackPanel stackPanel)
                    {
                        foreach (var innerChild in stackPanel.Children)
                        {
                            if (innerChild is Button button)
                            {
                                if (button.Content.ToString() == "ИЗМЕНИТЬ")
                                {
                                    button.Click += BtnEditStatus_Click;
                                }
                                else if (button.Content.ToString() == "УДАЛИТЬ")
                                {
                                    button.Click += BtnDeleteStatus_Click;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Не удалось найти StackPanel в содержимом карточки.");
            }

            return card;
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Добавить статус".
        /// </summary>
        private void BtnAddStatus_Click(object sender, RoutedEventArgs e)
        {
            AddEditStatusPage addEditStatusPage = new AddEditStatusPage(0, 0);

            AddEditStatusesFrame.Content = addEditStatusPage;

        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Изменить статус".
        /// </summary>
        private void BtnEditStatus_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag != null && int.TryParse(button.Tag.ToString(), out int statusId))
            {
                AddEditStatusPage addEditStatusPage = new AddEditStatusPage(statusId, 1);

                AddEditStatusesFrame.Content = addEditStatusPage;
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Удалить статус".
        /// </summary>
        private void BtnDeleteStatus_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag != null && int.TryParse(button.Tag.ToString(), out int statusId))
            {
                string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";

                string sqlQuery = "DELETE FROM Status WHERE Id_status = @StatusId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@StatusId", statusId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Удаление элемента из списка
                            foreach (var item in MyListBox.Items)
                            {
                                if (item is Card card && card.Tag != null && card.Tag.ToString() == statusId.ToString())
                                {
                                    MyListBox.Items.Remove(item);
                                    break;
                                }
                            }

                            MessageBox.Show("Статус успешно удален.");

                            // После удаления обновляем данные в ListBox
                            MyListBox.Items.Clear();
                            List<StatusData> updatedStatusesDataList = GetStatuses();
                            foreach (var statusData in updatedStatusesDataList)
                            {
                                var card = CreateBuildingCard(statusData);
                                MyListBox.Items.Add(card);
                            }

                            // ПЕРЕХОД НА СТРАНИЦУ ПОСЛЕ УДАЛЕНИЯ

                            SelectBuildingPage frame = new SelectBuildingPage();
                            ViewBuilding.Content = frame;
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить статус.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при удалении статуса: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Ошибка при получении идентификатора материала.");
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Назад".
        /// </summary>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            SelectBuildingPage selectBuildingPage = new SelectBuildingPage();
            ViewBuilding.Content = selectBuildingPage;
        }
    }

    /// <summary>
    /// Класс данных о статусе.
    /// </summary>
    public class StatusData
    {
        public string Id_status { get; set; }
        public string Status_name { get; set; }
    }
}