﻿namespace Octokit.Webhooks.Models.RepositoryImportEvent
{
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Status
    {
        [EnumMember(Value = "success")]
        Success,
        [EnumMember(Value = "cancelled")]
        Cancelled,
        [EnumMember(Value = "failure")]
        Failure,
    }
}
