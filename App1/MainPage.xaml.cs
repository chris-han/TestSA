using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            textBox.Text = "{ \"DeviceId\":\"dev - 01\", \"Temperature\":\"37.0\" }";
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {

            //Endpoint = sb://langfangbigdata.servicebus.windows.net/;SharedAccessKeyName=all;SharedAccessKey=XY63AiRTWSoAeq3JY5GMrjpSOElVKw7aTeYDcZnzeZo=
            //var key = @"yHYCZ/v3p0BJxzMhNLLOBAXeufhLXLR5IZeNur/oW5o=";
            var sas= @"SharedAccessSignature sr=https%3a%2f%2flangfangbigdata.servicebus.windows.net%2finputmsg%2fpublishers%2fclientapp%2fmessages&sig=aSLjx3CeLUVlOaXQEKRH5LwoFeM0gdeMYu10yA6%2b%2baw%3d&se=1449316622&skn=Send";

            //var sas = @"SharedAccessSignature sr=langfangbigdata.servicebus.windows.net/inputmsg/messages&sig=J0GNIEUMQtho5tm%2f%2fvF7MMS6%2flPYv4vaoFMEIvHmF%2fg%3d&se=1449313472&skn=all";
            //var sas = string.Format(@"SharedAccessSignature sr=langfangbigdata.servicebus.windows.net/inputmsg&sig={0}se=1449313472&skn=all", key);
            //sr = your -namespace.servicebus.windows.net&sig=tYu8qdH563Pc96Lky0SFs5PhbGnljF7mLYQwCZmk9M0%3d&se=1403736877&skn=RootManageSharedAccessKey

            var request = (HttpWebRequest)WebRequest.Create("https://langfangbigdata.servicebus.windows.net/inputmsg/publishers/clientApp/messages");


            var postData = textBox.Text;

            //var payload = JsonConvert.SerializeObject(deviceTelemetry);
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
