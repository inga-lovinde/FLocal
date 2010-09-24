<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template match="poll">
		<tr>
			<xsl:choose>
				<xsl:when test="position() mod 2 = 0">
					<xsl:attribute name="class">lighttable</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="class">darktable</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<td align="left">
				<img alt="*" hspace="5" style="vertical-align: text-bottom">
					<xsl:attribute name="src">/static/images/message-normal-read.gif</xsl:attribute>
				</img>
				<xsl:text> </xsl:text>
				<a>
					<xsl:attribute name="href">
						<xsl:text>/Polls/Info/</xsl:text>
						<xsl:value-of select="id"/>
						<xsl:text>/</xsl:text>
					</xsl:attribute>
					<xsl:value-of select="title" disable-output-escaping="yes" />
				</a>
			</td>
			<td align="left" nowrap="nowrap">
				<xsl:apply-templates select="poster/user" mode="userLink"/>
			</td>
			<td nowrap="nowrap" align="center">
				<xsl:apply-templates select="postDate/date" mode="dateTime"/>
			</td>
		</tr>
	</xsl:template>


</xsl:stylesheet>