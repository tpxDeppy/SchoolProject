using System.Text.Json.Serialization;

namespace SchoolProject.BL.Models.Enums
{
    //in order to see the actual values instead of numbers
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserType
    {
        Teacher,
        Pupil
    }
}
