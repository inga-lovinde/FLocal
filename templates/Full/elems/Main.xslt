<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="Header.xslt"/>
	<xsl:output method="xml" indent="yes" encoding="UTF-8"/>
	<xsl:template match="/root">
		<html>
			<head>
				<link rel="stylesheet" href="/static/css/global.css" type="text/css" />
				<link rel="stylesheet" href="/static/css/coffeehaus.css" type="text/css" />
				<link rel="shortcut icon" href="/static/favicons/smirk.ico" type="image/x-icon" />
				<title><xsl:value-of select="title"/></title>
			</head>
			<body>
				<xsl:call-template name="header"/>
				<xsl:call-template name="specific"/>
				<br />
			</body>
		</html>
	</xsl:template>

	<xsl:template match="category">
		<table width="100%" align="center" class="tableborders" cellpadding="3" cellspacing="1">
			<tr>
				<td class="tdheader" colspan="2"><xsl:value-of select="name"/></td>
				<td class="tdheader" align="center">Темы</td>
				<td class="tdheader" align="center">Сообщений</td>
				<td class="tdheader" align="center">Последнее</td>
				<td class="tdheader" align="center">Модератор</td>
			</tr>
			<xsl:apply-templates select="boards/board"/>
		</table>
	</xsl:template>

	<xsl:template match="board">
		<tr>
			<td valign="top" width="1%" class="darktable">
				<a href="/ubbthreads.php?Cat=&amp;C=&amp;check=Common&amp;src=">
					<xsl:attribute name="onClick">if (!confirm('Пометить все сообщения как прочитанные?')) {event.returnValue=false; return false;}</xsl:attribute>
					<img border="0" width="17" height="21" src="/static/images/newposts.gif" alt=""/>
				</a>
			</td>
			<td width="55%" class="darktable">
				<font class="forumtitle">
					<a href="/postlist.php?Cat=&amp;Board=Common&amp;time=1275991384&amp;showlite="><xsl:value-of select="name"/></a>
				</font>
				<br />
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td class="forumdescript">
							<a href="/apostlist.php?Cat=&amp;Board=Common">A</a>&#160;&#160;
						</td>
						<td class="forumdescript"><xsl:value-of select="description"/></td>
					</tr>
				</table>
			</td>
			<td width="7%" align="center" class="threadtotal" nowrap="nowrap">373</td>
			<td width="7%" align="center" class="posttotal" nowrap="nowrap">128648</td>
			<td width="15%" nowrap="nowrap" class="posttime">
				08.06.2010 14:03<br />
				<a href="/showflat.php?Cat=&amp;Board=Common&amp;Number=9554129&amp;src=#Post9554129">от igor</a>
			</td>
			<td width="10%" class="modcolumn" align="center">
				&#160;<a href="/showprofile.php?User=Sash&amp;What=ubbthreads">Sash</a>,
				<a href="/showprofile.php?User=DeadmoroZ&amp;What=ubbthreads">DeadmoroZ</a>
			</td>
		</tr>
	</xsl:template>
</xsl:stylesheet>

<!--
        <h1>Persons</h1>
        <ul>
          <xsl:apply-templates select="person">
            <xsl:sort select="family-name" />
          </xsl:apply-templates>
        </ul>
      </body>
    </html>
  </xsl:template>
 
  <xsl:template match="person">
    <li>
      <xsl:value-of select="family-name"/><xsl:text>, </xsl:text>
      <xsl:value-of select="name"/>
    </li>
  </xsl:template>
 
-->