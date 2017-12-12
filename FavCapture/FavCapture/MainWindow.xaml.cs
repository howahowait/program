using System;
using System.Collections.Generic;
using System.Linq;
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
using CoreTweet;




namespace FavCapture
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        CoreTweet.Tokens tokens;
        CoreTweet.OAuth.OAuthSession session;

        const string ApiKey = "6GzH4f1PDvlYowp4Bjz8FS2Zj";
        const string ApiSecret = "g7Pf5VZlXg7spy9GV7wbTxZedvsSlzbBfNdBx4I5lxQd76xb59";


        public MainWindow()
        {

            InitializeComponent();

            // load twitter token
            if (!string.IsNullOrEmpty(Properties.Settings.Default.AccessToken)
                && !string.IsNullOrEmpty(Properties.Settings.Default.AccessTokenSecret))
            {
                tokens = Tokens.Create(
                    ApiKey
                    , ApiSecret
                    , Properties.Settings.Default.AccessToken
                    , Properties.Settings.Default.AccessTokenSecret);
            }
        }

        private void startSettingButton_Click(object sender, RoutedEventArgs e)
        {
            session = OAuth.Authorize(ApiKey, ApiSecret);
            pinUritextbox.Text = session.AuthorizeUri.ToString();

        }

        private void pinButton_Click(object sender, RoutedEventArgs e)
        {
            tokens = session.GetTokens(pinTextbox.Text);
            pinResultTextbox.Text = tokens.ToString();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (tokens != null)
            {
                Properties.Settings.Default.AccessToken = tokens.AccessToken;
                Properties.Settings.Default.AccessTokenSecret = tokens.AccessTokenSecret;
                Properties.Settings.Default.Save();
            }
        }

        private void showTimelineButton_Click(object sender, RoutedEventArgs e)
        {
            timelineTextbox.Clear();
            foreach (var status in tokens.Statuses.HomeTimeline())
                timelineTextbox.AppendText(
                    string.Format("{0}: {1}{2}", status.User.ScreenName, status.Text, Environment.NewLine));
        }
    }
}
