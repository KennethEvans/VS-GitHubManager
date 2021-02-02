using KEUtils.Utils;
using Octokit;
using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitHubManager {
    public partial class MainForm : Form {
        public static readonly String NL = Environment.NewLine;
        public const string CSV_SEP = ",";
        /// <summary>
        /// A unique name that identifies the client to GitHub.  This should be
        /// the name of the product, GitHub organization, or the GitHub username
        /// (in that order of preference) that is using the Octokit framework.
        ///</summary>
        public static readonly string GitHubIdentity = Assembly
            .GetEntryAssembly()
            .GetCustomAttribute<AssemblyProductAttribute>()
            .Product;
        private GitHubClient client;
        public User CurrentUser;

        public MainForm() {
            InitializeComponent();
        }

        /// <summary>
        /// Appends the input plus a NL to the textBoxInfo.
        /// </summary>
        /// <param name="line"></param>
        public void WriteInfo(string line) {
            textBox.AppendText(line + NL);
        }

        private void ShowLoginForm() {
            while (client == null)
                using (var dialog = new LoginForm())
                    if (dialog.ShowDialog(this) == DialogResult.OK) {
                        CreateClient(dialog.Credentials);
                    } else {
                        Close();
                        return;
                    }

            //EnableUI(true);
        }

        private void CreateClient(Credentials credentials) {
            try {
                client = new GitHubClient(new ProductHeaderValue(GitHubIdentity));
                if (credentials == null) {
                    CurrentUser = null;
                    WriteInfo("Authentication=<None>");
                    return;
                }

                client.Credentials = credentials;
                CurrentUser = client.User
                    .Current()
                    .GetAwaiter()
                    .GetResult();
                Properties.Settings.Default.UserName = CurrentUser.Name;
                Properties.Settings.Default.Save();
                string info = "Authentication="
                    + credentials.AuthenticationType.ToString()
                    + " UserName=" + CurrentUser.Name;
                WriteInfo(info);
            } catch (Exception ex) {
                client = null;
                Utils.excMsg("Authentication Error", ex);
                WriteInfo("Authentication Error");
            }
        }

        private void OnFormLoad(object sender, EventArgs e) {
            BeginInvoke((MethodInvoker)ShowLoginForm);
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e) {
            // TODO
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e) {
            //if (!mainBackgroundWorker.IsBusy)
            //    return;

            //if (!isExitPending && mainBackgroundWorker.CancellationPending)
            //    mainBackgroundWorker.CancelAsync();

            //isExitPending = true;
            //e.Cancel = true;
            //return;
        }

        private void OnClearClick(object sender, EventArgs e) {
            textBox.Clear();
        }

        private void OnExitClick(object sender, EventArgs e) {
            Close();
        }

        private async void OnGetRateLimitsClick(object sender, EventArgs e) {
            if (client == null) {
                WriteInfo(NL + "Get Rate Limits: No client defined");
                return;
            }
            MiscellaneousRateLimit miscellaneousRateLimit = await client.Miscellaneous.GetRateLimits();
            //  The "core" object provides your rate limit status except for the Search API.
            RateLimit coreRateLimit = miscellaneousRateLimit.Resources.Core;
            RateLimit searchRateLimit = miscellaneousRateLimit.Resources.Search;
            StringBuilder builder = new StringBuilder(NL + "Rate Limits" + NL);
            builder.Append("Core" + NL);
            builder.Append($"  Limit={coreRateLimit.Limit}" + NL);
            builder.Append($"  Remaining={coreRateLimit.Remaining}" + NL);
            builder.Append($"  Reset={coreRateLimit.Reset.ToLocalTime()}" + NL);
            builder.Append("Search" + NL);
            builder.Append($"  Limit={searchRateLimit.Limit}" + NL);
            builder.Append($"  Remaining={searchRateLimit.Remaining}" + NL);
            builder.Append($"  Reset={searchRateLimit.Reset.ToLocalTime()}" + NL);
            WriteInfo(builder.ToString());
        }
    }
}
