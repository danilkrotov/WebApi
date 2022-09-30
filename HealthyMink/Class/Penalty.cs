namespace HealthyMink.Class
{
    /// <summary>
    /// Нарушение
    /// </summary>
    public class Penalty
    {
        /// <value>
        /// ID смены
        /// </value>
        public int Id { get; set; }
        /// <value>
        /// Ссылка на сотрудника
        /// </value>
        public Employee Employee { get; set; }
        /// <value>
        /// Время нарушения
        /// </value>
        public DateTime Time { get; set; }
        /// <value>
        /// Конструктор с auto increment
        /// </value>
        public Penalty(Employee employee, DateTime time)
        {
            Employee = employee;
            Time = time;
        }
        private Penalty() { }
    }
}
