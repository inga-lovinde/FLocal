<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" indent="no" />

	<xsl:variable name="mainUrl">
		<xsl:value-of select="/root/url/scheme"/>
		<xsl:text>://</xsl:text>
		<xsl:value-of select="/root/url/host"/>
		<xsl:text>:</xsl:text>
		<xsl:if test="/root/url/port &gt;= 1000">
			<xsl:value-of select="floor(/root/url/port div 1000)"/>
		</xsl:if>
		<xsl:text>443</xsl:text>
	</xsl:variable>

	<xsl:template match="/root">
		<rss version="2.0">
			<channel>
				<title>
					<xsl:call-template name="specificTitle"/>
				</title>
				<description>
					<xsl:call-template name="specificTitle"/>
				</description>
				<link>
					<xsl:value-of select="$mainUrl"/>
					<xsl:value-of select="currentUrl"/>
				</link>
				<lastBuildDate>
					<xsl:apply-templates select="current" mode="dateTime"/>
				</lastBuildDate>
				<pubDate>
					<xsl:apply-templates select="current" mode="dateTime"/>
				</pubDate>

				<xsl:call-template name="specific"/>

			</channel>
		</rss>
	</xsl:template>

	<xsl:template match="date" mode="dateTime">
		<xsl:value-of select="rss"/>
	</xsl:template>

</xsl:stylesheet>

