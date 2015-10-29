using ServiceStack.DataAnnotations;

namespace Domain
{
    public class Category
    {
        [AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }

    

    }
}