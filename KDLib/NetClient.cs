using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace KDLib
{
	public static class NetClient
	{
		#region PrivateMembers
		private static TcpClient ThisClient;
		private const int DELAY = 500;
		#endregion

		#region PublicProperties
		public static CancellationTokenSource MustCancel { get; } = new CancellationTokenSource();
		public static IPAddress ServerIP { get; private set; }
		#endregion

		/// <summary>
		/// Initialize every things for a Client
		/// </summary>
		public static void Initialize()
		{
		}

		/// <param name="ip">The IP address you want to check</param>
		/// <exception cref="ObjectDisposedException"/>
		/// <exception cref="SocketException"/>
		/// <returns>True if the parameter can be used for a connection; otherwise, false</returns>
		public async static Task<bool> IsValidConnection(IPAddress ip)
		{
			ThisClient = new TcpClient();
			var taskconnect = ThisClient.ConnectAsync(ip, Data.PortForTCP);
			var timer = Task.Delay(DELAY);
			var result = await Task.WhenAny(new[] { taskconnect, timer });
			if (result == taskconnect)
			{
				ServerIP = ip;
				ListenFromServer();
				return true;
			}
			ThisClient.Close();
			return false;
		}

		private static void ListenFromServer()
		{
			Task.Run(() =>
			{
				StreamReader ReadFromStream = new StreamReader(ThisClient.GetStream());
				string information = "";
				while (true)
				{
					information = "";
					information = ReadFromStream.ReadLine();
					if (KDCommand.FromJson(information) != null)
					{
						var command = KDCommand.FromJson(information);
						switch (command.PrefixCmd)
						{
							case CommandType.MCToMC:
							case CommandType.ServerToMC:
							case CommandType.EditScore:
							case CommandType.ChoosePlayer:
							case CommandType.NavigateToRound:
								Data.FrameCommands.Enqueue(command);
								break;
							case CommandType.ChangeMatchToID:
							case CommandType.StopEmergency:
							case CommandType.StartTimmer:
							case CommandType.Right:
							case CommandType.Wrong:
							case CommandType.NextQuestAt:
							case CommandType.ClientList:
							case CommandType.ConfirmIP:
							case CommandType.RefuseConnect:
								Data.RoundCommnads.Enqueue(command);
								break;
							case CommandType.AccpetConnect:
								Data.RoundCommnads.Enqueue(command);
								Data.FrameCommands.Enqueue(command);
								break;
						}
					}
				}
			});
		}

		public static void SendCommand(KDCommand Command)
		{
			SendCommand(Command, ThisClient);
		}

		public static async void SendCommand(KDCommand Command, TcpClient tcp)
		{
			try
			{
				await SendMessage(Command.ToJson(), tcp);
			}
			catch (AggregateException ae)
			{
				throw ae.Flatten();
			}
			catch
			{
				throw;
			}
		}
		private static Task SendMessage(string command, TcpClient tcp)
		{
			return Task.Run(async () =>
			{
				try
				{
					StreamWriter WriteToStream = new StreamWriter(tcp.GetStream()) { AutoFlush = true };
					await WriteToStream.WriteLineAsync(command);
				}
				catch (AggregateException ae)
				{
					throw ae.Flatten();
				}
				catch
				{
					throw;
				}
			}, MustCancel.Token);
		}
		public static bool CheckSameNetwork(string firstIP, string secondIP)
		{
			string subNet = GetSubnetMask(IPAddress.Parse(firstIP)).ToString();
			uint subnetmaskInInt = ConvertIPToUint(subNet);
			uint firstIPInInt = ConvertIPToUint(firstIP);
			uint secondIPInInt = ConvertIPToUint(secondIP);
			uint networkPortionofFirstIP = firstIPInInt & subnetmaskInInt;
			uint networkPortionofSecondIP = secondIPInInt & subnetmaskInInt;
			if (networkPortionofFirstIP == networkPortionofSecondIP)
				return true;
			else
				return false;
		}
		private static uint ConvertIPToUint(string ipAddress)
		{
			System.Net.IPAddress iPAddress = System.Net.IPAddress.Parse(ipAddress);
			byte[] byteIP = iPAddress.GetAddressBytes();
			uint ipInUint = (uint)byteIP[3] << 24;
			ipInUint += (uint)byteIP[2] << 16;
			ipInUint += (uint)byteIP[1] << 8;
			ipInUint += (uint)byteIP[0];
			return ipInUint;
		}
		public static IPAddress GetSubnetMask(IPAddress address)
		{
			foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
			{
				foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
				{
					if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
					{
						if (address.Equals(unicastIPAddressInformation.Address))
						{
							return unicastIPAddressInformation.IPv4Mask;
						}
					}
				}
			}
			throw new ArgumentException(string.Format("Can't find subnetmask for IP address '{0}'", address));
		}
	}
}
