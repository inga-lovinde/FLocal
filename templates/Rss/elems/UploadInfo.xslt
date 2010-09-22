<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="upload">
		<xsl:if test="not(uploader/user/name = /root/session/user/name)">
			<item>
				<title>
					<xsl:value-of select="filename"/>
					<xsl:text> от </xsl:text>
					<xsl:value-of select="uploader/user/name"/>
				</title>
				<description>
					<xsl:text><![CDATA[<img src="]]></xsl:text>
					<xsl:value-of select="$mainUrl"/>
					<xsl:text>/Upload/Item/</xsl:text>
					<xsl:value-of select="id"/>
					<xsl:text><![CDATA[" border="0" style="max-width:200px;max-height:150px;"/>]]></xsl:text>
				</description>
				<link>
					<xsl:value-of select="$mainUrl"/>
					<xsl:text>/Upload/Info/</xsl:text>
					<xsl:value-of select="id"/>
					<xsl:text>/</xsl:text>
				</link>
				<guid>
					<xsl:text>upload-</xsl:text>
					<xsl:value-of select="id"/>
				</guid>
				<pubDate>
					<xsl:apply-templates select="uploadDate/date" mode="dateTime"/>
				</pubDate>
			</item>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>
