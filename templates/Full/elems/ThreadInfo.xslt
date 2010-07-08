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
				<xsl:attribute name="title"><xsl:value-of select="bodyShort"/></xsl:attribute>
				<xsl:if test="isAnnouncement='true'">
					<img src="/static/images/sticky.gif" width="16" height="16" alt="" border="0" style="vertical-align: text-bottom;" />
					<xsl:text> </xsl:text>
				</xsl:if>
				<xsl:if test="isLocked='true'">
					<img src="/static/images/lock.gif" width="16" height="16" alt="" border="0" style="vertical-align:text-bottom"/>
					<xsl:text> </xsl:text>
				</xsl:if>
				<img alt="*" hspace="5" style="vertical-align: text-bottom">
					<xsl:choose>
						<xsl:when test="afterLastRead&lt;=lastPostId">
							<xsl:attribute name="src">/static/images/message-<xsl:value-of select="layerName"/>-notread.gif</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="src">/static/images/message-<xsl:value-of select="layerName"/>-read.gif</xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
				</img>
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
				<xsl:if test="pageOuter/isEmpty='false'">
					<span class="small" style="margin-left:1.5em">
						<xsl:apply-templates select="pageOuter" mode="withoutCurrent">
							<xsl:with-param name="baseLink">/Thread/<xsl:value-of select="id"/>/</xsl:with-param>
						</xsl:apply-templates>
					</span>
				</xsl:if>
			</td>
			<td align="left" nowrap="nowrap">
				<a>
					<xsl:attribute name="href">/User/<xsl:value-of select="topicstarter/user/id"/>/</xsl:attribute>
					<font>
						<xsl:attribute name="color">#0000ff</xsl:attribute>
						<xsl:value-of select="topicstarter/user/name"/>
					</font>
				</a>
			</td>
			<td align="center">
				<xsl:value-of select="totalViews"/>
			</td>
			<td align="center" nowrap="nowrap">
				<span class="separate"><xsl:value-of select="totalPosts"/></span>
				<xsl:if test="totalNewPosts and totalNewPosts!='0'">
					<a class="cup separate">
						<xsl:if test="/root/session/sessionKey">
							<xsl:attribute name="href">/do/MarkThreadAsRead/<xsl:value-of select="id"/>/p<xsl:value-of select="lastPostId"/>/</xsl:attribute>
						</xsl:if>
						<font class="new"><i>(<xsl:value-of select="totalNewPosts"/>)</i></font>
					</a>
				</xsl:if>
			</td>
			<td nowrap="nowrap" align="center">
				<xsl:choose>
					<xsl:when test="lastPostId &gt; 10000000">
						<xsl:apply-templates select="lastPostDate/date" mode="dateTime"/>
						<xsl:text> </xsl:text>
						<img alt="new" src="/static/images/new.gif">
							<xsl:if test="lastPostId &lt; 10000000">
								<xsl:attribute name="style">visibility:hidden</xsl:attribute>
							</xsl:if>
						</img>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>&#160;</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</td>
		</tr>
	</xsl:template>


</xsl:stylesheet>