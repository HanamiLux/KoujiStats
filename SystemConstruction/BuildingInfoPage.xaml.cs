using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;

namespace SystemConstruction
{
    /// <summary>
    /// Класс, представляющий страницу с информацией о здании.
    /// </summary>
    public partial class BuildingInfoPage : Page
    {
        private int buildingId;
        public int BuildingId { get; set; }


        /// <summary>
        /// Конструктор класса, инициализирует страницу информацией о здании.
        /// </summary>
        /// <param name="buildingId">Идентификатор здания.</param>
        public BuildingInfoPage(int buildingId)
        {
            InitializeComponent();

            this.buildingId = buildingId;
            GetBuildingData(buildingId);
        }

        /// <summary>
        /// Метод для получения данных о здании из базы данных.
        /// </summary>
        /// <param name="buildingId">Идентификатор здания.</param>
        /// <returns>Объект с данными о здании.</returns>
        private BuildingCardData GetBuildingData(int buildingId)
        {
            BuildingCardData buildingData = null;

            string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";
            //самое первое
            //string buildingQuery = "SELECT b.Id_building, b.Building_name, bp.Date_start, bp.Date_end, bu.Revenue, s.Status_name " +
            //       "FROM Building b " +
            //       "INNER JOIN Building_plan bp ON b.Id_building = bp.Building_id " +
            //       "INNER JOIN Budget bu ON b.Id_building = bu.Building_id " +
            //       "INNER JOIN Status s ON bp.Status_id = s.Id_status " +
            //       "WHERE b.Id_building = @BuildingId";


            //последнее рабочее
            //string buildingQuery = "SELECT b.Id_building, b.Building_name, bp.Date_start, bp.Date_end, SUM(bu.Revenue) AS TotalRevenue, s.Status_name " +
            //               "FROM Building b " +
            //               "INNER JOIN Building_plan bp ON b.Id_building = bp.Building_id " +
            //               "INNER JOIN Budget bu ON b.Id_building = bu.Building_id " +
            //               "INNER JOIN Status s ON bp.Status_id = s.Id_status " +
            //               "WHERE b.Id_building = @BuildingId " +
            //               "GROUP BY b.Id_building, b.Building_name, bp.Date_start, bp.Date_end, s.Status_name";

            // Запрос для получения данных о здании.
            string buildingQuery = @"
            SELECT 
            b.Id_building, 
            b.Building_name, 
            MIN(bp.Date_start) AS Date_start, 
            MAX(bp.Date_end) AS Date_end, 
            COALESCE(SUM(bu.Revenue), 0) AS TotalRevenue, 
            COALESCE(s.Status_name, 'Unknown') AS Status_name 
            FROM 
            Building b
            LEFT JOIN
            Building_plan bp ON b.Id_building = bp.Building_id 
            LEFT JOIN
            Budget bu ON b.Id_building = bu.Building_id 
            LEFT JOIN
            Status s ON bp.Status_id = s.Id_status 
            WHERE 
            b.Id_building = @BuildingId 
            GROUP BY 
            b.Id_building, 
            b.Building_name, 
            bp.Date_start, 
            bp.Date_end, 
            s.Status_name";

            // Запрос для получения материалов, используемых в здании.
            string materialsQuery = "SELECT m.Material_name, (m.Material_count * m.Price_per_one) AS TotalCost " +
                                    "FROM Materials m " +
                                    "WHERE m.Building_id = @BuildingId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(buildingQuery, connection);
                command.Parameters.AddWithValue("@BuildingId", buildingId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // Чтение данных о здании из результата запроса.
                if (reader.Read())
                {
                    buildingData = new BuildingCardData
                    {
                        Id_building = (int)reader["Id_building"],
                        BuildingName = reader["Building_name"].ToString(),
                        //DateStart = (DateTime)reader["Date_start"],
                        //DateEnd = (DateTime)reader["Date_end"],
                        //Revenue = (decimal)reader["TotalRevenue"],
                        DateStart = Convert.IsDBNull(reader["Date_start"]) ? DateTime.MinValue : (DateTime)reader["Date_start"],
                        DateEnd = Convert.IsDBNull(reader["Date_end"]) ? DateTime.MinValue : (DateTime)reader["Date_end"],
                        Revenue = Convert.IsDBNull(reader["TotalRevenue"]) ? 0 : (decimal)reader["TotalRevenue"],
                        StatusName = reader["Status_name"].ToString()
                    };
                }

                reader.Close();

                // Отображение полученных данных на форме.
                BuildingNameTB.Text = buildingData.BuildingName.ToString();
                DateTB.Text = $"{buildingData.DateStart.ToShortDateString()} - {buildingData.DateEnd.ToShortDateString()}";
                BudgetTB.Text = $"БЮДЖЕТ: {buildingData.Revenue}$";
                StatusTB.Text = buildingData.StatusName.ToString();

                SqlCommand materialsCommand = new SqlCommand(materialsQuery, connection);
                materialsCommand.Parameters.AddWithValue("@BuildingId", buildingId);
                SqlDataReader materialsReader = materialsCommand.ExecuteReader();

                DataTable materialsTable = new DataTable();
                materialsTable.Load(materialsReader);
                MaterialsGrid.ItemsSource = materialsTable.DefaultView;

                BuildingId = buildingData?.Id_building ?? 0;
                this.DataContext = this;
            }

            return buildingData;
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Бюджет".
        /// Открывает страницу просмотра бюджета выбранного здания.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Budget_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                int Id = (int)button.Tag;
                ViewBudgetFrame BudgetPage = new ViewBudgetFrame(Id);
                BudgetFrame.Content = BudgetPage;
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Материалы".
        /// Открывает страницу просмотра материалов, используемых в здании.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MaterialsBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                int Id = (int)button.Tag;
                MaterialsFrame materialsFrame = new MaterialsFrame(Id);
                MaterialFrame.Content = materialsFrame;
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Планы".
        /// Открывает страницу просмотра планов строительства выбранного здания.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PlansBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                int Id = (int)button.Tag;
                ViewPlansFrame plansFrame = new ViewPlansFrame(Id);
                PlansFrame.Content = plansFrame;

            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Ресурсы".
        /// Открывает страницу просмотра ресурсов, используемых в здании.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ResourcesBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                int Id = (int)button.Tag;
                ViewResourcesPage resourcesFrame = new ViewResourcesPage(Id);
                ResourcesFrame.Content = resourcesFrame;
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Назад".
        /// Открывает страницу выбора здания для возврата к предыдущему экрану.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            SelectBuildingPage selectBuildingPage = new SelectBuildingPage();
            MaterialFrame.Content = selectBuildingPage;
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Редактировать название здания".
        /// Открывает страницу редактирования названия выбранного здания.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void BuildingNameEditBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                int Id = (int)button.Tag;
                EditBuildingNamePage buildingNameFrame = new EditBuildingNamePage(Id);
                BuildingNameFrame.Content = buildingNameFrame;
            }
        }
    }
}