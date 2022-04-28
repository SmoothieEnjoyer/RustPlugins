using Newtonsoft.Json;
using System.Collections.Generic;

namespace Oxide.Plugins
{
	[Info("Unreliable Vehicles", "bearr", "1.0.0")]
	[Description("Vehicles have a chance of exploding when turned on.")]
	class UnreliableVehicles : RustPlugin
	{
		#region Init Config
		private GameConfig config;
		protected override void LoadConfig()
		{
			base.LoadConfig();
			try
			{
				config = Config.ReadObject<GameConfig>();
				if (config == null)
					PrintToConsole("Couldn't read config");

				Config.WriteObject(config);
			}
			catch
			{
				LoadDefaultConfig();
			}
		}
		#endregion

		#region Messages
		protected override void LoadDefaultMessages()
		{
			lang.RegisterMessages(new Dictionary<string, string>
			{
				["ExplosionText"] = "{0}'s vehicle exploded!"
			}, this);
		}
		#endregion

		#region Core/Hook/Oxide/The Code/The Good Stuff/What Does The Thing
		object OnEngineStart(BaseVehicle vehicle)
		{
			int number = UnityEngine.Random.Range(1, 101);
			if (number <= config.chanceofexploding && vehicle.gameObject != null && vehicle.GetDriver() != null)
			{
				FlameExplosive rocket = GameManager.server.CreateEntity("assets/prefabs/ammo/rocket/rocket_fire.prefab", vehicle.transform.position, vehicle.transform.rotation) as FlameExplosive;
				rocket.transform.position = new UnityEngine.Vector3(vehicle.transform.position.x, vehicle.transform.position.y, vehicle.transform.position.z);
				BasePlayer player = vehicle.GetDriver();
				vehicle.SetHealth(0.0f);
				vehicle.SetMaxHealth(0.0f);
				rocket.Explode();
				if (config.dobroadcastmessage == true)
				{
					PrintToChat(config.chatprefix + lang.GetMessage("ExplosionText", this), player.displayName);
				}
			}
			return null;
		}
		#endregion

		#region Config
		private class GameConfig
		{
			[JsonProperty("Chance Of Exploding")] public int chanceofexploding { get; set; }
			[JsonProperty("Broadcast Message To Chat")] public bool dobroadcastmessage { get; set; }
			[JsonProperty("Chat Prefix")] public string chatprefix { get; set; }
		}

		private GameConfig GetDefaultConfig()
		{
			return new GameConfig
			{
				chanceofexploding = 10,
				dobroadcastmessage = true,
				chatprefix = "<color=#ADFF2F>[Unreliable Vehicles]: </color>"
			};
		}

		protected override void LoadDefaultConfig()
		{
			Config.WriteObject(GetDefaultConfig(), true);
		}
		#endregion
	}
}
