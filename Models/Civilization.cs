using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace bot.aoe2.civpicker.Models
{
    [DataContract(Name = "civilizations")]
    public class Civlization
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [DataMember(Name = "expansion", EmitDefaultValue = false)]
        [JsonPropertyName("expansion")]
        public string Expansion { get; set; }

        [DataMember(Name = "army_type", EmitDefaultValue = false)]
        [JsonPropertyName("army_type")]
        public string ArmyType { get; set;}

        [DataMember(Name = "unique_unit", EmitDefaultValue = false)]
        [JsonPropertyName("unique_unit")]
        public List<string> UniqueUnit { get; set; }

        [DataMember(Name = "unique_tech", EmitDefaultValue = false)]
        [JsonPropertyName("unique_tech")]
        public List<string> UniqueTech { get; set; }

        [DataMember(Name = "team_bonus", EmitDefaultValue = false)]
        [JsonPropertyName("team_bonus")]
        public string TeamBonus { get; set; }

        [DataMember(Name = "civilization_bonus", EmitDefaultValue = false)]
        [JsonPropertyName("civilization_bonus")]
        public List<string> CivBonus { get; set; }
    }
}