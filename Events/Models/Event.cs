namespace Events.Models
{
    using System;
    using System.Collections.Generic;
 
    [Serializable]
    public class Event
    {
        public int Id { get; set; }

        public string Act { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string DepartureLocation { get; set; }

        public string ArrivalLocation { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate
        {
            get
            {
                return this.StartDate.AddDays(this.Days);
            }
        }

        public int Days { get; set; }

        public string Duration
        {
            get
            {
                return this.Days + (this.Days == 1 ? " Day" : " Days");
            }
        }

        public ContainerType ContainerType { get; set; }

        public string OwnerId { get; set; }
    }
}