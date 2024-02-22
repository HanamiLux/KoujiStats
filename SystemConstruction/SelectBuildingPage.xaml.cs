using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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
    /// Класс страницы выбора здания.
    /// </summary>
    public partial class SelectBuildingPage : Page
    {
        SolidColorBrush RedBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#DC143C"));
        SolidColorBrush WhiteBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFAFA"));

        /// <summary>
        /// Конструктор класса SelectBuildingPage.
        /// </summary>
        public SelectBuildingPage()
        {
            InitializeComponent();

            List<BuildingCardData> buildingDataList = GetBuildingData();

            // Создание и заполнение карточек на основе данных
            foreach (var buildingData in buildingDataList)
            {
                var card = CreateBuildingCard(buildingData);
                MyListBox.Items.Add(card);
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Создать здание".
        /// </summary>
        private void BtnCreateBuilding_Click(object sender, RoutedEventArgs e)
        {
            
            CreateBuildingFrame.Content = new CreateBuildingPage();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "ОТКРЫТЬ" на карточке здания.
        /// </summary>
        private void BuildingCardButton_Click(object sender, RoutedEventArgs e)
        {

            Button button = sender as Button;
            if (button != null)
            {
                // Получение значения Id_building из Tag кнопки
                int buildingId = (int)button.Tag;

                BuildingInfoPage buildingInfoPage = new BuildingInfoPage(buildingId);

                BuildingInfoFrame.Content = buildingInfoPage;
            }
        }

        /// <summary>
        /// Получает данные о зданиях из базы данных.
        /// </summary>
        /// <returns>Список объектов BuildingCardData.</returns>
        private List<BuildingCardData> GetBuildingData()
        {
            List<BuildingCardData> buildingDataList = new List<BuildingCardData>();

            string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
            
            string sqlQuery = @"SELECT 
                    b.Id_building, 
                    b.Building_name, 
                    MIN(bp.Date_start) AS Date_start, 
                    MAX(bp.Date_end) AS Date_end,
                    ISNULL(bu.TotalRevenue, 0) AS TotalRevenue, 
                    (
                        SELECT TOP 1 ISNULL(s.Status_name, 'Unknown') 
                        FROM Building_plan bp_inner 
                        LEFT JOIN Status s ON bp_inner.Status_id = s.Id_status 
                        WHERE bp_inner.Building_id = b.Id_building 
                        ORDER BY bp_inner.Date_start DESC
                    ) AS Status_name 
                FROM 
                    Building b 
                LEFT JOIN 
                    Building_plan bp ON b.Id_building = bp.Building_id 
                LEFT JOIN 
                    (
                        SELECT 
                            Building_id, 
                            SUM(ISNULL(Revenue, 0)) AS TotalRevenue
                        FROM 
                            Budget
                        GROUP BY 
                            Building_id
                    ) AS bu ON b.Id_building = bu.Building_id 
                GROUP BY 
                    b.Id_building, 
                    b.Building_name, 
                    bu.TotalRevenue;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        BuildingCardData buildingData = new BuildingCardData
                        {
                            Id_building = (int)reader["Id_building"],
                            BuildingName = reader["Building_name"].ToString(),
                            DateStart = reader["Date_start"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["Date_start"],
                            DateEnd = reader["Date_end"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["Date_end"],
                            Revenue = (decimal)reader["TotalRevenue"],
                            StatusName = reader["Status_name"].ToString()
                        };

                        buildingDataList.Add(buildingData);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении записи: " + ex.Message);
                }
                    
            }

            return buildingDataList;
        }

        /// <summary>
        /// Создает карточку здания на основе данных.
        /// </summary>
        /// <param name="buildingData">Данные о здании.</param>
        /// <returns>Экземпляр Card.</returns>
        private Card CreateBuildingCard(BuildingCardData buildingData)
        {
            var card = new Card
            {
                Width = 350,
                Height = 230,
                Margin = new Thickness(0, 0, 0, 50),
                Background = WhiteBrush,
                UniformCornerRadius = 30,
                Content = new StackPanel
                {
                    Margin = new Thickness(10),
                    Children =
                    {
                    new TextBlock { Text = $"{buildingData.BuildingName}",
                    Foreground = System.Windows.Media.Brushes.Black,
                        Margin = new Thickness(16, 16, 16, 4),
                        Style = (Style)FindResource("MaterialDesignHeadline5TextBlock"),
                        FontWeight = FontWeights.Bold},
                    new TextBlock { Text = $"{buildingData.DateStart.ToShortDateString()} - {buildingData.DateEnd.ToShortDateString()}",
                        Margin = new Thickness(16, 0, 16, 0),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Padding = new Thickness(4),
                        Style = (Style)FindResource("MaterialDesignBody2TextBlock"),
                        Background = RedBrush,
                        Foreground = WhiteBrush,
                        FontWeight = FontWeights.DemiBold,},
                    new TextBlock { Text = $"{buildingData.Revenue}$",
                        Margin = new Thickness(16, 0, 16, 0),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Padding = new Thickness(4),
                        Style = (Style)FindResource("MaterialDesignHeadline5TextBlock"),
                        Foreground = System.Windows.Media.Brushes.Black,
                        FontWeight = FontWeights.SemiBold,},
                    new TextBlock { Text = $"Статус:", 
                        Width = 120,
                        Margin = new Thickness(16, 3, 0, 0),
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Padding = new Thickness(4),
                        Style = (Style)FindResource("MaterialDesignBody2TextBlock"),
                        Background = RedBrush,
                        Foreground = WhiteBrush,
                        FontWeight = FontWeights.DemiBold, },
                    new TextBlock { Text = $"{buildingData.StatusName}",
                        Width = 120,
                        Margin = new Thickness(16, 0, 0, 0),
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Padding = new Thickness(4),
                        Style = (Style)FindResource("MaterialDesignBody2TextBlock"),
                        Background = RedBrush,
                        Foreground = WhiteBrush,
                        FontWeight = FontWeights.DemiBold,},
                    new Button
                        {
                        Height = 30,
                            Width = 100,
                            Padding = new Thickness(2, 2, 2, 2),
                            FontSize = 18,
                            Foreground = RedBrush,
                            FontWeight = FontWeights.Medium,
                            Content = "ОТКРЫТЬ",
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Style = (Style)Application.Current.FindResource("MaterialDesignFlatButton"),
                            Tag = buildingData.Id_building // Для передачи данных о стройке на следующую страницу
                        }
                    }
                }
            };

            // Привязка события Click к обработчику
            (card.Content as StackPanel).Children.OfType<Button>().Single().Click += BuildingCardButton_Click;

            return card;
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Статусы".
        /// </summary>
        private void BtnStatuses_Click(object sender, RoutedEventArgs e)
        {
            ViewStatusesPage viewStatusesPage = new ViewStatusesPage();
            StatusesFrame.Content = viewStatusesPage;
        }
    }
}