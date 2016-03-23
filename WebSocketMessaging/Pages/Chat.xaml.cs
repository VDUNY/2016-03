using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.AspNet.SignalR.Client;

namespace WebSocketMessaging
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Chat : PageFunction<ServerInfo>
    {
        private ServerInfo Server;

        public Chat(ServerInfo server)
        {
            Server = server;
            InitializeComponent();

            ConnectAsync();
        }


        /// <summary>
        /// The name for message formatting (there's still no authentication)
        /// </summary>
        public IHubProxy HubProxy { get; set; }
        public HubConnection Connection { get; set; }

        private ScrollViewer Scroller { get; set; }
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            HubProxy.Invoke("Send", Server.UserName, Message.Text);
            Message.Clear();
            Message.Focus();
        }


        /// <summary>
        /// Creates and connects the hub connection and hub proxy. This method
        /// is called asynchronously from SignInButton_Click.
        /// </summary>
        private async void ConnectAsync()
        {
            Connection = new HubConnection(Server.Server);
            Connection.Closed += Connection_Closed;

            HubProxy = Connection.CreateHubProxy("ChatHub");

            //Handle incoming event from server: use Invoke to switch to the UI thread from SignalR's thread
            HubProxy.On<string, string>("AddMessage", AddMessage);

            try
            {
                await Connection.Start();
            }
            catch (HttpRequestException)
            {
                //StatusText.Content = "Unable to connect to server: Start server before connecting clients.";
                //No connection: Don't enable Send button or show chat UI
                return;
            }
            Message.Focus();
        }

        // <Paragraph TextIndent="-50" Margin="50 0 0 0">
        //    <Run FontWeight="ExtraBold">10:00 Jaykul </Run>
        //    <Run>Lorem ipsum dolor sit amet</Run>
        private void AddMessage(string name, string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                var paragraph = new Paragraph
                {
                    Margin = new Thickness(50, 0, 0, 0),
                    TextIndent = -50
                };

                paragraph.Inlines.AddRange( new [] {
                    new Run($"{DateTimeOffset.Now:t} {name} ")
                    {
                        FontWeight = FontWeights.ExtraBold
                    },
                    new Run(message)
                });

                OutputViewer.Document.Blocks.Add(paragraph);

                OutputViewer.ScrollToEnd();
            });
        }

        private void Connection_Closed()
        {
            //Hide chat UI; show login UI
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() => NavigationService?.GoBack());
        }
    }
}
