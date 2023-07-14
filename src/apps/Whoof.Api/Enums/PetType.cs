using System.Runtime.Serialization;

namespace Whoof.Api.Enums;

public enum PetType
{
    [EnumMember(Value = "UNSPECIFIED")]
    Unspecified = 0,
    [EnumMember(Value = "DOG")]
    Dog = 1,
    [EnumMember(Value = "CAT")]
    Cat = 2,
    [EnumMember(Value = "CAPYBARA")]
    Capybara = 3,
    [EnumMember(Value = "OTHER")]
    Other = 4,
}