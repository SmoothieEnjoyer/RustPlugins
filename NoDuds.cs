using Newtonsoft.Json;
namespace Oxide.Plugins
{
	[Info("No Duds", "bearr", 1.1)]
	[Description("Prevents explosives from becoming dud")]
	class NoDuds : RustPlugin
	{
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

		private void Init()
		{
			permission.RegisterPermission("noduds.use", this);
		}

		object OnExplosiveDud(DudTimedExplosive explosive)
		{
			BasePlayer player = explosive.creatorEntity.ToPlayer();

			if (explosive.ShortPrefabName == "explosive.satchel.deployed" && permission.UserHasPermission(player.userID.ToString(), "noduds.use") == true)
			{
				if (config.satcheldud == false)
				{
					return true;
				}
				else if (config.satcheldud == true)
				{
					return null;
				}
			}
			else if (explosive.ShortPrefabName == "grenade.beancan.deployed" && permission.UserHasPermission(player.userID.ToString(), "noduds.use") == true)
			{
				PrintToChat("got here");
				if (config.beancandud == false)
				{
					PrintToChat("cancel");
					return true;
				}
				else if (config.beancandud == true)
				{
					return null;
				}
			}
			return null;
		}

		private class GameConfig
		{
			[JsonProperty("Satchel Charge Dud")] public bool satcheldud { get; set; }
			[JsonProperty("Beancan Dud")] public bool beancandud { get; set; }
		}

		private GameConfig GetDefaultConfig()
		{
			return new GameConfig
			{
				satcheldud = false,
				beancandud = false
			};
		}

		protected override void LoadDefaultConfig()
		{
			Config.WriteObject(GetDefaultConfig(), true);
		}
	}
}