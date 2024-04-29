namespace RentalMotor.Api.Models
{
    public class S3ObjectModel
    {
        public string? Name { get; set; }
        public string? PresignedUrl { get; set; }
    }
}