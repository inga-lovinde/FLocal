<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template match="conversation">
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
				<xsl:choose>
					<xsl:when test="afterLastRead and afterLastRead&gt;lastMessageId">
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>(*</xsl:text>
						<xsl:value-of select="totalNewMessages"/>
						<xsl:text>)</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
				<a>
					<xsl:attribute name="href">
						<xsl:text>/Conversation/</xsl:text>
						<xsl:value-of select="interlocutor/account/id"/>
						<xsl:text>/</xsl:text>
						<xsl:if test="afterLastRead&lt;=lastMessageId">
							<xsl:text>p</xsl:text>
							<xsl:value-of select="afterLastRead"/>
						</xsl:if>
					</xsl:attribute>
					<xsl:value-of select="interlocutor/account/user/name"/>
				</a>
			</td>
		</tr>
	</xsl:template>


</xsl:stylesheet>