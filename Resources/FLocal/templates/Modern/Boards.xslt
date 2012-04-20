<?xml version="1.0" encoding="ASCII"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\BoardInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:call-template name="Messages_Boards"/>
	</xsl:template>
	<xsl:template name="isLiteEnabled">true</xsl:template>
	<xsl:template name="specific">
		<div class="categoriescontainer">
			<xsl:apply-templates select="categories/category"/>
		</div>
		<p>
			<xsl:choose>
				<xsl:when test="session/user">
					<xsl:call-template name="Messages_YouAreXXX">
						<xsl:with-param name="userLink">
							<xsl:apply-templates select="session/user" mode="userLink"/>
						</xsl:with-param>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="Messages_YouAreAnonymous"/>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:text>. </xsl:text>
			<xsl:call-template name="Messages_TotalRegisteredUsers">
				<xsl:with-param name="usersNumber">
					<xsl:value-of select="totalRegistered"/>
				</xsl:with-param>
			</xsl:call-template>
			<xsl:text>. </xsl:text>
			<xsl:call-template name="Messages_TotalOnlineUsers">
				<xsl:with-param name="threshold">
					<xsl:value-of select="activity/threshold"/>
				</xsl:with-param>
				<xsl:with-param name="sessionsNumber">
					<xsl:value-of select="activity/sessions"/>
				</xsl:with-param>
			</xsl:call-template>
			<xsl:text>. </xsl:text>
		</p>
		<p>
			<a>
				<xsl:attribute name="href">/AllPosts/</xsl:attribute>
				<xsl:call-template name="Messages_LastPosts"/>
			</a>
			<xsl:text>. </xsl:text>
			<a>
				<xsl:attribute name="href">/AllThreads/</xsl:attribute>
				<xsl:call-template name="Messages_LastThreads"/>
			</a>
			<xsl:text>. </xsl:text>
			<a>
				<xsl:if test="session/user">
					<xsl:attribute name="href">/Users/User/<xsl:value-of select="session/user/id"/>/Mentions/</xsl:attribute>
				</xsl:if>
				<xsl:call-template name="Messages_LastReplies"/>
			</a>
			<xsl:text>. </xsl:text>
			<a>
				<xsl:if test="session/user">
					<xsl:attribute name="href">/Users/User/<xsl:value-of select="session/user/id"/>/PollsParticipated/</xsl:attribute>
				</xsl:if>
				<xsl:call-template name="Messages_LastPollsParticipated"/>
			</a>
			<xsl:text>. </xsl:text>
		</p>
	</xsl:template>

	<xsl:template match="category">
		<div class="categorycontainer">
			<h2 class="categorycontainer_title"><xsl:value-of select="name"/></h2>
			<div class="boardscontainer">
				<xsl:apply-templates select="boards/board">
					<xsl:with-param name="maxPerLine">
						<xsl:value-of select="ceiling(count(boards/board) div 2)"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</div>
		</div>
	</xsl:template>

</xsl:stylesheet>