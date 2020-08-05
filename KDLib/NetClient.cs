using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace KDLib
{
	public static class NetClient 
	{
		#region PrivateMembers
		private static TcpClient ThisClient = new TcpClient();

		private static TcpClient OnlClient = new TcpClient();
		
		private static ObservableCollection<string> _ClientComboBoxChoose;
		#endregion

		#region PublicProperties
		public static bool IsServerOnline { get; private set; }

		public static ObservableCollection<string> ClientComboBoxChoose
		{
			get
			{
				return _ClientComboBoxChoose;
			}
			set
			{
				_ClientComboBoxChoose = value;
				NotifyStaticPropertyChanged();
			}
		}

		public static void AddObservationCollectionAsync<T>(ICollection<T> collection, T item)
		{
			Action<T> addMethod = collection.Add;
			Application.Current.Dispatcher.BeginInvoke(addMethod, item);
		}

		public static IPAddress ServerIP { get; private set; }
		#endregion

		#region INotifyStaticPropertyChanged
		private static event EventHandler<PropertyChangedEventArgs> StaticPropertiesChanged;

		private static void NotifyStaticPropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (StaticPropertiesChanged != null)
            {
				StaticPropertiesChanged.Invoke(null, new PropertyChangedEventArgs(propertyName));
            }
		}

		private static void NotifyStaticPropertyChanged(params string[] Names)
		{
			if (StaticPropertiesChanged != null)
			{
				foreach (var propertyName in Names)
				{
					StaticPropertiesChanged(null, new PropertyChangedEventArgs(propertyName));
				}
			}
		}
		#endregion
		/// <summary>
		/// Initialize every things for a Client
		/// </summary>
		public static void Initialize()
        {
			ClientComboBoxChoose = new ObservableCollection<string>();
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

		public static Task Connect(IPAddress ServerAddress)
		{
			var tasks = new List<Task>();
			Task.Run(() =>
			{
				IsServerOnline = true;
				ServerIP = ServerAddress;

				ThisClient.Connect(ServerAddress, Data.PortForTCP);
				tasks.Add(ListenFromServer());
				tasks.Add(ProcessCommand());
			});
			return Task.WhenAll(tasks);
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
							SendCommand(new KDCommand((Data.OnFocus) ? CommandType.Forcusing : CommandType.LostForcus, OnlClient.Client.LocalEndPoint as IPEndPoint), OnlClient);;
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
								foreach (var c in JsonConvert.DeserializeObject<ObservableCollection<string>>(command.Content))
                                {
									AddObservationCollectionAsync(ClientComboBoxChoose, c);
                                }
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
								if (Data.Commands.Count > 0) command = Data.Commands.Dequeue();
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
					if (information != "") Data.Commands.Enqueue(JsonConvert.DeserializeObject<KDCommand>(information));
				}
			};
			await Task.Run(ThisAction);
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
			return Task.Factory.StartNew(ThisAction);
		}
	}
}
