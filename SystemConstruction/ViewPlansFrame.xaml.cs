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
    /// Класс представления для просмотра планов по строительству.
    /// </summary>
    public partial class ViewPlansFrame : Page
    {
        SolidColorBrush RedBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#DC143C"));
        SolidColorBrush WhiteBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFAFA"));
        private int buildingId; // Идентификатор здания

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="buildingId">Идентификатор здания.</param>
        public ViewPlansFrame(int buildingId)
        {
            InitializeComponent();

            this.buildingId = buildingId;

            // Получение списка планов для здания
            List<PlanData> plansDataList = GetPlansForBuilding(buildingId);

            // Создание и заполнение карточек на основе данных
            foreach (var planData in plansDataList)
            {
                var card = CreateBuildingCard(planData);
                MyListBox.Items.Add(card);
            }
        }

        /// <summary>
        /// Метод для получения списка планов для здания.
        /// </summary>
        /// <param name="buildingId">Идентификатор здания.</param>
        /// <returns>Список планов для здания.</returns>
        public List<PlanData> GetPlansForBuilding(int buildingId)
        {
            List<PlanData> plansDataList = new List<PlanData>();

            string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";

            string sqlQuery = @"
                SELECT bp.Id_building_plan, bp.Task_name, bp.Date_start, bp.Date_end, s.Status_name
                FROM Building_plan bp
                INNER JOIN Status s ON bp.Status_id = s.Id_status
                WHERE bp.Building_id = @BuildingId";


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
                        PlanData planData = new PlanData
                        {
                            Id_building_plan = reader["Id_building_plan"].ToString(),
                            Task_name = reader["Task_name"].ToString(),
                            DateStart = reader["Date_start"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["Date_start"],
                            DateEnd = reader["Date_end"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["Date_end"],
                            StatusName = reader["Status_name"].ToString()
                        };

                        plansDataList.Add(planData);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось получить данные" + ex.Message.ToString());
                }
            }

            return plansDataList;
        }

        /// <summary>
        /// Метод для создания карточки плана по строительству.
        /// </summary>
        /// <param name="planData">Данные о плане по строительству.</param>
        /// <returns>Карточка плана по строительству.</returns>
        private Card CreateBuildingCard(PlanData planData)
        {
            var card = new Card
            {
                Width = 350,
                Height = 300,
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
                    Text = planData.Task_name,
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
                        new TextBlock
                        {
                            Text = "НАЧАЛО:",
                            Foreground = System.Windows.Media.Brushes.Black,
                            FontWeight = FontWeights.Bold,
                            FontSize = 20,
                            Margin = new Thickness(15, 5, 0, 0),
                            HorizontalAlignment = HorizontalAlignment.Left
                        },
                        new TextBlock
                        {
                            Text = $"{planData.DateStart.ToShortDateString()}",
                            FontWeight = FontWeights.Medium,
                            FontSize = 18,
                            Background = RedBrush,
                            Foreground = WhiteBrush,
                            Padding = new Thickness(12, 7, 12, 7),
                            Margin = new Thickness(30, 0, 10, 0)
                        }
                    }
                },
                new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(10),
                    Children =
                    {
                        new TextBlock
                        {
                            Text = "ЗАВЕРШЕНИЕ:",
                            Foreground = System.Windows.Media.Brushes.Black,
                            FontWeight = FontWeights.Bold,
                            FontSize = 20,
                            Margin = new Thickness(15, 5, 0, 0),
                            HorizontalAlignment = HorizontalAlignment.Left
                        },
                        new TextBlock
                        {
                            Text = $"{planData.DateEnd.ToShortDateString()}",
                            FontWeight = FontWeights.Medium,
                            FontSize = 18,
                            Background = RedBrush,
                            Foreground = WhiteBrush,
                            Padding = new Thickness(12, 7, 12, 7),
                            Margin = new Thickness(30, 0, 10, 0)
                        }
                    }
                },
                new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(10),
                    Children =
                    {
                        new TextBlock
                        {
                            Text = "СТАТУС:",
                            Width = 120,
                            FontWeight = FontWeights.Medium,
                            Background = RedBrush,
                            Foreground = WhiteBrush,
                            FontSize = 14,
                            Padding = new Thickness(5),
                            Margin = new Thickness(10, 0, 10, 0),
                            HorizontalAlignment = HorizontalAlignment.Left
                        },
                        new TextBlock
                        {
                            Text = $"{planData.StatusName}",
                            FontWeight = FontWeights.Medium,
                            FontSize = 14,
                            Width = 120,
                            Background = RedBrush,
                            Foreground = WhiteBrush,
                            Padding = new Thickness(5),
                            Margin = new Thickness(10, 0, 10, 0),
                            HorizontalAlignment = HorizontalAlignment.Left
                        }
                    }
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
                            Tag = planData.Id_building_plan, // Хранение идентификатора плана
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
                            Tag = planData.Id_building_plan, // Хранение идентификатора плана
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
                                    button.Click += BtnEditPlan_Click;
                                }
                                else if (button.Content.ToString() == "УДАЛИТЬ")
                                {
                                    button.Click += BtnAddPlan_Click;
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
        /// Обработчик события нажатия кнопки "Добавить план".
        /// </summary>
        private void BtnAddPlan_Click(object sender, RoutedEventArgs e)
        {
            AddEditPlanPage addEditPlanPage = new AddEditPlanPage(0, 0, buildingId);

            ViewPlans.Content = addEditPlanPage;

        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Изменить план".
        /// </summary>
        private void BtnEditPlan_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag != null && int.TryParse(button.Tag.ToString(), out int materialId))
            {
                AddEditPlanPage addEditPlanPage = new AddEditPlanPage(materialId, 1, buildingId);

                ViewPlans.Content = addEditPlanPage;
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Удалить план".
        /// </summary>
        private void BtnDeletePlan_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag != null && int.TryParse(button.Tag.ToString(), out int planId))
            {
                string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";

                string sqlQuery = "DELETE FROM Building_plan WHERE Id_building_plan = @PlanId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@PlanId", planId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Удаление элемента из списка
                            foreach (var item in MyListBox.Items)
                            {
                                if (item is Card card && card.Tag != null && card.Tag.ToString() == planId.ToString())
                                {
                                    MyListBox.Items.Remove(item);
                                    break;
                                }
                            }

                            MessageBox.Show("План успешно удален.");

                            // После удаления обновляем данные в ListBox
                            MyListBox.Items.Clear();
                            List<PlanData> updatedPlansDataList = GetPlansForBuilding(buildingId);
                            foreach (var planData in updatedPlansDataList)
                            {
                                var card = CreateBuildingCard(planData);
                                MyListBox.Items.Add(card);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить план.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при удалении плана: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Ошибка при получении идентификатора плана.");
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Назад".
        /// </summary>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            BuildingInfoPage buildingInfoPage = new BuildingInfoPage(buildingId);
            ViewPlans.Content = buildingInfoPage;
        }
    }

    /// <summary>
    /// Класс данных о плане по строительству.
    /// </summary>
    public class PlanData
    {
        public string Id_building_plan { get; set; }
        public string Task_name { get;set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string StatusName { get; set; }
    }
}