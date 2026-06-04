namespace FrontEnd.Client.DTOs
{
    public class LocationDTO
    {
        public Guid? ID { get; set; } = new Guid();

        public string? Location { get; set; }

        public string? Code { get; set; }
    }
}
