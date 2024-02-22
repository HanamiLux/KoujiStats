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
    /// Класс страницы просмотра бюджета здания.
    /// </summary>
    public partial class ViewBudgetFrame : Page
    {
        SolidColorBrush RedBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#DC143C"));
        SolidColorBrush WhiteBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFAFA"));
        private int buildingId;
        public int BudgetId { get; set; }

        /// <summary>
        /// Конструктор класса ViewBudgetFrame.
        /// </summary>
        /// <param name="buildingId">Идентификатор здания.</param>
        public ViewBudgetFrame(int buildingId)
        {
            InitializeComponent();
            this.buildingId = buildingId;
            

            List<BudgetData> buildingDataList = GetBudgetForBuilding(buildingId);

            // Создание и заполнение карточек на основе данных
            foreach (var buildingData in buildingDataList)
            {
                var card = CreateBuildingCard(buildingData);
                MyListBox.Items.Add(card);
            }
        }

        /// <summary>
        /// Получает данные о бюджете для здания из базы данных.
        /// </summary>
        /// <param name="buildingId">Идентификатор здания.</param>
        /// <returns>Список объектов BudgetData.</returns>
        public List<BudgetData> GetBudgetForBuilding(int buildingId)
        {
            List<BudgetData> buildingDataList = new List<BudgetData>();

            string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";

            string sqlQuery = "SELECT bu.Id_budget, bu.Budget_name, bu.Revenue " +
                      "FROM Building b " +
                      "INNER JOIN Budget bu ON b.Id_building = bu.Building_id " +
                      "WHERE b.Id_building = @BuildingId";

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
                        BudgetData budgetData = new BudgetData
                        {
                            Id_Budget = reader["Id_budget"].ToString(),
                            BudgetName = reader["Budget_name"].ToString(),
                            Revenue = (decimal)reader["Revenue"]
                        };

                        buildingDataList.Add(budgetData);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось получить данные" + ex.Message.ToString());
                }
            }

            return buildingDataList;
        }

        /// <summary>
        /// Создает карточку бюджета на основе данных.
        /// </summary>
        /// <param name="buildingData">Данные о бюджете.</param>
        /// <returns>Экземпляр Card.</returns>
        private Card CreateBuildingCard(BudgetData buildingData)
        {
            var card = new Card
            {
                Width = 350,
                Height = 180,
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
                    Text = buildingData.BudgetName,
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
                            Text = "ПРИБЫЛЬ:",
                            Foreground = System.Windows.Media.Brushes.Black,
                            FontWeight = FontWeights.Bold,
                            FontSize = 20,
                            Margin = new Thickness(15, 5, 0, 0),
                            HorizontalAlignment = HorizontalAlignment.Left
                        },
                        new TextBlock
                        {
                            Text = $"{buildingData.Revenue}$",
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
                            Tag = buildingData.Id_Budget, // Store budget ID as Tag
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
                            Tag = buildingData.Id_Budget, // Store budget ID as Tag
                        }
                    }
                }
            }
                }
            };

            // Привязка обработчиков событий к кнопкам "ИЗМЕНИТЬ" и "УДАЛИТЬ"
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
                                    button.Click += BtnEditBudget_Click;
                                }
                                else if (button.Content.ToString() == "УДАЛИТЬ")
                                {
                                    button.Click += BtnDeleteBudget_Click;
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
        /// Обработчик события нажатия кнопки "Добавить бюджет".
        /// </summary>
        private void BtnAddBudget_Click(object sender, RoutedEventArgs e)
        {
            AddEditBudgetPage addEditBudgetPage = new AddEditBudgetPage(0, 0, buildingId);

            AddEditBudgetFrame.Content = addEditBudgetPage;
            
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "ИЗМЕНИТЬ" на карточке бюджета.
        /// </summary>
        private void BtnEditBudget_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag != null && int.TryParse(button.Tag.ToString(), out int budgetId))
            {
                AddEditBudgetPage addEditBudgetPage = new AddEditBudgetPage(budgetId, 1, buildingId);

                AddEditBudgetFrame.Content = addEditBudgetPage;
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "УДАЛИТЬ" на карточке бюджета.
        /// </summary>
        private void BtnDeleteBudget_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag != null && int.TryParse(button.Tag.ToString(), out int budgetId))
            {
                string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";

                string sqlQuery = "DELETE FROM Budget WHERE Id_budget = @BudgetId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@BudgetId", budgetId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Удаление элемента из списка
                            foreach (var item in MyListBox.Items)
                            {
                                if (item is Card card && card.Tag != null && card.Tag.ToString() == budgetId.ToString())
                                {
                                    MyListBox.Items.Remove(item);
                                    break;
                                }
                            }

                            MessageBox.Show("Бюджет успешно удален.");

                            // После удаления обновляем данные в ListBox
                            MyListBox.Items.Clear();
                            List<BudgetData> updatedBuildingDataList = GetBudgetForBuilding(buildingId);
                            foreach (var buildingData in updatedBuildingDataList)
                            {
                                var card = CreateBuildingCard(buildingData);
                                MyListBox.Items.Add(card);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить бюджет.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при удалении бюджета: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Ошибка при получении идентификатора бюджета.");
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Назад".
        /// </summary>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            BuildingInfoPage buildingInfoPage = new BuildingInfoPage(buildingId);
            BuildingFrame.Content = buildingInfoPage;
        }
    }

    /// <summary>
    /// Класс данных о бюджете.
    /// </summary>
    public class BudgetData
    {
        public string Id_Budget { get; set; }
        public string BudgetName { get; set; }
        public decimal Revenue { get; set; }
    }
}