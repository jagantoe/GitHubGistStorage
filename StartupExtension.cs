using Microsoft.Extensions.DependencyInjection;

namespace GitHubGistStorage;

public static class StartupExtension
{
	/// <summary>
	/// Configure the gist storage
	/// </summary>
	/// <param name="token">The GitHub Personal access token, make sure it has the gist scope!</param>
	/// <returns></returns>
	public static IServiceCollection ConfigureGistStorage(this IServiceCollection services, string projectName, string token, string gistId)
	{
		services.AddSingleton(new GistStorageOptions(projectName, token, gistId));
		services.AddSingleton<IGistClient, GistClient>();
		return services;
	}
}
