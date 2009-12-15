using System;
using System.Collections.Generic;

using SQLite.NET;

using MediaPortal.GUI.Library;

namespace mpNZB
{
  class MPTVSeries
  {
    SQLiteClient sqlClient = new SQLiteClient(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Database) + @"\TVSeriesDatabase4.db3");

    public List<GUIListItem> SeriesNames()
    {     
      SQLiteResultSet sqlResults = sqlClient.Execute("SELECT Pretty_Name FROM online_series ORDER BY Pretty_Name");

      List<GUIListItem> _Results = new List<GUIListItem>();

      for (int i = 0; i < sqlResults.Rows.Count; i++)
      {
        _Results.Add(new GUIListItem(sqlResults.Rows[i].fields[0].ToString()));
      }

      return _Results;
    }

    public List<GUIListItem> MissingEpisodes(string Pretty_Name)
    {
      SQLiteResultSet sqlResults = sqlClient.Execute("SELECT CompositeID, SeasonIndex, EpisodeIndex, EpisodeName FROM online_episodes WHERE SeriesID=\"" + sqlClient.Execute("SELECT id FROM online_series WHERE Pretty_Name=\"" + Pretty_Name + "\"").Rows[0].fields[0].ToString() + "\" ORDER BY SeasonIndex, EpisodeIndex");

      List<GUIListItem> _Results = new List<GUIListItem>();
      GUIListItem _Item = new GUIListItem();

      SQLiteResultSet sqlEpisodes;

      for (int i = 0; i < sqlResults.Rows.Count; i++)
      {
        sqlEpisodes = sqlClient.Execute("SELECT CompositeID FROM local_episodes WHERE CompositeID=\"" + sqlResults.Rows[i].fields[0] + "\"");

        if (sqlEpisodes.Rows.Count == 0)
        {
          _Item = new GUIListItem();

          _Item.DVDLabel = "[S]" + sqlResults.Rows[i].fields[1].ToString().PadLeft(2, '0') + "[E]" + sqlResults.Rows[i].fields[2].ToString().PadLeft(2, '0');
          _Item.Label = _Item.DVDLabel.Replace("[S]0", String.Empty).Replace("[S]", String.Empty).Replace("[E]", "x");

          _Results.Add(_Item);
        }
      }

      return _Results;
    }

  }
}