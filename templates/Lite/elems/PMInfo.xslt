<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template match="message">
		<div>
			<div>
				<xsl:choose>
					<xsl:when test="isReadByInterlocutor='false'">
						<xsl:attribute name="class">warning</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="class">lighttable</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
				<a><xsl:attribute name="name">Message<xsl:value-of select="id"/></xsl:attribute></a>
				<xsl:apply-templates select="poster/account/user" mode="userLink"/>
				<xsl:text> : </xsl:text>
				<xsl:choose>
					<xsl:when test="isRead='false'">
						<xsl:text>(*)</xsl:text>
					</xsl:when>
					<xsl:otherwise>
					</xsl:otherwise>
				</xsl:choose>
				<b><xsl:value-of select="title"/></b>
				<font class="small" style="padding-left:2em"><xsl:apply-templates select="postDate/date" mode="dateTime"/></font>
				<span style="padding-left:1em">
					<xsl:text> | </xsl:text>
					<a>
						<xsl:attribute name="href">/My/Conversations/PMReply/<xsl:value-of select="id"/>/</xsl:attribute>
						<xsl:text>Reply</xsl:text>
					</a>
					<xsl:text> | </xsl:text>
				</span>
			</div>
			<div class="darktable post">
				<xsl:value-of select="body" disable-output-escaping="yes" />
			</div>
		</div>
	</xsl:template>

</xsl:stylesheet>
