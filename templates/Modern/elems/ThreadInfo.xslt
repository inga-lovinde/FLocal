<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:import href="UserInfoBar.xslt"/>

	<xsl:template match="thread">
		<div>
			<xsl:attribute name="class">
				<xsl:text>threadcontainer</xsl:text>
				<xsl:if test="totalNewPosts and totalNewPosts!='0'">
					<xsl:text> updated</xsl:text>
				</xsl:if>
			</xsl:attribute>
			<div class="userbarcontainer" style="float:left">
				<xsl:apply-templates select="firstPost/post/poster/user" mode="userInfoBar"/>
			</div>
			<div class="threadicons" style="float:right">
				<xsl:if test="isAnnouncement='true'">
					<xsl:text> </xsl:text>
					<img src="/static/images/sticky.gif" width="16" height="16" alt="" border="0" style="vertical-align: text-bottom;" />
				</xsl:if>
				<xsl:if test="isLocked='true'">
					<xsl:text> </xsl:text>
					<img src="/static/images/lock.gif" width="16" height="16" alt="" border="0" style="vertical-align:text-bottom"/>
				</xsl:if>
				<xsl:comment>fill</xsl:comment>
			</div>
			<h4 class="threadcontainer_title">
				<xsl:attribute name="title"><xsl:value-of select="firstPost/post/bodyShort"/></xsl:attribute>
				<a>
					<xsl:attribute name="href">
						<xsl:text>/Thread/</xsl:text>
						<xsl:value-of select="id"/>
						<xsl:text>/</xsl:text>
						<xsl:if test="totalNewPosts and totalNewPosts!='0'">
							<xsl:text>p</xsl:text>
							<xsl:value-of select="afterLastRead"/>
						</xsl:if>
					</xsl:attribute>
					<xsl:value-of select="title"/>
				</a>
			</h4>
			<div style="position:absolute;text-align:right;right:0px;bottom:0px;background-color:inherit;padding:3pt 0pt 0pt 3pt;">
				<xsl:attribute name="onmouseover">showChildren(this);</xsl:attribute>
				<xsl:attribute name="onmouseout">hideChildren(this);</xsl:attribute>
				<div default="default" style="z-index:1;">
					<a class="pseudolink">Информация</a>
				</div>
				<div class="threadcontainer_additionalcontainer" style="display:none;">
					<div class="threadcontainer_additional" style="right:0px;">
						<p>
							<xsl:text>Сообщений: </xsl:text>
							<xsl:value-of select="totalPosts"/>
							<xsl:if test="totalNewPosts and totalNewPosts!='0'">
								<a>
									<xsl:if test="/root/session/sessionKey">
										<xsl:attribute name="href">/do/MarkThreadAsRead/<xsl:value-of select="id"/>/p<xsl:value-of select="lastPostId"/>/</xsl:attribute>
									</xsl:if>
									<font class="new"><i>(<xsl:value-of select="totalNewPosts"/>)</i></font>
								</a>
							</xsl:if>
						</p>
						<p>
							<xsl:text>Последнее: </xsl:text>
							<xsl:apply-templates select="lastPostDate/date" mode="dateTime"/>
						</p>
						<p>
							<xsl:text>Просмотров: </xsl:text>
							<xsl:value-of select="totalViews"/>
						</p>
						<xsl:if test="pageOuter/isEmpty='false'">
							<p>
								<xsl:text>Страницы: </xsl:text>
								<xsl:apply-templates select="pageOuter" mode="withoutCurrent">
									<xsl:with-param name="baseLink">/Thread/<xsl:value-of select="id"/>/</xsl:with-param>
								</xsl:apply-templates>
							</p>
						</xsl:if>
					</div>
				</div>
			</div>
		</div>
<!--
				<img alt="*" hspace="5" style="vertical-align: text-bottom">
					<xsl:choose>
						<xsl:when test="totalNewPosts and totalNewPosts!='0'">
							<xsl:attribute name="src">/static/images/message-<xsl:value-of select="firstPost/post/layerName"/>-notread.gif</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="src">/static/images/message-<xsl:value-of select="firstPost/post/layerName"/>-read.gif</xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
				</img>
				<xsl:text> </xsl:text>
				<xsl:if test="pageOuter/isEmpty='false'">
					<span class="small" style="margin-left:1.5em">
						<xsl:apply-templates select="pageOuter" mode="withoutCurrent">
							<xsl:with-param name="baseLink">/Thread/<xsl:value-of select="id"/>/</xsl:with-param>
						</xsl:apply-templates>
					</span>
				</xsl:if>
			</td>
			<td align="left" nowrap="nowrap">
				<xsl:apply-templates select="firstPost/post/poster/user" mode="userLink"/>
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
				<xsl:apply-templates select="lastPostDate/date" mode="dateTime"/>
			</td>
		</tr>
-->
	</xsl:template>


</xsl:stylesheet>