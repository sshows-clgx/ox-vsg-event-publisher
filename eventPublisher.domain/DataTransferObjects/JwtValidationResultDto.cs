using System.Collections.Generic;

namespace eventPublisher.domain.dataTransferObjects
{
    public class JwtValidationResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public FullJwtDataDto JwtData { get; set; }
    }

    public class FullJwtDataDto
    {
        public Dictionary<string, string> Payload { get; set; }
        public ApplicationDto AppInfo { get; set; }
    }

    public class ApplicationDto
    {
        public long ImmutableAppID { get; set; }
        public string AppID { get; set; }
        public string AppName { get; set; }
        public string MainUrl { get; set; }
        public bool AutoLinkDuringRegister { get; set; }
        public string OAuthHandlerUrl { get; set; }
        public bool IsThirdParty { get; set; }
    }
}