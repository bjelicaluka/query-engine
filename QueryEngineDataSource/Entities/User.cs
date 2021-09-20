
namespace QueryEngineDataSource.Entities
{
    public class User : Accessible
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }
}