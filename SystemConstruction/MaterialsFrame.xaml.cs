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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SystemConstruction
{
    /// <summary>
    /// Класс страницы для отображения материалов здания.
    /// </summary>
    public partial class MaterialsFrame : Page
    {
        SolidColorBrush RedBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#DC143C"));
        SolidColorBrush WhiteBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFAFA"));
        private int buildingId;

        /// <summary>
        /// Конструктор класса MaterialsFrame.
        /// </summary>
        /// <param name="buildingId">Идентификатор здания.</param>
        public MaterialsFrame(int buildingId)
        {
            InitializeComponent();
            this.buildingId = buildingId;

            List<MaterialData> materialsDataList = GetMaterialForBuilding(buildingId);

            // Создание и заполнение карточек на основе данных
            foreach (var materialData in materialsDataList)
            {
                var card = CreateBuildingCard(materialData);
                MyListBox.Items.Add(card);
            }
        }

        /// <summary>
        /// Получение списка материалов для указанного здания из базы данных.
        /// </summary>
        /// <param name="buildingId">Идентификатор здания.</param>
        /// <returns>Список объектов MaterialData, представляющих материалы здания.</returns>
        public List<MaterialData> GetMaterialForBuilding(int buildingId)
        {
            List<MaterialData> materialDataList = new List<MaterialData>();

            string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";

            string sqlQuery = @"
        SELECT m.Id_materials, m.Material_name, m.Material_count, 
               (m.Material_count * m.Price_per_one) AS TotalPrice
        FROM Materials m
        WHERE m.Building_id = @BuildingId";

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
                        MaterialData materialData = new MaterialData
                        {
                            Id_materials = reader["Id_materials"].ToString(),
                            Material_name = reader["Material_name"].ToString(),
                            Material_count = (int)reader["Material_count"],
                            Price = (decimal)reader["TotalPrice"]
                        };

                        materialDataList.Add(materialData);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось получить данные" + ex.Message.ToString());
                }
            }

            return materialDataList;
        }

        /// <summary>
        /// Создание карточки материала на основе переданных данных.
        /// </summary>
        /// <param name="materialData">Данные о материале.</param>
        /// <returns>Экземпляр карточки материала.</returns>
        private Card CreateBuildingCard(MaterialData materialData)
        {
            // Логика создания карточки
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
                    Text = materialData.Material_name,
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
                            Text = $"{materialData.Material_count}",
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
                    Text = $"{materialData.Price}$",
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
                            Tag = materialData.Id_materials, // Store budget ID as Tag
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
                            Tag = materialData.Id_materials, // Store budget ID as Tag
                        }
                    }
                }
            }
                }
            };
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
                                    button.Click += BtnEditMaterial_Click;
                                }
                                else if (button.Content.ToString() == "УДАЛИТЬ")
                                {
                                    button.Click += BtnDeleteMaterial_Click;
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
        /// Обработчик события нажатия кнопки "Добавить материал".
        /// Открывает страницу добавления/редактирования материала.
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Параметры события.</param>

        private void BtnAddMaterial_Click(object sender, RoutedEventArgs e)
        {
            AddEditMaterialPage addEditMaterialPage = new AddEditMaterialPage(0, 0, buildingId);

            AddEditMaterialFrame.Content = addEditMaterialPage;

        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Редактировать материал".
        /// Открывает страницу редактирования материала с передачей идентификатора материала.
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Параметры события.</param>
        private void BtnEditMaterial_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag != null && int.TryParse(button.Tag.ToString(), out int materialId))
            {
                AddEditMaterialPage addEditMaterialPage = new AddEditMaterialPage(materialId, 1, buildingId);

                AddEditMaterialFrame.Content = addEditMaterialPage;
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Удалить материал".
        /// Удаляет материал из базы данных и обновляет отображение списка материалов.
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Параметры события.</param>
        private void BtnDeleteMaterial_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag != null && int.TryParse(button.Tag.ToString(), out int materialId))
            {
                string connectionString = "Server=DESKTOP-5G83A57;Database=ConstructionDB;Integrated Security=True;";

                string sqlQuery = "DELETE FROM Materials WHERE Id_materials = @MaterialId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaterialId", materialId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Удаление элемента из списка
                            foreach (var item in MyListBox.Items)
                            {
                                if (item is Card card && card.Tag != null && card.Tag.ToString() == materialId.ToString())
                                {
                                    MyListBox.Items.Remove(item);
                                    break;
                                }
                            }

                            MessageBox.Show("Материал успешно удален.");

                            // После удаления обновляем данные в ListBox
                            MyListBox.Items.Clear();
                            List<MaterialData> updatedMaterialsDataList = GetMaterialForBuilding(buildingId);
                            foreach (var materialData in updatedMaterialsDataList)
                            {
                                var card = CreateBuildingCard(materialData);
                                MyListBox.Items.Add(card);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить материал.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при удалении материала: " + ex.Message);
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
        /// Возвращает пользователя на страницу информации о здании.
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Параметры события.</param>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            BuildingInfoPage buildingInfoPage = new BuildingInfoPage(buildingId);
            ViewBuilding.Content = buildingInfoPage;
        }
    }

    /// <summary>
    /// Класс для хранения данных о материалах.
    /// </summary>
    public class MaterialData
    {
        /// <summary>
        /// Идентификатор материала.
        /// </summary>
        public string Id_materials { get; set; }

        /// <summary>
        /// Название материала.
        /// </summary>
        public string Material_name { get; set; }

        /// <summary>
        /// Количество материала.
        /// </summary>
        public int Material_count { get; set; }

        /// <summary>
        /// Цена материала.
        /// </summary>
        public decimal Price { get; set; }
    }
}