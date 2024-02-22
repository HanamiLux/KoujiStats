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
    /// Класс представления для просмотра ресурсов здания.
    /// </summary>
    public partial class ViewResourcesPage : Page
    {
        SolidColorBrush RedBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#DC143C"));
        SolidColorBrush WhiteBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFAFA"));
        private int buildingId; // Идентификатор здания

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="buildingId">Идентификатор здания.</param>
        public ViewResourcesPage(int buildingId)
        {
            InitializeComponent();

            this.buildingId = buildingId;

            // Получение списка ресурсов для здания
            List<ResourceData> resourcesDataList = GetResourceForBuilding(buildingId);

            // Создание и заполнение карточек на основе данных
            foreach (var resourceData in resourcesDataList)
            {
                var card = CreateBuildingCard(resourceData);
                MyListBox.Items.Add(card);
            }
        }

        /// <summary>
        /// Метод для получения списка ресурсов для здания.
        /// </summary>
        /// <param name="buildingId">Идентификатор здания.</param>
        /// <returns>Список ресурсов для здания.</returns>
        public List<ResourceData> GetResourceForBuilding(int buildingId)
        {
            List<ResourceData> resourceDataList = new List<ResourceData>();

            string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";

            string sqlQuery = @"
                SELECT r.Id_resource, r.Resource_name, r.Resource_count, 
                (r.Resource_count * r.Price_per_one_month) AS TotalPrice
                FROM Resources r
                WHERE r.Building_id = @BuildingId";

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
                        ResourceData resourceData = new ResourceData
                        {
                            Id_resource = reader["Id_resource"].ToString(),
                            Resource_name = reader["Resource_name"].ToString(),
                            Resource_count = (int)reader["Resource_count"],
                            Price = (decimal)reader["TotalPrice"]
                        };

                        resourceDataList.Add(resourceData);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось получить данные" + ex.Message.ToString());
                }
            }

            return resourceDataList;
        }

        /// <summary>
        /// Метод для создания карточки ресурса здания.
        /// </summary>
        /// <param name="resourceData">Данные о ресурсе здания.</param>
        /// <returns>Карточка ресурса здания.</returns>
        private Card CreateBuildingCard(ResourceData resourceData)
        {
            var card = new Card
            {
                Width = 350,
                Height = 220,
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
                    Text = resourceData.Resource_name,
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
                            Text = "КОЛ-ВО:",
                            Foreground = System.Windows.Media.Brushes.Black,
                            FontWeight = FontWeights.Bold,
                            FontSize = 20,
                            Margin = new Thickness(15, 5, 0, 0),
                            HorizontalAlignment = HorizontalAlignment.Left
                        },
                        new TextBlock
                        {
                            Text = $"{resourceData.Resource_count}",
                            FontWeight = FontWeights.Medium,
                            FontSize = 18,
                            Background = RedBrush,
                            Foreground = WhiteBrush,
                            Padding = new Thickness(12, 7, 12, 7),
                            Margin = new Thickness(30, 0, 10, 0)
                        }
                    }
                },
                new TextBlock
                {
                    Foreground = System.Windows.Media.Brushes.Black,
                    Text = $"{resourceData.Price}$",
                    FontWeight = FontWeights.Normal,
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
                            Tag = resourceData.Id_resource, // Хранение идентификатора ресурса
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
                            Tag = resourceData.Id_resource, // Хранение идентификатора ресурса
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
                                    button.Click += BtnEditResource_Click;
                                }
                                else if (button.Content.ToString() == "УДАЛИТЬ")
                                {
                                    button.Click += BtnDeleteResource_Click;
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
        /// Обработчик события нажатия кнопки "Добавить ресурс".
        /// </summary>
        private void BtnAddResource_Click(object sender, RoutedEventArgs e)
        {
            AddEditResourcePage addEditResourcePage = new AddEditResourcePage(0, 0, buildingId);

            AddEditResourceFrame.Content = addEditResourcePage;

        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Изменить ресурс".
        /// </summary>
        private void BtnEditResource_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag != null && int.TryParse(button.Tag.ToString(), out int resourceId))
            {
                AddEditResourcePage addEditResourcePage = new AddEditResourcePage(resourceId, 1, buildingId);

                AddEditResourceFrame.Content = addEditResourcePage;
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Удалить ресурс".
        /// </summary>
        private void BtnDeleteResource_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag != null && int.TryParse(button.Tag.ToString(), out int resourceId))
            {
                string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";

                string sqlQuery = "DELETE FROM Resources WHERE Id_resource = @ResourceId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@ResourceId", resourceId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Удаление элемента из списка
                            foreach (var item in MyListBox.Items)
                            {
                                if (item is Card card && card.Tag != null && card.Tag.ToString() == resourceId.ToString())
                                {
                                    MyListBox.Items.Remove(item);
                                    break;
                                }
                            }

                            MessageBox.Show("Ресурс успешно удален.");

                            // После удаления обновляем данные в ListBox
                            MyListBox.Items.Clear();
                            List<ResourceData> updatedResourcesDataList = GetResourceForBuilding(buildingId);
                            foreach (var resourceData in updatedResourcesDataList)
                            {
                                var card = CreateBuildingCard(resourceData);
                                MyListBox.Items.Add(card);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить ресурс.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при удалении ресурса: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Ошибка при получении идентификатора ресурса.");
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Назад".
        /// </summary>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            BuildingInfoPage buildingInfoPage = new BuildingInfoPage(buildingId);
            ViewBuilding.Content = buildingInfoPage;
        }
    }

    /// <summary>
    /// Класс данных о ресурсе здания.
    /// </summary>
    public class ResourceData
    {
        public string Id_resource { get; set; }
        public string Resource_name { get; set; }
        public int Resource_count { get; set; }
        public decimal Price { get; set; }
    }
}