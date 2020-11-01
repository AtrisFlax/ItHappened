namespace ItHappened.Infrastructure.Mappers
{
    public abstract class FactDto
    {
        public string FactName { get; set; }
        public string Description { get; set; }
        public double Priority { get; set; }
    }
}