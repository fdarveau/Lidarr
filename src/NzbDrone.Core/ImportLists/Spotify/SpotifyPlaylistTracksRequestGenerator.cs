using System;
using System.Collections.Generic;
using NzbDrone.Common.Http;

namespace NzbDrone.Core.ImportLists.Spotify
{
    public class SpotifyPlaylistTracksRequestGenerator : SpotifyRequestGeneratorBase<SpotifyPlaylistTracksSettings>
    {
        public override ImportListPageableRequestChain GetSpotifyListItems()
        {
            var pageableRequests = new ImportListPageableRequestChain();

            pageableRequests.Add(GetPagedRequests());

            return pageableRequests;
        }

        private IEnumerable<ImportListRequest> GetPagedRequests()
        {
            var maxPages = PageSize <= 0 ? 1 : (int)Math.Ceiling((double)Settings.Count / PageSize);
            var baseUrl = $"{Settings.BaseUrl.TrimEnd('/')}/playlists/{Settings.PlaylistId}/tracks?fields=items(track(name,album(name,id),artists))&limit={PageSize}";

            for (var page = 0; page < maxPages; page++)
            {
                yield return AddTokenToRequest(new ImportListRequest($"{baseUrl}&offset={page * PageSize}", HttpAccept.Json));
            }
        }
    }
}
