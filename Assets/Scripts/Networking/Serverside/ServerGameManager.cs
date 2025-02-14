using System;
using System.Collections.Generic;

using SMGCore.EventSys;
using Voxels.Networking.Events;
using Voxels.Networking.Serverside;

namespace Voxels.Networking {
	public sealed class ServerGameManager {
		public static float  Time = 0f;
		public static Random Random;

		public static float TickTimeSeconds { get; private set; }

		Dictionary<string, BaseServersideController> _controllers = new Dictionary<string, BaseServersideController>();

		HashSet<BaseServersideController> _initQueue = new HashSet<BaseServersideController>();

		public bool Initialized {
			get {
				return _initQueue.Count == 0;
			}
		}

		public void Create(float tickTimeSeconds) {
			TickTimeSeconds = tickTimeSeconds;
			Time = 0f;
			Random = new Random();
			_controllers.Add("save-load",      new ServerSaveLoadController(this));
			_controllers.Add("server",         new ServerController(this));
			_controllers.Add("chat-server",    new ServerChatManager(this));
			_controllers.Add("players",        new ServerPlayerEntityManager(this));
			_controllers.Add("chunks",         new ServerChunkManager(this));
			_controllers.Add("procgen",        new ServerLandGenerator(this));
			_controllers.Add("worldstate",     new ServerDynamicEntityController(this));
			_controllers.Add("entity-server",  new ServerWorldStateController(this));
		}

		public void AddToInitQueue(BaseServersideController controller) {
			if ( controller == null ) {
				return;
			}
			_initQueue.Add(controller);
		}

		public void RemoveFromInitQueue(BaseServersideController controller) {
			if ( !_initQueue.Contains(controller) ) {
				return;
			}
			_initQueue.Remove(controller);
			if ( _initQueue.Count == 0 ) {
				EventManager.Fire(new OnServerInitializationFinished());
			} 
		}

		public void Reset() {
			foreach ( var pair in _controllers ) {
				try {
					pair.Value.Reset();
				}
				catch ( Exception e ) {
					DebugOutput.LogError(e);
				}
			}
		}

		public void Init() {
			foreach ( var pair in _controllers ) {
				try {
					pair.Value.Init();
				}
				catch ( Exception e ) {
					DebugOutput.LogError(e);
				}
			}
		}

		public void PostInit() {
			foreach ( var pair in _controllers ) {
				try {
					pair.Value.PostInit();
				}
				catch ( Exception e ) {
					DebugOutput.LogError(e);
				}
			}
		}

		public void Load() {
			foreach ( var pair in _controllers ) {
				var name = pair.Key;
				var controller = pair.Value;
				controller.Load();
			}
		}

		public void PostLoad() {
			foreach ( var pair in _controllers ) {
				pair.Value.PostLoad();
			}
		}

		public void Save() {
			foreach ( var pair in _controllers ) {
				pair.Value.Save();
			}
		}

		public void UpdateControllers() {
			foreach ( var pair in _controllers ) {
				try {
					pair.Value.Update();
				} catch (Exception e) {
					DebugOutput.LogException(e);
				}				
			}
			Time += TickTimeSeconds;
		}

		public void LateUpdateControllers() {
			foreach ( var pair in _controllers ) {
				try {
					pair.Value.LateUpdate();
				}
				catch ( Exception e ) {
					DebugOutput.LogException(e);
				}
			}
		}

		public void RareUpdateControllers() {
			foreach ( var pair in _controllers ) {
				try {
					pair.Value.RareUpdate();
				}
				catch ( Exception e ) {
					DebugOutput.LogException(e);
				}				
			}
		}
	}

}
