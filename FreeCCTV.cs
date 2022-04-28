using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("Free CCTV", "SmoothieEnjoyer", "0.1.2")]
    [Description("Allows CCTVs to operate without electricity.")]
    
    class FreeCCTV : RustPlugin
    {
        #region Initialization
        void OnServerInitialized()
        {
            ChangePower(999999);
        }
        #endregion
        
        #region cctv
        void OnEntitySpawned(CCTV_RC cctv)
        {
            cctv.UpdateHasPower(999999, 1);
            cctv.SendNetworkUpdateImmediate();
        }
         
        void ChangePower(int amt)
        {
            foreach (var cctv in UnityEngine.Object.FindObjectsOfType<CCTV_RC>())
            {
                cctv.UpdateHasPower(amt, 1);
				cctv.SendNetworkUpdateImmediate();
            }
        }
        void Unload()
        {
            ChangePower(0);
        }
        #endregion
    }
}