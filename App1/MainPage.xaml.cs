using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Net;
using System.Text;

namespace SimpleEventHubPublisher
{

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            textBox.Text = "{ \"DeviceId\":\"dev - 01\", \"Temperature\":\"37.0\" }";
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {

            var sas= @"SharedAccessSignature sr=https%3a%2f%2flangfangbigdata.servicebus.windows.net%2finputmsg%2fpublishers%2fclientapp%2fmessages&sig=aSLjx3CeLUVlOaXQEKRH5LwoFeM0gdeMYu10yA6%2b%2baw%3d&se=1449316622&skn=Send";

              var request = (HttpWebRequest)WebRequest.Create("https://langfangbigdata.servicebus.windows.net/inputmsg/publishers/clientApp/messages");


            var postData = textBox.Text;

            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.Headers["Authorization"] = sas;
            request.ContentType = "application/atom+xml;type=entry;charset=utf-8";
            int length = Encoding.UTF8.GetByteCount(postData);
            request.Headers["ContentLength"] = length.ToString();

            using (var stream = await request.GetRequestStreamAsync())
            {
                stream.Write(data, 0, length);
            }

            var response = await request.GetResponseAsync();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            textBox1.Text = responseString.ToString();
        }
    }
}
