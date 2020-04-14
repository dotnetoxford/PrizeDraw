using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PrizeDraw.ZoomFunctions.Models;

namespace PrizeDraw.ZoomFunctions
{
    public static class ZoomWebhook
    {
        [FunctionName("ZoomWebhook")]
        [return: Table("Attendees")]
        public static async Task<Attendee> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            var bodyText = await new StreamReader(req.Body).ReadToEndAsync();

            log.LogInformation(bodyText);

            var data = JsonConvert.DeserializeObject<ParticipantJoinedAndLeftEventData>(bodyText);

            return new Attendee(data.payload.@object.id)
            {
                MeetingUuid = data.payload.@object.uuid,
                MeetingName = data.payload.@object.topic,
                UserName = data.payload.@object.participant.user_name,
                UserId = data.payload.@object.participant.user_id,
                UserUuid = data.payload.@object.participant.id,
                JoinTime = data.payload.@object.participant.join_time,
                LeaveTime = data.payload.@object.participant.leave_time,
            };
        }
    }
}