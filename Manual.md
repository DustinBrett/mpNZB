<h1>Table of Contents</h1>



# Introduction #

mpNZB was created to allow downloading of NZB's from within [MediaPortal](http://www.team-mediaportal.com/). It grabs lists of NZB's 3 different ways. Feeds that were created by the sites, Searching the site for specific items or Groups such as alt.binaries.multimedia. It gets this data from RSS feeds that the sites provide.

# Supported #

## Sites ##

Currently mpNZB supports 11 different sites and can support most sites with RSS feeds.

  * [Binsearch](http://binsearch.info/) (Groups)
  * [MyTvNZB](http://mytvnzb.foechoer.be/) ([Feeds](http://code.google.com/p/mpnzb/wiki/Sites_Custom#MyTvNZB))
  * [Newzbin](http://www.newzbin.com/)`**` (Feeds/Groups/Search)
  * [Newzleech](http://www.newzleech.com/) (Groups/Search)
  * [NZBClub](http://www.nzbclub.com/) (Groups/Search)
  * [NZBIndex](http://www.nzbindex.com/) (Groups/Search)
  * [NZBMatrix](http://nzbmatrix.com/)`**` ([Feeds](http://code.google.com/p/mpnzb/wiki/Sites_Custom#NZBMatrix))
  * [NZBs](http://www.nzbs.org/)`*` ([Feeds/Search](http://code.google.com/p/mpnzb/wiki/Sites_Custom#NZBs))
  * [NZBSerien](http://nzbserien.org/) (Feeds)
  * [NZBsRus](http://www.nzbsrus.com/)`*` ([Feeds](http://code.google.com/p/mpnzb/wiki/Sites_Custom#NZBsRus))
  * [TvNZB](http://www.tvnzb.com/) (Feeds)

`*` <sub>Requires account with site.</sub><br>
<code>**</code> <sub>Requires paid account with site.</sub><br>
<br>
<h2>Skins</h2>

<ul><li>Black & White 1080<br>
</li><li>Black Glass<br>
</li><li>BleazleWide<br>
</li><li>Blue3<br>
</li><li>Blue3wide<br>
</li><li>Indigo<br>
</li><li>MediaStream<br>
</li><li>Monochrome<br>
</li><li>PureVisionHD<br>
</li><li>StreamedMP</li></ul>

<h1>Updates</h1>

<h2>Changelog</h2>

There is a detailed wiki at ChangeLog.<br>
<br>
<h2>To-Do</h2>

There is a detailed wiki at ToDo.<br>
<br>
<h2>Forum</h2>

There is a forum topic started <a href='http://forum.team-mediaportal.com/plugins-47/mpnzb-nzb-downloader-57265/'>here</a>.<br>
<br>
<h2>Issues</h2>

All issues should be posted <a href='http://code.google.com/p/mpnzb/issues/list'>here</a>.<br>
<br>
<h1>Installation</h1>

<h2>Requirements</h2>

<ul><li><a href='http://www.team-mediaportal.com/'>MediaPortal</a> (>= 1.0)<br>
</li><li><a href='http://www.sabnzbd.org/'>SABnzbd</a> (<a href='http://sourceforge.net/project/showfiles.php?group_id=207766&package_id=248837&release_id=671171'>0.4.8</a> or higher)</li></ul>

<h2>Instructions</h2>

Download the latest release from <a href='http://code.google.com/p/mpnzb/downloads/list'>here</a>. Then run the MPI file and follow the instructions.<br>
<br>
<h1>Configuration</h1>

<h2>Plugin</h2>

Right click mpNZB from the <a href='http://www.team-mediaportal.com/'>MediaPortal</a> plugin configuration area and click <b>Configuration</b>. When it first pops up it should look like this:<br>
<br>
<a href='http://mpnzb.googlecode.com/svn/wiki/images/manual/mpNZB_Client_Settings.PNG'>http://mpnzb.googlecode.com/svn/wiki/images/manual/mpNZB_Client_Settings.PNG</a>

<ul><li><i>Program</i>
<ul><li><b>Grabber</b> - This setting is mostly for future use as it currently only supports <a href='http://www.sabnzbd.org/'>SABnzbd</a>. This may never change because <a href='http://www.sabnzbd.org/'>SABnzbd</a> is so much better than other Usenet clients when it comes to integrating with other programs.<br>
</li><li><b>Save to categories</b> - Checking this box will show a pop-up every time you go to download a NZB. It asks which <a href='http://www.sabnzbd.org/'>SABnzbd</a> category you want to download it to. These categories are setup within <a href='http://www.sabnzbd.org/'>SABnzbd</a>.</li></ul></li></ul>

<ul><li><i>Settings</i>
<ul><li><b>Address</b> - IP/Hostname of the <a href='http://www.sabnzbd.org/'>SABnzbd</a> server.<br>
</li><li><b>Port</b> - Port number of the <a href='http://www.sabnzbd.org/'>SABnzbd</a> server.<br>
</li><li><b>Test Connection</b> - Tests if the plugin will be able to connect using the given information. This includes the Username/Password also if they are required.<br>
</li><li><b>Requires Authentication</b> - Check this box if your <a href='http://www.sabnzbd.org/'>SABnzbd</a> server has a Username/Password set.<br>
</li><li><b>Username</b> - Username of the <a href='http://www.sabnzbd.org/'>SABnzbd</a> server.<br>
</li><li><b>Password</b> - Password of the <a href='http://www.sabnzbd.org/'>SABnzbd</a> server.</li></ul></li></ul>

<a href='http://mpnzb.googlecode.com/svn/wiki/images/manual/mpNZB_Plugin_Settings.PNG'>http://mpnzb.googlecode.com/svn/wiki/images/manual/mpNZB_Plugin_Settings.PNG</a>

<ul><li><i>Visual</i>
<ul><li><b>Update Frequency</b> - How often the status area (Paused/Speed/etc.) in the plug-in is refreshed.<br>
<ul><li>Set this higher than 0.<br>
</li></ul></li><li><b>Display Name</b> - Allows you to change what name is shown for this plug-in on the <a href='http://www.team-mediaportal.com/'>MediaPortal</a> GUI.<br>
</li><li><b>Show "Download Complete" notifications.</b> - Will pop-up a "Download Complete" dialog box whenever an item leaves the <a href='http://www.sabnzbd.org/'>SABnzbd</a> queue.</li></ul></li></ul>

<ul><li><i>Feeds</i>
<ul><li><b>Max Results (per feed)</b> - Sets the maximum possible results that will be returned from an RSS feed that allows setting maximum values.<br>
<ul><li>The results may be less than this number if the RSS feed has a maximum number of results lower than this number.<br>
</li><li>Set this higher than 0.<br>
</li></ul></li><li><b>Add <a href='http://code.google.com/p/mptvseries/'>MP-TVSeries</a> missing episodes to search</b> - Checking this box will add an extra option called "Missing Episodes" to the search dialog when you select a site to search. This option contains a list of all episodes that you are missing from your MP-TVSeries shows database.<br>
<ul><li>This feature will only work if you have <a href='http://code.google.com/p/mptvseries/'>MP-TVSeries</a> plug-in and checked the "Download Episode information for the whole series" checkbox in <a href='http://code.google.com/p/mptvseries/'>MP-TVSeries</a> configuration.</li></ul></li></ul></li></ul>

<ul><li><i><a href='http://code.google.com/p/mptvseries/'>MP-TVSeries</a> Formating</i> - If <b>Add <a href='http://code.google.com/p/mptvseries/'>MP-TVSeries</a> missing episodes to search</b> is checked then this decides how to format the season/episode search string.<br>
<ul><li><b><code>[S]x[EE]</code></b> - 1x01<br>
</li><li><b><code>S[SS]E[EE]</code></b> - S01E01</li></ul></li></ul>

<a href='http://mpnzb.googlecode.com/svn/wiki/images/manual/mpNZB_Searches.PNG'>http://mpnzb.googlecode.com/svn/wiki/images/manual/mpNZB_Searches.PNG</a>

<ul><li><i>Add Search</i> - This section is if you want to search multiple items at once on a site.<br>
<ul><li><b>Name</b> - The name for the search string that will appear in the pop-up search dialog inside the plugin.<br>
</li><li><b>String</b> - Here you put the things you want to search for separated by an "|" symbol.<br>
<ul><li><b>Example</b> - <code>Smallville|Buffy the Vampire Slayer|Lost</code>
</li></ul></li><li><b>Add</b> - Once you have filled out Name/String you press this to add it to the list.<br>
</li><li><b>Delete</b> - You select an item from the list and press this to delete that item.</li></ul></li></ul>

<a href='http://mpnzb.googlecode.com/svn/wiki/images/manual/mpNZB_Groups.PNG'>http://mpnzb.googlecode.com/svn/wiki/images/manual/mpNZB_Groups.PNG</a>

<ul><li><i>Add Group</i> - Here you define the groups that can be searched in the Groups section of the plugin.<br>
<ul><li><b>Group</b> - The name of the Usenet group.<br>
<ul><li><b>Example</b> - <code>alt.binaries.multimedia</code>
</li></ul></li><li><b>Add</b> - Once you have filled out Group you press this to add it to the list.<br>
</li><li><b>Delete</b> - You select an item from the list and press this to delete that item.</li></ul></li></ul>

<h2>Sites</h2>

There is a detailed wiki at Sites_HowTo and examples at Sites_Custom.<br>
<br>
<h1>Navigation</h1>

This is what mpNZB looks like using the Blue3 skin:<br>
<br>
<a href='http://mpnzb.googlecode.com/svn/wiki/images/manual/Nav_Main.PNG'>http://mpnzb.googlecode.com/svn/wiki/images/manual/Nav_Main.PNG</a>

<h2>Title</h2>

When you first start the program this will say the name of the plugin. Once you have clicked on any of the Feeds/Groups/Search it will change to show what type of data you requested.<br>
<br>
<h2>Main Buttons</h2>

<ul><li><b>Refresh</b> - Refreshes the currently selected feed.<br>
<ul><li>Disabled if no feed has been selected yet.<br>
</li></ul></li><li><b>Feeds</b> - Shows a dialog list of feeds based on the site you select.<br>
</li><li><b>Groups</b> - Shows a dialog list of groups based on the site you select.<br>
<ul><li>List is generated from the groups you enter in the Groups tab in the mpNZB configuration.<br>
</li></ul></li><li><b>Search</b> - Shows a dialog list of the following items to search on the site you select.<br>
<ul><li><b>New Search</b> - Manually typed keyboard search.<br>
</li><li><b>Missing Episodes</b> - <a href='http://code.google.com/p/mptvseries/'>MP-TVSeries</a> database of missing episodes.<br>
</li><li>Also lists any custom searches created in the Searches tab in the mpNZB configuration.<br>
</li></ul></li><li><b>Job Queue</b> - Shows a list of items in the <a href='http://www.sabnzbd.org/'>SABnzbd</a> queue.<br>
</li><li><b>History</b> - Shows a list of items in the <a href='http://www.sabnzbd.org/'>SABnzbd</a> history.<br>
</li><li><b>Pause Queue</b> - Pauses the <a href='http://www.sabnzbd.org/'>SABnzbd</a> queue.</li></ul>

<h2>Listview</h2>

This list changes depending on the section you are in. The formatting for each list item is also different depending on the section you are in.<br>
<br>
<ul><li><b>Feeds/Groups/Search</b> - {Title} {Size (MB)}<br>
</li><li><b>Job Queue</b> - {Title} {Remaining (MB) / Total (MB)}<br>
</li><li><b>History</b> - {Title} {Status}<br>
<ul><li><b>Status</b> - There are several different types of status messages depending on what SABnzbd did or is doing to the item.<br>
<ul><li><b>Completed</b>
</li><li><b>Extracting</b>
</li><li><b>Failed</b>
</li><li><b>Verifying</b>
</li><li><b>Waiting</b></li></ul></li></ul></li></ul>

Clicking a list item has different effects depending on the section you are in.<br>
<br>
<ul><li><b>Feeds/Groups/Search</b> - Asks if you want to download the NZB.<br>
</li><li><b>Job Queue</b> - Several different options related to the queue item.<br>
<ul><li><b>Move Up</b>
</li><li><b>Move to Top</b>
</li><li><b>Move Down</b>
</li><li><b>Move to Bottom</b>
</li><li><b>Delete Job</b>
</li><li><b>Change Category</b>
</li></ul></li><li><b>History</b> - Shows the entire <a href='http://www.sabnzbd.org/'>SABnzbd</a> history for that item.</li></ul>

<h2>Menu/Info Button</h2>

This refers to the menu/info button on your remote/keyboard in <a href='http://www.team-mediaportal.com/'>MediaPortal</a>. The options change depending on the type of list item you have selected.<br>
<br>
<ul><li><b>Feeds/Groups/Search</b>
<ul><li><b>Sort By</b> - Sorts the items currently in the listview based on the options you select. The options are Name/Date/Size in Ascending or Descending order.<br>
</li><li><b>Information</b> - If the site you are listing has extra information in its RSS feeds then this option will appear. For sites such as <a href='http://www.newzbin.com/'>Newzbin</a> it will list Source, Video Format, Video Genre, Language and Subtitle Language.<br>
</li></ul></li><li><b>Job Queue</b>
<ul><li><b>Information</b> - Lists information related to the <a href='http://www.sabnzbd.org/'>SABnzbd</a> job.</li></ul></li></ul>

<h2>Status Bar</h2>

This status bar message changes all the time depending on what your doing. If there is an error with the program it will mention it here. It also mentions any actions you take and how <a href='http://www.sabnzbd.org/'>SABnzbd</a> responded to them.