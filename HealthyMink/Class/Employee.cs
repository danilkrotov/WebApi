using HealthyMink.Class.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthyMink.Class
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    public class Employee
    {
        /// <value>
        /// Номер пропуска (ID)
        /// </value>
        public int Id { get; set; }
        /// <value>
        /// Фамилия
        /// </value>
        [property: Required]
        public string Surname { get; set; }
        /// <value>
        /// Имя
        /// </value>
        [property: Required]
        public string Name { get; set; }
        /// <value>
        /// Отчество
        /// </value>
        public string? Patronymic { get; set; }
        /// <value>
        /// Должность
        /// </value>
        [property: Required]
        public JobTitle JobTitle { get; set; }
        /// <value>
        /// Смена
        /// </value>
        public List<Shift>? Shift { get; set; }
        /// <value>
        /// Количество нарушений
        /// </value>
        [NotMapped]
        public int Penalty { get; set; }
        /// <summary>
        /// Новый сотрудник
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="surname">Фамилия</param>
        /// <param name="name">Имя</param>
        /// <param name="patronymic">Отчество</param>
        /// <param name="jobTitle">Должность</param>
        /// <param name="shift">Смена</param>
        public Employee(int id, string surname, string name, string? patronymic, JobTitle jobTitle, List<Shift>? shift)
        {
            Id = id;
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            JobTitle = jobTitle;
            Shift = shift;
        }
        private Employee() { }
    }
}
