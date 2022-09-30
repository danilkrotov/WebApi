namespace HealthyMink.Class
{
    /// <summary>
    /// Смена
    /// </summary>
    public class Shift
    {
        /// <value>
        /// ID смены
        /// </value>
        public int Id { get; set; }
        /// <value>
        /// Время начала смены
        /// </value>
        public DateTime StartTime { get; set; }
        /// <value>
        /// Время конца смены
        /// </value>
        public DateTime EndTime { get; set; }
        /// <value>
        /// Количество отработанных часов
        /// </value>
        public int Hour { get; set; }
        /// <value>
        /// Ссылка на работника
        /// </value>
        public Employee Employee { get; set; }
        /// <value>
        /// Конструктор с поддержкой ввода ID
        /// </value>
        /// <param name="id">ID</param>
        /// <param name="startTime">Время начала смены</param>
        /// <param name="endTime">Время конца смены</param>
        /// <param name="hour">Количество отработанных часов</param>
        /// <param name="employee">Ссылка на работника</param>
        public Shift(int id, DateTime startTime, DateTime endTime, int hour, Employee employee)
        {
            Id = id;
            StartTime = startTime;
            EndTime = endTime;
            Hour = hour;
            Employee = employee;
        }
        /// <value>
        /// Конструктор с auto increment
        /// </value>
        /// <param name="startTime">Время начала смены</param>
        /// <param name="endTime">Время конца смены</param>
        /// <param name="hour">Количество отработанных часов</param>
        /// <param name="employee">Ссылка на работника</param>
        public Shift(DateTime startTime, DateTime endTime, int hour, Employee employee)
        {
            StartTime = startTime;
            EndTime = endTime;
            Hour = hour;
            Employee = employee;
        }
        public Shift() { }
    }
}
