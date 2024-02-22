using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemConstruction
{
    // Класс для хранения данных о карточке здания.
    public class BuildingCardData
    {
        // Идентификатор здания.
        public int Id_building { get; set; }
        // Название здания.
        public string BuildingName { get; set; }
        // Дата начала строительства.
        public DateTime DateStart { get; set; }
        // Дата окончания строительства.
        public DateTime DateEnd { get; set; }
        // Доход от здания.
        public decimal Revenue { get; set; }
        // Название статуса здания.
        public string StatusName { get; set; }
    }
}