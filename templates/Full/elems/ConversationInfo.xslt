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
				<img alt="*" hspace="5" style="vertical-align: text-bottom">
					<xsl:choose>
						<xsl:when test="afterLastRead and afterLastRead&gt;lastMessageId">
							<xsl:attribute name="src">/static/images/message-normal-read.gif</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="src">/static/images/message-normal-notread.gif</xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
				</img>
				<a>
					<xsl:attribute name="href">
						<xsl:text>/Conversation/</xsl:text>
						<xsl:value-of select="interlocutor/account/id"/>
						<xsl:text>/</xsl:text>
						<xsl:if test="afterLastRead&lt;=lastPostId">
							<xsl:text>p</xsl:text>
							<xsl:value-of select="afterLastRead"/>
						</xsl:if>
					</xsl:attribute>
					<xsl:value-of select="interlocutor/account/user/name"/>
				</a>
				<span class="small" style="margin-left:1.5em">
					<xsl:apply-templates select="pageOuter" mode="withoutCurrent">
						<xsl:with-param name="baseLink">/Conversation/<xsl:value-of select="interlocutor/account/id"/>/</xsl:with-param>
					</xsl:apply-templates>
				</span>
			</td>
			<td align="center" nowrap="nowrap">
				<span class="separate"><xsl:value-of select="totalMessages"/></span>
				<xsl:if test="totalNewMessages and totalNewMessages!='0'">
					<a class="cup separate">
						<font class="new"><i>(<xsl:value-of select="totalNewMessages"/>)</i></font>
					</a>
				</xsl:if>
			</td>
			<td nowrap="nowrap" align="center">
				<xsl:apply-templates select="lastMessageDate/date" mode="dateTime"/>
			</td>
		</tr>
	</xsl:template>


</xsl:stylesheet>