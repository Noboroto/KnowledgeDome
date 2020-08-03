using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using KDLib.KDException;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

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

		public static List<string> ClientComboBoxChoose { get; private set; }

        public static IPAddress ServerIP { get; private set; }
        #endregion

        private async static Task<bool> IsValidConnection(IPAddress ip)
		{
			using (TcpClient tcp = new TcpClient())
			{
				var taskconnect = tcp.ConnectAsync(ip, Data.PortForValidCheck);
				var timer = Task.Delay(500);

				var result = await Task.WhenAny(new[] { taskconnect, timer });
				return result == taskconnect;
			}
		}

		public async static Task Connect(string ip)
        {
			try
            {
				IPAddress tmp;
				if (IPAddress.TryParse(ip, out tmp)) await Connect(tmp);
				else throw new IPWrongFormat();
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

		public async static Task Connect(IPAddress ServerAddress)
		{
			try
			{
				if (await IsValidConnection(ServerAddress))
                {
					IsServerOnline = true;
					ServerIP = ServerAddress;
					return;
					/*
					ThisClient.Connect(ServerAddress, Data.PortForTCP);

					var Tasks = new List<Task>();
					Tasks.Add(ListenFromServer());
					Tasks.Add(ProcessCommand());
					
					await Task.WhenAll(Tasks.ToArray());*/
				}
				else
                {
					IsServerOnline = false;
					ServerIP = null;
					throw new IPNotFoundException();
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

		private async static void CheckOnlineServer()
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
							SendCommand(new KDCommand((Data.OnFocus) ? CommandType.Forcusing : CommandType.LostForcus, OnlClient.Client.LocalEndPoint), OnlClient);;
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
			await Task.Factory.StartNew(ThisAction);
		}

		private async static Task ProcessCommand ()
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
							case CommandType.ClientList:
								ClientComboBoxChoose = JsonConvert.DeserializeObject<List<string>>(Data.Commands.Dequeue().Content);
								goto EndCommand;
							case CommandType.AccpetConnect:
								OnlClient.Connect(ServerIP, Data.PortForChecker);
								CheckOnlineServer();
								goto EndCommand;
							case CommandType.RefuseConnect:
								ThisClient.Close();
								Data.Commands.Clear();
								return;
							EndCommand:
								command = Data.Commands.Dequeue();
								continue;
							default:
								continue;
                        }
                    }
                }
			};
			await Task.Factory.StartNew(ThisAction);
		}

		public static async void SendCommand(KDCommand Command, TcpClient tcp)
		{
			try
            {
				await SendMessage(JsonConvert.SerializeObject(Command), tcp);
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

		private static Task ListenFromServer()
		{
			Action ThisAction = () =>
			{
				using (StreamReader ReadFromStream = new StreamReader(ThisClient.GetStream()))
				{
					while (true)
					{
						string information = "";
						try
						{
							information = ReadFromStream.ReadToEnd();
						}
						catch (ObjectDisposedException)
						{
							break;
						}
						catch (AggregateException ae)
						{
							throw ae.Flatten();
						}
						catch
						{
							throw;
						}
						if (information != "") Data.Commands.Enqueue(JsonConvert.DeserializeObject<KDCommand>(information));
					}
				}
			};
			return Task.Factory.StartNew(ThisAction);
		}

		private static Task SendMessage(string command, TcpClient tcp)
		{
			Action ThisAction = () => {
				using (StreamWriter WriteToStream = new StreamWriter(tcp.GetStream()) { AutoFlush = true })
				{
					try
					{
						WriteToStream.Write(command);
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
			return Task.Factory.StartNew(ThisAction);
		}
	}
}
