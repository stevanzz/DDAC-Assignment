﻿using Events.Web.Helpers;
using Events.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events.Web.Mappers
{
    public static class EventsMapper
    {
        public static Models.Event MapToModel(this Models.EventViewModel model)
        {
            Models.Event eventMapped = new Models.Event();
            eventMapped.Title = model.Title;
            eventMapped.Act = model.Act;
            eventMapped.Description = model.Description;
            eventMapped.DepartureLocation = model.DepartureLocation;
            eventMapped.ArrivalLocation = model.ArrivalLocation;
            eventMapped.Id = model.Id;
            eventMapped.ContainerType = (ContainerType)model.ContainerType;
            eventMapped.Days = model.Days.Value;
            eventMapped.StartDate = model.StartDate.Value;
            eventMapped.OwnerId = model.OwnerId;
            return eventMapped;
        }

        public static Models.EventViewModel MapToViewModel(this Models.Event model)
        {
            Models.EventViewModel eventMapped = new Models.EventViewModel();
            eventMapped.Title = model.Title;
            eventMapped.Act = model.Act;
            eventMapped.Description = model.Description;
            eventMapped.DepartureLocation = model.DepartureLocation;
            eventMapped.ArrivalLocation = model.ArrivalLocation;
            eventMapped.Id = model.Id;
            eventMapped.Days = model.Days;
            eventMapped.StartDate = model.StartDate;
            eventMapped.ContainerPluralName = model.ContainerType.GetPluralizedName();
            return eventMapped;
        }

        public static IEnumerable<Models.EventViewModel> MapToViewModelCollection(this IEnumerable<Models.Event> model)
        {
            var models = new List<Models.EventViewModel>();
            if (model != null)
            {
                foreach (var item in model)
                {
                    models.Add(item.MapToViewModel());
                }
            }
            return models;
        }
    }
}