using System.ComponentModel.DataAnnotations;

namespace Application.Dto
{
    public class PortDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Lat { get; set; }
        [Required]
        public string Lon { get; set; }
    }
}
