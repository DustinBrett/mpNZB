# 1.4 #
**Additions**
  * Suggestions have been added to search
    * Blockbuster, Netflix, Rotten Tomatoes, Yahoo! Movies, Zip.ca

**Fixes**
  * Saving to a blank category if you exited during category selection

# 1.3 #
**Additions**
  * Missing Episodes now search 3 ways (`S01E01, 1x01, 101`)
  * NFO viewer for Newzbin results

**Modifications**
  * Newzbin authentication using `HTTP Auth` instead of cookies (Faster)
  * Recoded `Download Complete` (Faster / Reliable)
  * Items in queue will say "Paused" when paused

# 1.2 #
**Additions**
  * Pause/Resume individual download
  * Black Glass skin inside MPI

**Fixes**
  * Download Complete uses History instead of Queue (More accurate/responsive)
  * Search string was not displayed for Missing Episodes

**Modifications**
  * Updated Queue information
  * Updated History information
  * Missing Episodes just searches (`Series Name [1x01|S01E01]`)
  * Toggle `btnPause` if queue is paused on startup
  * Status updates only stop on plugin exit

# 1.1 #
**Additions**
  * SABnzbd 0.5+ Support
  * API Key support
  * Custom `<searches>` instead of a single `<search>`
  * Version displayed on startup in status bar

**Fixes**
  * References to MyTVSeries have been changed to MP-TVSeries
  * Empty Max Results caused error
  * Removed sites in list when required login information was missing
  * NZBClub size data was not showing

**Modifications**
  * Event types in History view
  * Queue commands updated to new API
  * Changed default for `Download NZB?` to `Yes`
  * Missing episodes now searches using `S01E01` & `1x01`

# 1.07 #
**Fixes**
  * Remove sorting if not download item

**Additions**
  * Added more feeds to NZBs in Sites\_Custom
  * SABnzbd History View
  * Subtitles added to Newzbin information
  * MyTvNZB has same information as TvNZB now

# 1.06 #
**Fixes**
  * pubDate is now `TryParseExact` instead of `ParseExact`
  * NZBs and NZBsRus were wrong in Sites\_Custom
  * Removed `RegEx` from size type 1 (Using `"(\d+[,|.]\d+)\s?(\w{2,4})"` for all sites)
  * Menu only showed up with 2 items (Now shows with just 1)

**Additions**
  * NZBClub Groups
  * NZBMatrix Feeds
  * NZBSerien Feed
  * Generic login support using Sites.xml (Newzbin only currently)
  * NZB Information for Newzbin & TvNZB
  * Season/Episode search string formatting for MyTVSeries

# 1.05 #
**Fixes**
  * Removed unnecessary SABnzbd code
  * Blank searches not allowed
  * Error checking if Status Update Interval = 0
  * Fixed status "Idle" message on start

**Additions**
  * Download notification
  * Skins
    * `Black & White 1080`
    * `BleazleWide`
    * `Blue3`
    * `Indigo`
    * `MediaStream`
    * `Monochrome`
    * `PureVisionHD`