using Octokit;
using System.Text.Json;

namespace GitHubGistStorage;

public interface IGistClient
{
	RateLimit? GetRateLimit();
	Task<Gist> GetGist();
	Task<Dictionary<string, string>> GetGistDictionary();
	Task<Gist> UpdateGist(string file, object content);
	Task<Gist> UpdateGist(string file, string content);
}

internal sealed class GistClient : IGistClient
{
	private readonly GistStorageOptions _gistStorageOptions;
	private readonly GitHubClient _client;

	public GistClient(GistStorageOptions gistStorageOptions)
	{
		_gistStorageOptions = gistStorageOptions;
		_client = new GitHubClient(new ProductHeaderValue(_gistStorageOptions.ProjectName));
		_client.Credentials = new Credentials(gistStorageOptions.Token);
	}

	/// <summary>
	/// Get the rate limit based on the latest call
	/// </summary>
	/// <returns></returns>
	public RateLimit? GetRateLimit()
	{
		return _client.GetLastApiInfo()?.RateLimit;
	}

	/// <summary>
	/// Get the gist
	/// </summary>
	/// <returns></returns>
	public async Task<Gist> GetGist()
	{
		return await _client.Gist.Get(_gistStorageOptions.GistId);
	}
	/// <summary>
	/// Get the gist files as a dictionary
	/// </summary>
	/// <returns></returns>
	public async Task<Dictionary<string, string>> GetGistDictionary()
	{
		var gist = await _client.Gist.Get(_gistStorageOptions.GistId);
		var dict = new Dictionary<string, string>();
		foreach (var file in gist.Files)
		{
			dict.Add(file.Key, file.Value.Content);
		}
		return dict;
	}

	/// <summary>
	/// Update a file in the gist
	/// </summary>
	/// <param name="file">The file name</param>
	/// <param name="content">The content which will be serialized</param>
	/// <returns></returns>
	public async Task<Gist> UpdateGist(string file, object content)
	{
		var fileUpdate = new GistFileUpdate()
		{
			Content = JsonSerializer.Serialize(content)
		};
		var gist = new GistUpdate();
		gist.Files.Add(file, fileUpdate);
		return await _client.Gist.Edit(_gistStorageOptions.GistId, gist);
	}
	/// <summary>
	/// Update a file in the gist
	/// </summary>
	/// <param name="file">The file name</param>
	/// <param name="content">The content</param>
	/// <returns></returns>
	public async Task<Gist> UpdateGist(string file, string content)
	{
		var fileUpdate = new GistFileUpdate()
		{
			Content = content
		};
		var gist = new GistUpdate();
		gist.Files.Add(file, fileUpdate);
		return await _client.Gist.Edit(_gistStorageOptions.GistId, gist);
	}
}