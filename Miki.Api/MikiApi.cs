﻿using Miki.API.Leaderboards;
using Miki.Rest;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Miki.API
{
	public class MikiApiClient : IDisposable
	{
		private readonly RestClient _client;

		private const string _baseUrl = "https://api.miki.ai/";

		public MikiApiClient(string token)
		{
			_client = new RestClient(_baseUrl);
            _client.AddHeader("Authorization", "Bearer " + token);
		}

		/// <summary>
		/// Builds the url to the leaderboards page on the miki website
		/// </summary>
		/// <param name="options">Leaderboards Options Object</param>
		/// <returns>https://miki.ai/leaderboards/{guild_id?}/{type}</returns>
		public string BuildLeaderboardsUrl(LeaderboardsOptions options)
			=> "https://miki.ai" + BuildLeaderboardsRoute(options);

		/// <summary>
		/// Pulls the leaderboards data from the API
		/// </summary>
		/// <param name="options">Leaderboards Options Object</param>
		public async Task<LeaderboardsObject> GetPagedLeaderboardsAsync(LeaderboardsOptions options)
			=> (await _client.GetAsync<LeaderboardsObject>(BuildLeaderboardsRoute(options))).Data;

		private string BuildLeaderboardsRoute(LeaderboardsOptions options)
		{
			StringBuilder sb = new StringBuilder()
				.Append("/leaderboards");

			if (options.GuildId.HasValue)
			{
				sb.Append($"/{options.GuildId}");
			}

			sb.Append($"/{options.Type.ToString().ToLower()}");


			QueryString qs = new QueryString();

			qs.Add("amount", options.Amount);
			qs.Add("offset", options.Offset);

			return sb.ToString() + qs.Query;
		}

		public void Dispose()
		{
			_client.Dispose();
		}
	}
}