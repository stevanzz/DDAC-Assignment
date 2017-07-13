﻿using Events.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.ApplicationService.Validators
{
    public static class EventsValidator
    {
        public static bool ValidateEventData(Event submittedEvent)
        {
            DateTime startDate;

            if (!string.IsNullOrWhiteSpace(submittedEvent.Title) && submittedEvent.Title.Length <= 100
                && !string.IsNullOrWhiteSpace(submittedEvent.Description) && submittedEvent.Description.Length <= 100
                && !string.IsNullOrWhiteSpace(submittedEvent.DepartureLocation) && submittedEvent.DepartureLocation.Length <= 100
                && !string.IsNullOrWhiteSpace(submittedEvent.ArrivalLocation) && submittedEvent.ArrivalLocation.Length <= 100
                && DateTime.TryParse(submittedEvent.StartDate.ToString(), out startDate)
                && (submittedEvent.Days > 0 && submittedEvent.Days <= 100))
            {
                return true;
            }
            System.Diagnostics.Debug.WriteLine(submittedEvent.Title);
            System.Diagnostics.Debug.WriteLine(submittedEvent.Description);
            System.Diagnostics.Debug.WriteLine(submittedEvent.DepartureLocation);
            System.Diagnostics.Debug.WriteLine(submittedEvent.ArrivalLocation);
            System.Diagnostics.Debug.WriteLine(submittedEvent.StartDate.ToString());
            System.Diagnostics.Debug.WriteLine(submittedEvent.Days);
            return false;
        }
    }
}