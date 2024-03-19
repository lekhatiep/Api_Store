namespace Domain.Base
{
    public class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
    }
}
