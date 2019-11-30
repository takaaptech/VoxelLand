using Voxels.Networking.Clientside;

namespace Voxels.Networking {
	public class S_PingMessageHandler : BaseServerMessageHandler {
		public override void ProcessMessage(byte[] rawCommand) {
			base.ProcessMessage(rawCommand);
			var cc = ClientController.Instance;
			cc.SendNetMessage(ClientPacketID.Pong, new C_PongMessage());
		}

	}
}
