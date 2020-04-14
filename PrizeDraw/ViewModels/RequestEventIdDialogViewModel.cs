using System.Collections.Generic;
using GalaSoft.MvvmLight;
using PrizeDraw.Enums;

namespace PrizeDraw.ViewModels
{
    public class RequestEventIdDialogViewModel : ViewModelBase
    {
        public string EventId { get; set; }
        public EventType EventType { get; set; }

        public Dictionary<string, EventType> EventTypes => new Dictionary<string, EventType>
        {
            {"Meetup", EventType.Meetup},
            {"Zoom", EventType.Zoom},
        };
    }
}
