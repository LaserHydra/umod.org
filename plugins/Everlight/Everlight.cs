using Newtonsoft.Json;
using System.ComponentModel;

namespace Oxide.Plugins
{
    [Info("Everlight", "Wulf/lukespragg", "3.1.0", ResourceId = 2170)]
    [Description("Allows infinite light from configured objects by not consuming fuel")]
    public class Everlight : CovalencePlugin
    {
        #region Configuration

        private Configuration config;

        public class Configuration
        {
            [DefaultValue(true)]
            [JsonProperty(PropertyName = "Campfires (true/false)", DefaultValueHandling = DefaultValueHandling.Populate)]
            public bool Campfires;

            [DefaultValue(false)]
            [JsonProperty(PropertyName = "Fire pits (true/false)", DefaultValueHandling = DefaultValueHandling.Populate)]
            public bool FirePits;

            [DefaultValue(false)]
            [JsonProperty(PropertyName = "Fireplaces (true/false)", DefaultValueHandling = DefaultValueHandling.Populate)]
            public bool Fireplaces;

            [DefaultValue(false)]
            [JsonProperty(PropertyName = "Furnaces (true/false)", DefaultValueHandling = DefaultValueHandling.Populate)]
            public bool Furnaces;

            [DefaultValue(false)]
            [JsonProperty(PropertyName = "Grills (true/false)", DefaultValueHandling = DefaultValueHandling.Populate)]
            public bool Grills;

            [DefaultValue(true)]
            [JsonProperty(PropertyName = "Lanterns (true/false)", DefaultValueHandling = DefaultValueHandling.Populate)]
            public bool Lanterns;

            [DefaultValue(false)]
            [JsonProperty(PropertyName = "Refineries (true/false)", DefaultValueHandling = DefaultValueHandling.Populate)]
            public bool Refineries;

            [DefaultValue(false)]
            [JsonProperty(PropertyName = "Search lights (true/false)", DefaultValueHandling = DefaultValueHandling.Populate)]
            public bool SearchLights;

        }

        protected override void LoadConfig()
        {
            base.LoadConfig();
            try
            {
                config = Config.ReadObject<Configuration>();
                if (config?.Lanterns == null) LoadDefaultConfig();
            }
            catch
            {
                LogWarning($"Could not read oxide/config/{Name}.json, creating new config file");
                LoadDefaultConfig();
            }
            SaveConfig();
        }

        protected override void LoadDefaultConfig() => config = new Configuration();

        protected override void SaveConfig() => Config.WriteObject(config);

        #endregion Configuration

        #region Fuel Magic

        private object OnFindBurnable(BaseOven oven)
        {
            if (oven?.fuelType == null) return null;

            if (oven.panelName.Contains("lantern") && !config.Lanterns) return null;
            else if (oven.panelName.Contains("campfire") || !config.Campfires) return null;
            else if (oven.panelName.Contains("fire_pit") || !config.FirePits) return null;
            else if (oven.panelName.Contains("furnace") && !config.Furnaces) return null;
            else if (oven.panelName.Contains("fireplace") && !config.Fireplaces) return null;
            else if (oven.panelName.Contains("bbq") && !config.Grills) return null;
            else if (oven.panelName.Contains("refinery") && !config.Refineries) return null;

            return ItemManager.CreateByItemID(oven.fuelType.itemid, 1);
        }

        #endregion Fuel Magic
    }
}
