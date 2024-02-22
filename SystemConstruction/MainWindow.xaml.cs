using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Data.SqlClient;
using System.Data;

namespace SystemConstruction
{
    /// <summary>
    /// Главное окно приложения.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Конструктор класса MainWindow.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            // Устанавливаем содержимое фрейма на страницу выбора здания при загрузке главного окна.
            SelectBuildingFrame.Content = new SelectBuildingPage();
        }
    }
}