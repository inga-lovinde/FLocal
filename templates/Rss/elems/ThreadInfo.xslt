<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="thread">
		<item>
			<title>
				<xsl:text>Новое сообщение в обсуждении </xsl:text>
				<xsl:value-of select="title"/>
			</title>
			<description>
				<xsl:text>Всего в обсуждении </xsl:text>
				<xsl:value-of select="totalPosts"/>
				<xsl:text> сообщений</xsl:text>
				<xsl:if test="totalNewPosts &gt; 0">
					<xsl:text>, из них </xsl:text>
					<xsl:value-of select="totalNewPosts"/>
					<xsl:text> непрочитанных</xsl:text>
				</xsl:if>
			</description>
			<link>
				<xsl:value-of select="$mainUrl"/>
				<xsl:text>/Thread/</xsl:text>
				<xsl:value-of select="id"/>
				<xsl:text>/</xsl:text>
				<xsl:if test="totalNewPosts and totalNewPosts!='0'">
					<xsl:text>p</xsl:text>
					<xsl:value-of select="afterLastRead"/>
				</xsl:if>
			</link>
			<guid>
				<xsl:text>thread-</xsl:text>
				<xsl:value-of select="id"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="lastPostId"/>
			</guid>
			<pubDate>
				<xsl:apply-templates select="lastPostDate/date" mode="dateTime"/>
			</pubDate>
		</item>
	</xsl:template>

</xsl:stylesheet>