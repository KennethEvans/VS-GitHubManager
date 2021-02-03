using KEUtils.About;
using KEUtils.Utils;
using Octokit;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            // Doesn't handle blanks in the product name
            .Product.Replace(" ", "");
        private GitHubClient client;
        public User CurrentUser;
        private List<Repo> repoList;

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
                    WriteInfo("Authentication=None");
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

        private void OnAboutClick(object sender, EventArgs e) {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Image image = null;
            //try {
            //    image = Image.FromFile(@".\Help\GPXViewer256.png");
            //} catch (Exception ex) {
            //    Utils.excMsg("Failed to get AboutBox image", ex);
            //}
            AboutBox dlg = new AboutBox(image, assembly);
            dlg.ShowDialog();
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
            StringBuilder builder = new StringBuilder(NL + "Rate Limits");
            builder.AppendLine("Core");
            builder.AppendLine($"  Limit={coreRateLimit.Limit}");
            builder.AppendLine($"  Remaining={coreRateLimit.Remaining}");
            builder.AppendLine($"  Reset={coreRateLimit.Reset.ToLocalTime()}");
            builder.AppendLine("Search");
            builder.AppendLine($"  Limit={searchRateLimit.Limit}");
            builder.AppendLine($"  Remaining={searchRateLimit.Remaining}");
            builder.AppendLine($"  Reset={searchRateLimit.Reset.ToLocalTime()}");
            WriteInfo(builder.ToString());
        }

        private async Task GetReleases(GitHubClient client, Repo repo, Repository repos) {
            IReadOnlyList<Release> releases =
                await client.Repository.Release.GetAll(repos.Owner.Login, repos.Name);
            repo.ReleaseCount = releases.Count;
        }

        private async Task GetReadme(GitHubClient client, Repo repo, Repository repos) {
            try {
                Readme readme =
                    await client.Repository.Content.GetReadme(repos.Id);
                repo.Readme = readme;
            } catch (Exception) {
                repo.Readme = null;
            }
        }

        private async void OnGetRepositoriesClick(object sender, EventArgs e) {
            if (client == null) {
                WriteInfo(NL + "Get Repositories: No client defined");
                return;
            }
            Credentials credentials = client.Credentials;
            if (credentials == null) {
                WriteInfo(NL + "Get Repositories: Cannot determine credentials");
                return;
            }
            if (credentials.AuthenticationType == AuthenticationType.Anonymous) {
                WriteInfo(NL + "Get Repositories: Must be authenticated");
                return;
            }
            IReadOnlyList<Repository> repositories = await client.Repository.GetAllForCurrent();
            repoList = new List<Repo>();
            Repo repo;
            List<Task> taskList = new List<Task>();
            foreach (Repository repos in repositories) {
                repo = new Repo();
                repoList.Add(repo);
                repo.Name = repos.Name;
                repo.FullName = repos.FullName;
                repo.Description = repos.Description;
                repo.Size = repos.Size;
                repo.Private = repos.Private;
                repo.License = repos.License;
                repo.Language = repos.Language;
                repo.CreatedAt = repos.CreatedAt;
                repo.UpdatedAt = repos.UpdatedAt;
                repo.PushedAt = repos.PushedAt;
                repo.OpenIssuesCount = repos.OpenIssuesCount;
                repo.ForksCount = repos.ForksCount;
                taskList.Add(GetReleases(client, repo, repos));
                taskList.Add(GetReadme(client, repo, repos));
            }
            await Task.WhenAll(taskList);
            StringBuilder builder = new StringBuilder(NL + "Repositories"
                + $" ({repositories.Count})" + NL);
            int n = 0;
            foreach (Repo repo1 in repoList) {
                builder.Append($"{++n} ");
                builder.Append(repo1.ToString());
            }
            WriteInfo(builder.ToString());
        }
    }

    public class Repo {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public long Size { get; set; }
        public bool Private { get; set; }
        public string Language { get; set; }
        public LicenseMetadata License { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset? PushedAt { get; set; }
        public int OpenIssuesCount { get; set; }
        public int ForksCount { get; set; }
        public int ReleaseCount { get; set; }
        public Readme Readme { get; set; }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Name={Name}");
            builder.AppendLine($"    FullName={FullName}");
            builder.AppendLine($"    Description={Description}");
            builder.AppendLine($"    Size={Size} KB");
            builder.AppendLine($"    Private={Private}");
            builder.AppendLine($"    Language={Language}");
            if (License != null) {
                builder.AppendLine($"    License={License.Name}");
            } else {
                builder.AppendLine($"    License=<NA>");
            }
            if (Readme != null) {
                builder.AppendLine($"    Readme={Readme.Name}");
            } else {
                builder.AppendLine($"    Readme=<None>");
            }
            builder.AppendLine($"    ReleaseCount={ReleaseCount}");
            builder.AppendLine($"    OpenIssuesCount={OpenIssuesCount}");
            builder.AppendLine($"    ForksCount={ForksCount}");
            builder.AppendLine($"    CreatedAt={CreatedAt.ToLocalTime()}");
            builder.AppendLine($"    UpdatedAt={UpdatedAt.ToLocalTime()}");
            if (PushedAt.HasValue) {
                builder.AppendLine($"    PushedAt={PushedAt.Value.ToLocalTime()}");
            } else {
                builder.AppendLine($"    PushedAt=Never");
            }
            return builder.ToString();
        }
    }
}

