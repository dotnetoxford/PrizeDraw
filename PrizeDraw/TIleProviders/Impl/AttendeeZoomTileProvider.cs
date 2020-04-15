using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;
using PrizeDraw.Models;
using PrizeDraw.ViewModels;

namespace PrizeDraw.TIleProviders.Impl
{
    public class AttendeeZoomTileProvider : ITileProvider
    {
        private readonly string _eventId;

        public AttendeeZoomTileProvider(string eventId) =>
            _eventId = eventId;

        public Task<List<TileViewModel>> GetTilesAsync()
        {
            var dic = new Dictionary<string, string>();

            foreach(var attendee in GetAttendees(_eventId))
                if (attendee.JoinTime != null)
                    dic[attendee.UserId] = attendee.UserName;
                else
                    dic.Remove(attendee.UserId);

            var rand = new Random();

            return Task.FromResult(dic.Select(x => new TileViewModel(
                name: x.Value,
                attendeeId: x.Key,
                remoteImageUri: null, //x.member.photo?.highres_link ?? x.member.photo?.photo_link,
                color: new SolidColorBrush(Color.FromRgb((byte)rand.Next(100, 256),
                                                         (byte)rand.Next(100, 256),
                                                         (byte)rand.Next(100, 256)))
            )).ToList());
        }

        private IEnumerable<Attendee> GetAttendees(string meetingId) =>
            CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("PrizeDraw_AzureStorageConnectionString"))
                .CreateCloudTableClient()
                .GetTableReference("Attendees")
                .ExecuteQuery(new TableQuery<Attendee>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, meetingId)))
                .Where(x => x.Type != 0) // Type 0 is when user joins/leaves a breakout room
                .OrderBy(x => x.JoinTime ?? x.LeaveTime)
                .ToList();

        public Task SaveTilesAsync(List<TileViewModel> tiles) =>
            throw new NotSupportedException();
    }
}