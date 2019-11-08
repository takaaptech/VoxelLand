namespace Voxels.Networking {
	public class BaseClientMessageHandler {
		public virtual void ProcessMessage(ClientState client, byte[] rawCommand) {
			
		}
	}

	public class BaseServerMessageHandler {
		public virtual void ProcessMessage(byte[] rawCommand) {

		}
	}
}

