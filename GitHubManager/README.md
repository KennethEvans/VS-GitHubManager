# VS-GitHubManager

GitHub Manager is a C# application that uses the GitHub API to get information about repositories.

You can get information about your own repository including private repositories if you have an access token and about other's repositories using their login name.  You can save the results as a CSV file.

It is not required to have an access token, but without one you will not get private repsoitories and your rate limits will be significantly reduced. See https://docs.github.com/en/github/authenticating-to-github/creating-a-personal-access-token

See https://kennethevans.github.io/index.html#GitHubManager.


**Installation**

If you are installing from a download, just unzip the files into a directory somewhere convenient. Then run it from there. If you are installing from a build, copy these files and directories from the bin/Release directory to a convenient directory. This is a .Net 5 project, so you can also use Publish.

* GitHubManager.exe
* GitHubManager.dll
* Octokit.dll
* Utils.dll
* GitHubManager.256x256.png

To uninstall, just delete these files. 

**Development**

GitHub Manager uses the NuGet package Octokit as well as the class library Utils.dll from https://github.com/KennethEvans/VS-Utils.

It is hosted at https://github.com/KennethEvans/VS-GitHubManager.


**More Information**

More information and FAQ are at https://kennethevans.github.io as well as more projects from the same author.

Licensed under the MIT license. (See: https://en.wikipedia.org/wiki/MIT_License)