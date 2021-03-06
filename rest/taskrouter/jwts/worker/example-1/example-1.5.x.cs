// Download the twilio-csharp library from
// https://www.twilio.com/docs/libraries/csharp#installation
using System;
using System.Collections.Generic;
using Twilio.Http;
using Twilio.Jwt.Taskrouter;

class Example
{
    static void Main(string[] args)
    {
        // Find your Account Sid and Auth Token at twilio.com/console
        const string accountSid = "ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
        const string authToken = "your_auth_token";
        const string workspaceSid = "WSXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
        const string workerSid = "WKXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        var updateActivityFilter = new Dictionary<string, Policy.FilterRequirement>
        {
            { "ActivitySid", Policy.FilterRequirement.Required }
        };

        var urls = new PolicyUrlUtils(workspaceSid, workerSid);

        var allowActivityUpdates = new Policy(urls.Worker,
                                              HttpMethod.Post,
                                              postFilter: updateActivityFilter);

        var allowTasksUpdate = new Policy(urls.AllTasks, HttpMethod.Post);

        var allowReservationUpdate = new Policy(urls.AllReservations, HttpMethod.Post);

        var policies = new List<Policy>
        {
            allowActivityUpdates,
            allowTasksUpdate,
            allowReservationUpdate
        };

        // By default, tokens are good for one hour.
        // Override this default timeout by specifiying a new value (in seconds).
        // For example, to generate a token good for 8 hours:
        var capability = new TaskRouterCapability(
            accountSid,
            authToken,
            workspaceSid,
            null,
            policies: policies,
            expiration: DateTime.UtcNow.AddSeconds(28800)); // 60 * 60 * 8

        Console.WriteLine(capability.ToJwt());
    }
}

class PolicyUrlUtils
{
    const string taskRouterBaseUrl = "https://taskrouter.twilio.com";
    const string taskRouterVersion = "v1";

    readonly string _workspaceSid;
    readonly string _workerSid;

    public PolicyUrlUtils(string workspaceSid, string workerSid)
    {
        _workspaceSid = workspaceSid;
        _workerSid = workerSid;
    }

    public string AllTasks => $"{Workspace}/Tasks/**";

    public string Worker => $"{Workspace}/Workers/{_workerSid}";

    public string AllReservations => $"{Worker}/Reservations/**";

    string Workspace =>
        $"{taskRouterBaseUrl}/{taskRouterVersion}/Workspaces/{_workspaceSid}";
}
