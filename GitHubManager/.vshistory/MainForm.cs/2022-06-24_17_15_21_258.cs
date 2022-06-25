using KEUtils.About;
using KEUtils.InputDialog;
using KEUtils.Utils;
using Octokit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
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
        private User CurrentUser;
        private string currentRepositoryOwner;
        private List<Repo> repoList;


        public MainForm() {
            InitializeComponent();
        }

        /// <summary>
        /// Appends the input to the textBoxInfo.
        /// </summary>
        /// <param name="line"></param>
        public void WriteInfo(string line = "") {
            textBox.AppendText(line);
        }

        /// <summary>
        /// Appends the input plus a NL to the textBoxInfo.
        /// </summary>
        /// <param name="line"></param>
        public void WriteLineInfo(string line = "") {
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

        }

        private async void CreateClient(Credentials credentials) {
            try {
                client = new GitHubClient(new Octokit.ProductHeaderValue(GitHubIdentity));
                if (credentials == null) {
                    CurrentUser = null;
                    WriteLineInfo("AuthenticationType=Not Authenticated");
                    return;
                }

                client.Credentials = credentials;
                WriteLineInfo("AuthenticationType=" + credentials.AuthenticationType.ToString());
                CurrentUser = await client.User.Current();
                Properties.Settings.Default.UserName = CurrentUser.Name;
                Properties.Settings.Default.Save();
                await GetCurrentUserInformation(client);
                WriteInfo(GetUserInformation(CurrentUser));
#if false
                // DEBUG
                await GetCurrentUserInformation(client);
                Test();
#endif
            } catch (Exception ex) {
                client = null;
                Utils.excMsg("Authentication Error ", ex);
                WriteLineInfo("Authentication Error"
                     + " (Be sure the token or password is what you intended)"
                     + NL + ex.Message + NL);
            }
        }

        private void SaveCsv(string fileName) {
            try {
                using (StreamWriter sw = new StreamWriter(fileName)) {
                    sw.WriteLine("Repository Information for " + currentRepositoryOwner);
                    sw.Write(Repo.GetSummary(repoList));
                    sw.WriteLine(Repo.CsvHeader());
                    foreach (Repo repo in repoList) {
                        sw.WriteLine(repo.CsvRow());
                    }
                }
                WriteLineInfo(NL + "SaveCsv: Wrote " + fileName);
            } catch (Exception ex) {
                string msg = "SaveCsv: Error writing CSV file " + fileName;
                WriteLineInfo(NL + msg);
                Utils.excMsg(msg, ex);
                return;
            }
        }

        private string GetUserInformation(User user) {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Login={user.Login}");
            builder.AppendLine($"Name={user.Name}");
            builder.AppendLine($"Id={user.Id}");
            builder.AppendLine($"AccountType={user.Type}");
            builder.AppendLine($"CreatedAt={user.CreatedAt.ToLocalTime()}");
            builder.AppendLine($"UpdatedAt={user.UpdatedAt.ToLocalTime()}");
            builder.AppendLine($"Email={user.Email}");
            builder.AppendLine($"Company={user.Company}");
            builder.AppendLine($"Blog={user.Blog}");
            builder.AppendLine($"Bio={user.Bio}");
            builder.AppendLine($"HtmlUrl={user.HtmlUrl}");
            builder.AppendLine($"Url={user.Url}");
            builder.AppendLine($"Location={user.Location}");
            builder.AppendLine($"AvatarUrl={user.AvatarUrl}");
            builder.AppendLine($"Followers={user.Followers}");
            builder.AppendLine($"Following={user.Following}");
            builder.AppendLine($"PublicRepos={user.PublicRepos}");
            builder.AppendLine($"PublicGists={user.PublicGists}");
            builder.AppendLine($"The following, if zero, may be inaccurate, owing to access restrictions:");
            builder.AppendLine($"DiskUsage={user.DiskUsage}");
            builder.AppendLine($"OwnedPrivateRepos={user.OwnedPrivateRepos}");
            builder.AppendLine($"TotalPrivateRepos={user.TotalPrivateRepos}");
            builder.AppendLine($"PrivateGists={user.PrivateGists}");
            return builder.ToString();
        }

        #region Async Tasks
        private async Task GetReleases(GitHubClient client, Repo repo, Repository repos) {
            try {
                // Note, using Owner.Login, not Owner.Name
                IReadOnlyList<Release> releases =
                    await client.Repository.Release.GetAll(repos.Owner.Login, repos.Name);
                repo.ReleaseCount = releases.Count;
            } catch (Exception) {
                repo.ReleaseCount = -1;
            }
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

        private async Task GetCollaborators(GitHubClient client, Repo repo, Repository repos) {
            try {
                IReadOnlyList < User > collaborators =
                    await client.Repository.Collaborator.GetAll(repos.Id);
                if (collaborators != null) repo.CollaboratorsCount = collaborators.Count;
            } catch (Exception) {
                repo.Readme = null;
            }
        }

        private async Task GetActivity(GitHubClient client, Repo repo, Repository repos) {
            try {
                IReadOnlyList<User> stargazers = await client.Activity.Starring.GetAllStargazers(repos.Owner.Login, repos.Name);
                if (stargazers != null) repo.StarCount = stargazers.Count;
                IReadOnlyList<User> watchers = await client.Activity.Watching.GetAllWatchers(repos.Owner.Login, repos.Name);
                if (watchers != null) repo.Watchers = watchers.Count;
            } catch (Exception) {
                repo.Readme = null;
            }
        }

        private async Task GetRepositories(string userName, IReadOnlyList<Repository> repositories) {
            try {
                repoList = new List<Repo>();
                currentRepositoryOwner = userName;
                Repo repo;
                List<Task> taskList = new List<Task>();
                foreach (Repository repos in repositories) {
                    repo = new Repo(repos);
                    repoList.Add(repo);
                    taskList.Add(GetReleases(client, repo, repos));
                    taskList.Add(GetReadme(client, repo, repos));
                    taskList.Add(GetCollaborators(client, repo, repos));
                    taskList.Add(GetActivity(client, repo, repos));
                    taskList.Add(GetParentName(client, repo, repos));
                }
                await Task.WhenAll(taskList);
                StringBuilder builder = new StringBuilder("Repositories"
                    + $" ({repositories.Count})" + NL);
                int n = 0;
                foreach (Repo repo1 in repoList) {
                    builder.Append($"{++n} ");
                    builder.Append(repo1.ToString());
                }
                WriteInfo(builder.ToString());
                WriteLineInfo("Summary");
                WriteInfo(Repo.GetSummary(repoList));
            } catch (Exception ex) {
                string msg = "Failed to get repository information";
                Utils.excMsg(msg, ex);
                WriteInfo(msg + NL + ex.Message);
            }
        }

        /// <summary>
        /// Gets current user information via REST.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private async Task GetCurrentUserInformation(GitHubClient client) {
            try {
                using (var httpClient = new HttpClient()) {
                    httpClient.BaseAddress = new Uri("https://api.github.com/");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(GitHubIdentity, "0"));
                    if (client.Credentials != null) {
                        // This works for both Oauth or Basic
                        // (Likely Basic is no longer used and is converted to Oauth)
                        httpClient.DefaultRequestHeaders.Authorization
                             = new AuthenticationHeaderValue("Bearer", client.Credentials.Password);
                    }
                    string requestUri = "user";
                    HttpResponseMessage response = await httpClient.GetAsync(requestUri);

                    if (response.IsSuccessStatusCode) {
                        Task<string> data = response.Content.ReadAsStringAsync();
                        if (data != null && !String.IsNullOrEmpty(data.Result)) {
                            string json = FormatJsonText(data.Result);
                            WriteLineInfo(json);
                            // TODO Handle the JSON
                            //SimpleRepository repository = JsonSerializer.Deserialize<SimpleRepository>(data.Result);

                        } else {
                            WriteLineInfo("GetCurrentUserInformation: No data");
                        }
                    } else {
                        WriteLineInfo("GetCurrentUserInformation returned " + response.StatusCode);
                    }
                }
            } catch (Exception ex) {
                string msg = "GetCurrentUserInformation: Exception occurred";
                Utils.excMsg(msg, ex);
                WriteLineInfo(msg + NL + ex.Message);
            }
        }

        /// <summary>
        /// Method to get the parent for a repository. Uses REST directly since
        /// Octokit.Net always returns null for Repository.Client.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="repo"></param>
        /// <param name="repos"></param>
        /// <returns></returns>
        private async Task GetParentName(GitHubClient client, Repo repo, Repository repos) {
            try {
                using (var httpClient = new HttpClient()) {
                    httpClient.BaseAddress = new Uri("https://api.github.com/");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(GitHubIdentity, "0"));
                    if (client.Credentials != null) {
                        // This works for both Oauth or Basic
                        // (Likely Basic is no longer used and is converted to Oauth)
                        httpClient.DefaultRequestHeaders.Authorization
                             = new AuthenticationHeaderValue("Bearer", client.Credentials.Password);
                    }
                    string requestUri = "repos/" + repos.Owner.Login + "/" + repos.Name;
                    HttpResponseMessage response = await httpClient.GetAsync(requestUri);

                    if (response.IsSuccessStatusCode) {
                        Task<string> data = response.Content.ReadAsStringAsync();
                        if (data != null && !String.IsNullOrEmpty(data.Result)) {
                            //string json = FormatJsonText(data.Result);
                            //WriteLineInfo(json);
                            SimpleRepository repository = JsonSerializer.Deserialize<SimpleRepository>(data.Result);
                            if (repository == null) {
                                repo.ParentName = "<Not Found>";
                                return;
                            }
                            if (repository.parent == null) {
                                repo.ParentName = "<None>";
                                return;
                            }
                            repo.ParentName = repository.parent.full_name;
                        } else {
                            repo.ParentName = "<Not Found>";
                        }
                    } else {
                        repo.ParentName = "<" + response.StatusCode + ">";
                    }
                }
            } catch (Exception ex) {
                WriteLineInfo("Exception getting Product for " + repos.FullName
                    + NL + ex.Message);
                repo.ParentName = "<Exception>";
            }
        }

        /// <summary>
        /// Test method for using REST.
        /// </summary>
        async private void Test() {
            WriteLineInfo(NL + "Test");
            try {
                using (var httpClient = new HttpClient()) {
                    httpClient.BaseAddress = new Uri("https://api.github.com/");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("test", "1"));
                    HttpResponseMessage response = await httpClient.GetAsync("repos/dmoonfire/ant-design-blazor");

                    if (response.IsSuccessStatusCode) {
                        Task<string> data = response.Content.ReadAsStringAsync();
                        if (data != null && !String.IsNullOrEmpty(data.Result)) {
                            string json = FormatJsonText(data.Result);
                            WriteLineInfo(json);
                            SimpleRepository repository = JsonSerializer.Deserialize<SimpleRepository>(data.Result);
                            WriteLineInfo("parent=" + repository.parent.full_name);
                        } else {
                            WriteLineInfo("Response is null or empty");
                        }
                    } else {
                        WriteLineInfo("Received " + response.StatusCode);
                    }
                }
#if false
                string uri = "https://api.github.com/repos/dmoonfire/ant-design-blazor";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Credentials = new NetworkCredential(httpClient.Credentials.Login, client.Credentials.Password);
                request.UserAgent = RequestConstants.UserAgentValue;
                string content = "<None>";
                using (var response = (HttpWebResponse)request.GetResponse()) {
                    using (var stream = response.GetResponseStream()) {
                        using (var sr = new StreamReader(stream)) {
                            content = sr.ReadToEnd();
                        }
                    }
                }
                WriteLineInfo(content);
#endif
            } catch (Exception ex) {
                Utils.excMsg("Test", ex);
            }
        }

        static string FormatJsonText(string jsonString) {
            using var doc = JsonDocument.Parse(
                jsonString,
                new JsonDocumentOptions {
                    AllowTrailingCommas = true
                }
            );
            MemoryStream memoryStream = new MemoryStream();
            using (
                var utf8JsonWriter = new Utf8JsonWriter(
                    memoryStream,
                    new JsonWriterOptions {
                        Indented = true
                    }
                )
            ) {
                doc.WriteTo(utf8JsonWriter);
            }
            return new System.Text.UTF8Encoding()
                .GetString(memoryStream.ToArray());
        }
        #endregion

        #region Event Handlers
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
            try {
                image = Image.FromFile(@".\GitHubManager.256x256.png");
            } catch (Exception ex) {
                Utils.excMsg("Failed to get AboutBox image", ex);
            }
            AboutBox dlg = new AboutBox(image, assembly);
            dlg.ShowDialog();
        }

        private void OnSaveCsvClick(object sender, EventArgs e) {
            if (repoList == null || repoList.Count == 0) {
                WriteLineInfo(NL + "Save CSV: No repositories to save");
                return;
            }
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Select CSV file";
            dlg.FileName = $"RepositoryInfo-{currentRepositoryOwner}" +
                $"-{DateTime.Now.ToString("yyyy-MM-dd")}.csv";
            dlg.Filter = "CSV|*.csv";
            if (dlg.ShowDialog() == DialogResult.OK) {
                SaveCsv(dlg.FileName);
            }
        }

        private async void OnGetRateLimitsClick(object sender, EventArgs e) {
            if (client == null) {
                WriteLineInfo(NL + "Get Rate Limits: No client defined");
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

        private async void OnGetRepositoriesClick(object sender, EventArgs e) {
            if (client == null) {
                WriteLineInfo(NL + "Get Repositories: No client defined");
                return;
            }
            if (CurrentUser == null) {
                WriteLineInfo(NL + "Get Repositories: Must be authenticated");
                return;
            }
            WriteLineInfo(NL + "Searching for Repositories for "
                + CurrentUser.Login);
            // This will get them all (no paging)
            IReadOnlyList<Repository> repositories = await client.Repository.GetAllForCurrent();
            await GetRepositories(CurrentUser.Login, repositories);
        }

        private async void OnGetUserRepositoriesClick(object sender, EventArgs e) {
            if (client == null) {
                WriteLineInfo(NL + "Get User Repositories: No client defined");
                return;
            }
            string userName;
            string msg = "Enter ";
            InputDialog dlg = new InputDialog("Repository Name", msg,
                Properties.Settings.Default.RepositoryOwner);
            DialogResult res = dlg.ShowDialog();
            if (res == DialogResult.OK) {
                userName = dlg.Value;
                Properties.Settings.Default.RepositoryOwner = userName;
                Properties.Settings.Default.Save();
                if (!String.IsNullOrEmpty(userName)) {
                    WriteLineInfo(NL + "Searching for Repositories for " + userName);
                } else {
                    WriteLineInfo(NL + "Searching for repositories: Invalid user name: "
                        + userName);
                    return;
                }
            } else {
                return;
            }
            // This will get them all (no paging)
            IReadOnlyList<Repository> repositories = await client.Repository.GetAllForUser(userName);
            await GetRepositories(userName, repositories);
        }

        /// <summary>
        /// Gets the repositories using Search. Not used as the information is 
        /// incomplete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnGetUserRepositoriesClick1(object sender, EventArgs e) {
            if (client == null) {
                WriteLineInfo(NL + "Get User Repositories: No client defined");
                return;
            }
            string userName;
            string msg = "Enter ";
            InputDialog dlg = new InputDialog("Repository Name", msg,
                Properties.Settings.Default.RepositoryOwner);
            DialogResult res = dlg.ShowDialog();
            if (res == DialogResult.OK) {
                userName = dlg.Value;
                Properties.Settings.Default.RepositoryOwner = userName;
                Properties.Settings.Default.Save();
                if (!String.IsNullOrEmpty(userName)) {
                    WriteLineInfo(NL + "Searching for Repositories for " + userName);
                } else {
                    WriteLineInfo(NL + "Searching for repositories: Invalid user name: "
                        + userName);
                    return;
                }
            } else {
                return;
            }
            User user = await client.User.Get(userName);
            if (user == null) {
                WriteLineInfo("User not found: " + userName);
                return;
            }
            List<Repository> repositories = new List<Repository>();
            int page = 1;
            // Handle more than 1 page
            while (true) {
                SearchRepositoryResult reposResult = await client.Search.SearchRepo(new SearchRepositoriesRequest() {
                    User = userName,
                    PerPage = 100,
                    Page = page++
                });
                IReadOnlyList<Repository> repositories1 = reposResult.Items;
                if (repositories1 == null) {
                    WriteLineInfo("Error finding repositories");
                    return;
                }
                foreach (Repository repos in repositories1) {
                    repositories.Add(repos);
                }
                //WriteLineInfo($"Items.Count={reposResult.Items.Count} " +
                //    $"TotalCount={reposResult.TotalCount} " +
                //    $"IncompleteResults={reposResult.IncompleteResults}");
                if (repositories1.Count < 100) break;
            }
            if (repositories.Count == 0) {
                WriteLineInfo("No repositories found");
                return;
            }
            await GetRepositories(userName, repositories);
        }

        private async void OnGetUserInformationClick(object sender, EventArgs e) {
            if (client == null) {
                WriteLineInfo(NL + "Get User Information: No client defined");
                return;
            }
            string userName;
            string msg = "Enter ";
            InputDialog dlg = new InputDialog("Repository Name", msg,
                Properties.Settings.Default.RepositoryOwner);
            DialogResult res = dlg.ShowDialog();
            if (res == DialogResult.OK) {
                userName = dlg.Value;
                Properties.Settings.Default.RepositoryOwner = userName;
                Properties.Settings.Default.Save();
                if (!String.IsNullOrEmpty(userName)) {
                    WriteLineInfo(NL + "User information for " + userName);
                } else {
                    WriteLineInfo(NL + "Get User Information: Invalid user name: "
                        + userName);
                    return;
                }
            } else {
                return;
            }
            User user = await client.User.Get(userName);
            WriteInfo(GetUserInformation(user));
        }
        #endregion

        public class SimpleRepository {
            public SimpleParent parent { get; set; }
        }
        public class SimpleParent {
            public string full_name { get; set; }
        }

        public class Repo {
            public static readonly string CSV_SEP = ",";
            public static readonly string[] HEADER = {
             "Name",
             "FullName",
             "Description",
             "Size (KB)",
             "Private",
             "Language",
             "License",
             "Readme",
             "ReleaseCount",
             "CollaboratorsCount",
             "OpenIssuesCount",
             "Fork",
             "ParentName",
             "ForksCount",
             "StarCount",
             "Watchers",
             "CreatedAt",
             "UpdatedAt",
             "PushedAt",
         };

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
            public int OpenIssuesCount { get; set; } = -1;
            public int CollaboratorsCount { get; set; } = -1;
            public bool Fork { get; set; }
            public int ForksCount { get; set; } = -1;
            public int ReleaseCount { get; set; } = -1;
            public Readme Readme { get; set; }
            public int StarCount { get; set; } = -1;
            public int Watchers { get; set; } = -1;
            public string HomePage { get; set; }
            public Repository Parent { get; set; }
            public string ParentName { get; set; }

            public Repo(Repository repos) {
                Name = repos.Name;
                FullName = repos.FullName;
                Description = repos.Description;
                Size = repos.Size;
                Private = repos.Private;
                License = repos.License;
                Language = repos.Language;
                CreatedAt = repos.CreatedAt;
                UpdatedAt = repos.UpdatedAt;
                PushedAt = repos.PushedAt;
                OpenIssuesCount = repos.OpenIssuesCount;
                Fork = repos.Fork;
                ForksCount = repos.ForksCount;
                //HomePage = repos.Homepage;
                Parent = repos.Parent;
                if (Parent != null) ParentName = Parent.FullName;
            }

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
                builder.AppendLine($"    Collaborators={CollaboratorsCount}");
                builder.AppendLine($"    OpenIssuesCount={OpenIssuesCount}");
                builder.AppendLine($"    Fork={Fork}");
                // This appears to always be null
                //builder.AppendLine($"    Parent={Parent}");
                builder.AppendLine($"    ParentName={ParentName}");
                builder.AppendLine($"    ForksCount={ForksCount}");
                builder.AppendLine($"    StarCount={StarCount}");
                builder.AppendLine($"    Watchers={Watchers}");
                //builder.AppendLine($"    HomePage={HomePage}");
                builder.AppendLine($"    CreatedAt={CreatedAt.ToLocalTime()}");
                builder.AppendLine($"    UpdatedAt={UpdatedAt.ToLocalTime()}");
                if (PushedAt.HasValue) {
                    builder.AppendLine($"    PushedAt={PushedAt.Value.ToLocalTime()}");
                } else {
                    builder.AppendLine($"    PushedAt=Never");
                }
                return builder.ToString();
            }

            public static string CsvHeader() {
                StringBuilder builder = new StringBuilder();
                foreach (string col in HEADER) {
                    builder.Append(col).Append(CSV_SEP);
                }
                // Remove the last separator
                string line = builder.ToString();
                line = line.Substring(0, line.Length - CSV_SEP.Length);
                return line;
            }

            public string CsvRow() {
                StringBuilder builder = new StringBuilder();
                builder.Append($"{Name}").Append(CSV_SEP);
                builder.Append($"{FullName}").Append(CSV_SEP);
                builder.Append($"\"{Description}\"").Append(CSV_SEP);
                builder.Append($"{Size}").Append(CSV_SEP);
                builder.Append($"{Private}").Append(CSV_SEP);
                builder.Append($"{Language}").Append(CSV_SEP);
                if (License != null) {
                    builder.Append($"{License.Name}").Append(CSV_SEP);
                } else {
                    builder.Append($"").Append(CSV_SEP);
                }
                if (Readme != null) {
                    builder.Append($"{Readme.Name}").Append(CSV_SEP);
                } else {
                    builder.Append($"").Append(CSV_SEP);
                }
                builder.Append($"{ReleaseCount}").Append(CSV_SEP);
                builder.Append($"{CollaboratorsCount}").Append(CSV_SEP);
                builder.Append($"{OpenIssuesCount}").Append(CSV_SEP);
                builder.Append($"{Fork}").Append(CSV_SEP);
                if (ParentName != null && ParentName.Equals("<None>")) {
                    // Leave it blank for <None>
                    builder.Append($"").Append(CSV_SEP);
                } else {
                    builder.Append($"{ParentName}").Append(CSV_SEP);
                }
                builder.Append($"{ForksCount}").Append(CSV_SEP);
                builder.Append($"{StarCount}").Append(CSV_SEP);
                builder.Append($"{Watchers}").Append(CSV_SEP);
                builder.Append($"{CreatedAt.ToLocalTime()}").Append(CSV_SEP);
                builder.Append($"{UpdatedAt.ToLocalTime()}").Append(CSV_SEP);
                if (PushedAt.HasValue) {
                    builder.Append($"{PushedAt.Value.ToLocalTime()}").Append(CSV_SEP);
                } else {
                    builder.Append($"").Append(CSV_SEP);
                }
                // Remove the last separator
                string line = builder.ToString();
                line = line.Substring(0, line.Length - CSV_SEP.Length);
                return line;
            }

            public static string GetSummary(List<Repo> repoList, string tab = "    ") {
                StringBuilder builder = new StringBuilder();
                int nPrivate = 0, nForked = 0, nMissingReadmes = 0, nMissingLicenses = 0,
                    nMissingDescriptions = 0, nOpenIssues = 0, nStars = 0, nWatchers = 0,
                    nCollaborators = 0;
                foreach (Repo repo in repoList) {
                    if (repo.Private) nPrivate++;
                    if (repo.Fork) nForked++;
                    if (repo.Readme == null || String.IsNullOrEmpty(repo.Readme.Name)) nMissingReadmes++;
                    if (repo.License == null || String.IsNullOrEmpty(repo.License.Name)) nMissingLicenses++;
                    if (String.IsNullOrEmpty(repo.Description)) nMissingDescriptions++;
                    if (repo.OpenIssuesCount > 0) nOpenIssues++;
                    nStars += repo.StarCount;
                    nWatchers += repo.Watchers;
                }
                builder.Append(tab).AppendLine($"Repositories={repoList.Count}");
                builder.Append(tab).AppendLine($"Private={nPrivate}");
                builder.Append(tab).AppendLine($"Forked={nForked}");
                builder.Append(tab).AppendLine($"Missing Descriptions={nMissingDescriptions}");
                builder.Append(tab).AppendLine($"Missing Readme's={nMissingReadmes}");
                builder.Append(tab).AppendLine($"Missing Licenses={nMissingLicenses}");
                builder.Append(tab).AppendLine($"Open Issues={nOpenIssues}");
                builder.Append(tab).AppendLine($"Stars={nStars}");
                builder.Append(tab).AppendLine($"Watchers={nWatchers}");
                builder.Append(tab).AppendLine($"Collaborators={nCollaborators}");
                return builder.ToString();
            }
        }
    }
}

