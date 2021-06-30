using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace bot.aoe2.civpicker.Models
{
    [DataContract]
    public class PlayerRating 
    {
        [DataMember(Name = "rating", EmitDefaultValue = false)]
        [JsonPropertyName("rating")]
        public int Elo { get; set; }

        [DataMember(Name = "num_wins", EmitDefaultValue = false)]
        [JsonPropertyName("num_wins")]
        public string Wins { get; set; }

        [DataMember(Name = "num_losses", EmitDefaultValue = false)]
        [JsonPropertyName("num_losses")]
        public string Loses { get; set; }

        [DataMember(Name = "streak", EmitDefaultValue = false)]
        [JsonPropertyName("streak")]
        public int Streak { get; set; }

        [DataMember(Name = "drops", EmitDefaultValue = false)]
        [JsonPropertyName("drops")]
        public int Drops { get; set; }

        [DataMember(Name = "timestamp", EmitDefaultValue = false)]
        [JsonPropertyName("timestamp")]
        public string TimeStamp { get; set; }

        [JsonIgnore]
        public string PlayerId { get; set; }
    }
}