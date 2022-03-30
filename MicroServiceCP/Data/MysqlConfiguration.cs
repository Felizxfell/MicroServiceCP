namespace MicroServiceCP.Data
{
    public class MysqlConfiguration
    {
        public MysqlConfiguration(string connectionString) => ConnectionString = connectionString;

        public string ConnectionString { get; set; }
    }
}
