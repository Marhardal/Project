using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

namespace FrontEnd.Client.DTOs
{
    public class TrackingDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid userID { get; set; }
        public Guid ProjectID { get; set; }
        public Guid StatusID { get; set; }
        public DateTime assignedDate { get; set; }
        public DateTime? closingDate { get; set; }
        public DateTime createdOn { get; set; } = DateTime.Now;
        public DateTime? updatedOn { get; set; }
        //public ICollection<ProjectModel>? Projects { get; set; }
        //public ICollection<Status>? Statuses { get; set; }
        [ForeignKey("ProjectID")]
        public ProjectDTO? Project { get; set; }
        [ForeignKey("StatusID")]
        public StatusDTO? Status { get; set; }
        public ReviewDTO? Review { get; set; }

    }

    public enum ProjectType
    {
        [Display(Name = "Proposal")]
        Proposal,
        [Display(Name = "Environmental Impact Assessment")]
        ESIA,

        [Display(Name = "Project Audit")]
        Audit,

        [Display(Name = "Environmental & Social Management Plan")]
        ESMP,
    }

    public enum Proposal
    {
        NotStarted,
        Submitted,
        Approved,
        Rejected
    }
}
