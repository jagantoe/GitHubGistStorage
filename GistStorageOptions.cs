namespace GitHubGistStorage;

internal sealed class GistStorageOptions
{
	public GistStorageOptions(string projectName, string token, string gistId)
	{
		ProjectName = projectName;
		Token = token;
		GistId = gistId;
	}

	public string ProjectName { get; set; }
	public string Token { get; set; }
	public string GistId { get; set; }
}
