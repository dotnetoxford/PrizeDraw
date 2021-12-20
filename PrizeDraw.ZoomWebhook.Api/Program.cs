using System.Text.Json;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Options;
using PrizeDraw.ZoomWebhook.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Settings>(builder.Configuration);

var keyVaultUri = builder.Configuration["KeyVaultUri"];

if (!string.IsNullOrWhiteSpace(keyVaultUri))
    builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultKeyVaultSecretManager());

var app = builder.Build();

app.MapPost("/", async (ParticipantJoinedAndLeftEventData data, [FromQuery]string authCode, ILogger<Program> logger, IOptions<Settings> settings) =>
{
    if (authCode != settings.Value.AuthCode)
        return Results.Unauthorized();

    logger.LogInformation("Data = {Data}", JsonSerializer.Serialize(data));

    var tableClient = new TableClient(settings.Value.StorageConnectionString, "Attendees");

    await tableClient.AddEntityAsync(new Attendee
    {
        PartitionKey = data.payload.@object.id, // Meeting ID
        RowKey = Guid.NewGuid().ToString(),

        Type = data.payload.@object.type,
        MeetingUuid = data.payload.@object.uuid,
        MeetingId = data.payload.@object.id,
        MeetingName = data.payload.@object.topic,
        UserName = data.payload.@object.participant.user_name,
        UserId = data.payload.@object.participant.user_id,
        UserUuid = data.payload.@object.participant.id,
        JoinTime = data.payload.@object.participant.join_time,
        LeaveTime = data.payload.@object.participant.leave_time,
    });

    return Results.Ok();
});

app.Run();