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
			<xsl:attribute name="style">
				<xsl:text>z-index:</xsl:text>
				<xsl:value-of select="1000-position()"/>
			</xsl:attribute>
			<div class="userbarcontainer" style="float:left">
				<xsl:apply-templates select="firstPost/post/poster/user" mode="userInfoBar"/>
			</div>
			<div class="threadicons" style="float:right">
				<xsl:if test="firstPost/post/layerName != 'normal'">
					<xsl:text> </xsl:text>
					<img width="16" height="16" border="0" style="vertical-align: text-bottom;">
						<xsl:attribute name="alt"><xsl:value-of select="firstPost/post/layer/name"/></xsl:attribute>
						<xsl:choose>
							<xsl:when test="totalNewPosts and totalNewPosts!='0'">
								<xsl:attribute name="src">/static/images/woocons/compat/<xsl:value-of select="firstPost/post/layerName"/>-notread.png</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="src">/static/images/woocons/compat/<xsl:value-of select="firstPost/post/layerName"/>-read.png</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
					</img>
				</xsl:if>
				<xsl:if test="isAnnouncement='true'">
					<xsl:text> </xsl:text>
					<img src="/static/images/woocons/NotePinned.png" width="16" height="16" alt="sticky" border="0" style="vertical-align: text-bottom;" />
				</xsl:if>
				<xsl:if test="isLocked='true'">
					<xsl:text> </xsl:text>
					<img src="/static/images/woocons/LockClosed.png" width="16" height="16" alt="locked" border="0" style="vertical-align:text-bottom"/>
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
			<div>
				<xsl:attribute name="style">
					<xsl:variable name="rawHotness">
						<xsl:call-template name="log2">
							<xsl:with-param name="number">
								<xsl:value-of select="totalPosts"/>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:variable>
					<xsl:variable name="hotness">
						<xsl:choose>
							<xsl:when test="$rawHotness &lt; 0">0</xsl:when>
							<xsl:when test="$rawHotness &gt; 12">12</xsl:when>
							<xsl:otherwise><xsl:value-of select="$rawHotness"/></xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:text>position:absolute;text-align:right;right:0px;bottom:0px;padding:2pt 5pt 0pt 5pt;</xsl:text>
					<xsl:text>background-color:rgb(</xsl:text>
					<xsl:value-of select="round($hotness*21.25)"/>
					<xsl:text>,0,</xsl:text>
					<xsl:value-of select="255-round($hotness*21.25)"/>
					<xsl:text>)</xsl:text>
				</xsl:attribute>
				<xsl:attribute name="onmouseover">showChildren(this);</xsl:attribute>
				<xsl:attribute name="onmouseout">hideChildren(this);</xsl:attribute>
				<div default="default" style="z-index:1;">
					<a class="pseudolink" style="color:black;">?</a>
				</div>
				<div class="threadcontainer_additionalcontainer" style="display:none;">
					<div class="threadcontainer_additional" style="right:-5pt;">
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
	</xsl:template>


</xsl:stylesheet>