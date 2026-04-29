using Project.Models;
using System.ComponentModel.DataAnnotations;

        public class Status
        {
            public Guid ID { get; set; } = Guid.NewGuid();

            [Required]
            public string? Name { get; set; }

            [Required]
            public string? Color { get; set; }

            public int SortOrder { get; set; }

            public DateTime createdOn { get; set; }
            public DateTime updatedOn { get; set; }

            //public TrackingModel? Tracking { get; set; }
            public ICollection<TrackingModel>? Trackings { get; set; }

        }
