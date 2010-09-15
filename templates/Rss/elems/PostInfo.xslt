<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="post">
		<item>
			<title>
				<xsl:value-of select="title"/>
				<xsl:text> от </xsl:text>
				<xsl:value-of select="poster/user/name"/>
			</title>
			<description>
				<xsl:value-of select="bodyShort"/>
			</description>
			<link>
				<xsl:value-of select="$mainUrl"/>
				<xsl:text>/Post/</xsl:text>
				<xsl:value-of select="id"/>
				<xsl:text>/</xsl:text>
			</link>
			<guid>
				<xsl:text>post-</xsl:text>
				<xsl:value-of select="id"/>
			</guid>
			<pubDate>
				<xsl:apply-templates select="postDate/date" mode="dateTime"/>
			</pubDate>
		</item>
	</xsl:template>

</xsl:stylesheet>