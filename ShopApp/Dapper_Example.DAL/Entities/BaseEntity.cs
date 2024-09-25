namespace Dapper_Example.DAL.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime? CreationTime { get; set; } = DateTime.Now;
        public bool IsRowActive { get; set; } = true;
    }
}
