<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template match="thread">
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
				<xsl:if test="isAnnouncement='true'">
					<xsl:text>{a}</xsl:text>
				</xsl:if>
				<xsl:if test="isLocked='true'">
					<xsl:text>{l}</xsl:text>
				</xsl:if>
				<xsl:if test="firstPost/post/layerName='offtop'">{o}</xsl:if>
				<xsl:if test="firstPost/post/layerName='garbage'">{g}</xsl:if>
				<xsl:choose>
					<xsl:when test="afterLastRead&lt;=lastPostId">
						<xsl:text>(*</xsl:text>
						<xsl:value-of select="totalNewPosts"/>
						<xsl:text>)</xsl:text>
					</xsl:when>
					<xsl:otherwise>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:text> </xsl:text>
				<a>
					<xsl:attribute name="href">
						<xsl:text>/Thread/</xsl:text>
						<xsl:value-of select="id"/>
						<xsl:text>/</xsl:text>
						<xsl:if test="afterLastRead&lt;=lastPostId">
							<xsl:text>p</xsl:text>
							<xsl:value-of select="afterLastRead"/>
						</xsl:if>
					</xsl:attribute>
					<xsl:value-of select="title"/>
				</a>
			</td>
			<td align="left" nowrap="nowrap">
				<xsl:apply-templates select="firstPost/post/poster/user" mode="userLink"/>
			</td>
		</tr>
	</xsl:template>


</xsl:stylesheet>