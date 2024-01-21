using ProtoBuf;

namespace Contracts;

[ProtoContract]
public class AddTrackInfo() : IMessage
{
    [ProtoMember(1)] public string IpAddress { get; set; }

    [ProtoMember(2)] public string? UserAgent { get; set; }

    [ProtoMember(3)] public string? Referrer { get; set; }
    
    [ProtoMember(4)] public string? Timestamp { get; set; }

    [ProtoIgnore] public string RoutingKey => "add_track_info";
}