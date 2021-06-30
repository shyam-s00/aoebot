using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace bot.aoe2.civpicker.Models
{
    [DataContract(Name = "units")]
    public class Units
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [DataMember(Name = "description", EmitDefaultValue = false)]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [DataMember(Name = "expansion", EmitDefaultValue = false)]
        [JsonPropertyName("expansion")]
        public string Expansion { get; set; }

        [DataMember(Name = "age", EmitDefaultValue = false)]
        [JsonPropertyName("age")]
        public string Age { get; set; }

        [DataMember(Name = "created_in", EmitDefaultValue = false)]
        [JsonPropertyName("created_in")]
        public string CreatedIn { get; set; }

        [DataMember(Name = "cost", EmitDefaultValue = false)]
        [JsonPropertyName("cost")]
        public Costs Cost { get; set; }

        [DataMember(Name = "build_time", EmitDefaultValue = false)]
        [JsonPropertyName("build_time")]
        public int BuildTime { get; set; }

        [DataMember(Name = "reload_time", EmitDefaultValue = false)]
        [JsonPropertyName("reload_time")]
        public decimal ReloadTime { get; set; }

        [DataMember(Name = "attack_delay", EmitDefaultValue = false)]
        [JsonPropertyName("attack_delay")]
        public decimal AttackDelay { get; set; }

        [DataMember(Name = "movement_rate", EmitDefaultValue = false)]
        [JsonPropertyName("movement_rate")]
        public decimal MovementRate { get; set; }

        [DataMember(Name = "line_of_sight", EmitDefaultValue = false)]
        [JsonPropertyName("line_of_sight")]
        public int LineOfSight { get; set; }

        [DataMember(Name = "hit_points", EmitDefaultValue = false)]
        [JsonPropertyName("hit_points")]
        public int HitPoints { get; set; }

        [DataMember(Name = "range", EmitDefaultValue = false)]
        [JsonPropertyName("range")]
        public string Range { get; set; }

        [DataMember(Name = "attack", EmitDefaultValue = false)]
        [JsonPropertyName("attack")]
        public int Attack { get; set; }

        [DataMember(Name = "armor", EmitDefaultValue = false)]
        [JsonPropertyName("armor")]
        public string Armor { get; set; }

        [DataMember(Name = "attack_bonus", EmitDefaultValue = false)]
        [JsonPropertyName("attack_bonus")]
        public List<string> AttackBonus { get; set; }

        [DataMember(Name = "armor_bonus", EmitDefaultValue = false)]
        [JsonPropertyName("armor_bonus")]
        public List<string> ArmorBonus { get; set; }

        [DataMember(Name = "search_radius", EmitDefaultValue = false)]
        [JsonPropertyName("search_radius")]
        public int SearchRadius { get; set; }

        [DataMember(Name = "accuracy", EmitDefaultValue = false)]
        [JsonPropertyName("accuracy")]
        public string Accuracy { get; set; }

        [DataMember(Name = "blast_radius", EmitDefaultValue = false)]
        [JsonPropertyName("blast_radius")]
        public decimal BlastRadius { get; set; }
    }
}