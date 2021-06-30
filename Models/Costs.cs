using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace bot.aoe2.civpicker.Models
{
    [DataContract(Name = "cost")]
    public class Costs
    {
        [DataMember(Name = "Wood", EmitDefaultValue = false)]
        [JsonPropertyName("Wood")]
        public int Wood { get; set; }

        [DataMember(Name = "Food", EmitDefaultValue = false)]
        [JsonPropertyName("Food")]
        public int Food { get; set; }

        [DataMember(Name = "Stone", EmitDefaultValue = false)]
        [JsonPropertyName("Stone")]
        public int Stone { get; set; }

        [DataMember(Name = "Gold", EmitDefaultValue = false)]
        [JsonPropertyName("Gold")]
        public int Gold { get; set; }

        public override string ToString()
        {
            return ((Wood == 0) ? "" : $"Wood: {Wood} ") + ((Food == 0) ? "" : $"Food: {Food} ") + ((Stone == 0) ? "" : $"Stone: {Stone} ") + ((Gold == 0) ? "" : $"Gold: {Gold} ");
        }
    }
}