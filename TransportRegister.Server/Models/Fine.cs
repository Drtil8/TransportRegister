using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportRegister.Server.Models
{
    public class Fine
    {
        [Key]
        public int FineId { get; set; }
        public double Amount { get; set; }
        public bool IsActive { get; set; }
        public DateOnly PaidOn { get; set; }
        public DateOnly DueDate { get; set; }

        public int OffenceId { get; set; }
        [ForeignKey(nameof(OffenceId))]
        public Offence Offence { get; set; }
    }
}
