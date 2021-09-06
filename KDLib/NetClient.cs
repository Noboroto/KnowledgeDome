using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KDLib
{
	public static class NetClient
	{
		#region PrivateMembers
		private static TcpClient ThisClient = new TcpClient();
		private static TcpClient OnlClient = new TcpClient();
		#endregion

		#region PublicProperties
		public static bool IsServerOnline { get; private set; }

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
			using (TcpClient tcp = new TcpClient())
			{
				var taskconnect = tcp.ConnectAsync(ip, Data.PortForValidCheck);
				var timer = Task.Delay(500);
				var result = await Task.WhenAny(new[] { taskconnect, timer });
				return result == taskconnect;
			}
		}

		public async static Task Connect(IPAddress ServerAddress)
		{
			var tasks = new List<Task>();
			await Task.Run(() =>
			{
				IsServerOnline = true;
				ServerIP = ServerAddress;

				ThisClient.Connect(ServerAddress, Data.PortForTCP);
				tasks.Add(ListenFromServer());
				tasks.Add(ProcessCommand());
			}, MustCancel.Token);
			await Task.WhenAll(tasks);
		}

		private async static Task CheckOnlineServer()
		{
			Action ThisAction = async () =>
			{
				while (true)
				{
					try
					{
						if (ThisClient.Client.Poll(500, SelectMode.SelectRead) && ThisClient.Client.Available == 0)
						{
							if (!IsServerOnline) await Connect(ServerIP);
							IsServerOnline = true;
							SendCommand(new KDCommand(Data.OnFocus ? CommandType.Forcusing : CommandType.LostForcus, OnlClient.Client.LocalEndPoint as IPEndPoint), OnlClient);
							await Task.Delay(1000);
						}
						else
						{
							IsServerOnline = false;
						}
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
			};
			await Task.Run(ThisAction, MustCancel.Token);
		}

		private async static Task ProcessCommand()
		{
			Action ThisAction = () =>
			{
				while (true)
				{
					if (Data.Commands.Count > 0)
					{
						KDCommand command = Data.Commands.Peek();
						switch (command.PrefixCmd)
						{
							case CommandType.AccpetConnect:
								OnlClient.Connect(ServerIP, Data.PortForChecker);
								CheckOnlineServer().Start();
								goto EndCommand;
							case CommandType.RefuseConnect:
								ThisClient.Close();
								Data.Commands.Clear();
								return;
							EndCommand:
								if (Data.Commands.Count > 0) command = Data.Commands.Dequeue();
								continue;
							default:
								continue;
						}
					}
				}
			};
			await Task.Run(ThisAction, MustCancel.Token);
		}



		private async static Task ListenFromServer()
		{
			Action ThisAction = () =>
			{
				StreamReader ReadFromStream = new StreamReader(ThisClient.GetStream());
				bool CanLive = true;
				while (true && CanLive)
				{
					string information = "";
					try
					{
						information = ReadFromStream.ReadLineAsync().Result;
					}
					catch (ObjectDisposedException)
					{
						break;
					}
					catch (AggregateException ae)
					{
						foreach (var e in ae.InnerExceptions)
						{
							if (e is SocketException || e is IOException)
							{
								ThisClient.Close();
								CanLive = false;
								break;
							}
						}
						throw ae.Flatten();
					}
					catch
					{
						throw;
					}
					if (information != "") Data.Commands.Enqueue(KDCommand.FromJson(information));
				}
			};
			await Task.Run(ThisAction, MustCancel.Token);
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

		private async static Task SendMessage(string command, TcpClient tcp)
		{
			Action ThisAction = () =>
			{
				using (StreamWriter WriteToStream = new StreamWriter(tcp.GetStream()) { AutoFlush = true })
				{
					try
					{
						WriteToStream.Write(command);
					}
					catch (AggregateException ae)
					{
						foreach (var e in ae.InnerExceptions)
						{
							if (e is IOException) continue;
							if (e is SocketException)
							{
								ThisClient.Close();
								break;
							}
						}
					}
					catch
					{
						throw;
					}
				}
			};
			await Task.Run(ThisAction, MustCancel.Token);
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
