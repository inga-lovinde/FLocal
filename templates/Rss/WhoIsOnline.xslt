<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle">Кто в онлайне</xsl:template>
	<xsl:template name="specific">
		<xsl:apply-templates select="users/user"/>
	</xsl:template>

	<xsl:template match="users/user">
		<item>
			<title>
				<xsl:value-of select="name"/>
			</title>
			<description>
				<xsl:text>Посещает </xsl:text>
				<xsl:value-of select="lastUrl"/>
			</description>
			<link>
				<xsl:value-of select="$mainUrl"/>
				<xsl:text>/Users/Online/</xsl:text>
			</link>
			<guid>
				<xsl:text>online-</xsl:text>
				<xsl:value-of select="id"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="lastActivity/date/ticks"/>
			</guid>
			<pubDate>
				<xsl:apply-templates select="lastActivity/date" mode="dateTime"/>
			</pubDate>
		</item>
	</xsl:template>

</xsl:stylesheet>