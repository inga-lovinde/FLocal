<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="ForumHeader.xslt"/>
	<xsl:import href="MyHeader.xslt"/>
	<xsl:import href="PollsHeader.xslt"/>
	<xsl:import href="UsersHeader.xslt"/>
	<xsl:import href="UploadHeader.xslt"/>

	<xsl:template name="isClassicEnabled">true</xsl:template>
	<xsl:template name="isLiteEnabled">false</xsl:template>
	<xsl:template name="isRssEnabled">false</xsl:template>
	<xsl:template name="rssRelativeLink"><xsl:value-of select="currentBaseUrl"/></xsl:template>

	<xsl:template name="headerLink">
		<xsl:param name="url"/>
		<xsl:param name="text"/>
		<xsl:param name="isDisabled"/>
		<xsl:param name="postfix"/>
		<xsl:param name="skipDatePostfix"/>
		<a target="_top">
			<xsl:attribute name="class">
				<xsl:text>headerLink</xsl:text>
				<xsl:choose>
					<xsl:when test="$isDisabled='true'">
						<xsl:text> disabled</xsl:text>
					</xsl:when>
					<xsl:when test="starts-with(/root/currentUrl, $url)">
						<xsl:text> selected</xsl:text>
					</xsl:when>
				</xsl:choose>
			</xsl:attribute>
			<xsl:if test="not($isDisabled='true')">
				<xsl:attribute name="href">
					<xsl:value-of select="$url"/>
					<xsl:value-of select="$postfix"/>
					<xsl:if test="not($skipDatePostfix='true')">?<xsl:value-of select="current/date/ticks"/></xsl:if>
				</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="$text"/>
		</a>
	</xsl:template>

	<xsl:template name="header">
		<div class="designslinks" style="float:right;">
			<xsl:variable name="isClassicEnabled">
				<xsl:call-template name="isClassicEnabled"/>
			</xsl:variable>
			<xsl:variable name="isLiteEnabled">
				<xsl:call-template name="isLiteEnabled"/>
			</xsl:variable>
			<xsl:variable name="isRssEnabled">
				<xsl:call-template name="isRssEnabled"/>
			</xsl:variable>
			<xsl:variable name="rssRelativeLink">
				<xsl:call-template name="rssRelativeLink"/>
			</xsl:variable>
			<xsl:variable name="postfix">
				<xsl:text>.forum.hn</xsl:text>
				<xsl:text>:</xsl:text>
				<xsl:value-of select="url/port"/>
			</xsl:variable>
			<xsl:if test="not(starts-with(url/host, 'public.'))">
				<a>
					<xsl:if test="$isClassicEnabled='true'">
						<xsl:attribute name="href">
							<xsl:value-of select="url/scheme"/>
							<xsl:text>://classic</xsl:text>
							<xsl:value-of select="$postfix"/>
							<xsl:value-of select="currentUrl"/>
						</xsl:attribute>
					</xsl:if>
					<img border="0" alt="Switch to classic mode">
						<xsl:attribute name="src">
							<xsl:text>/static/images/classic.jpg</xsl:text>
						</xsl:attribute>
					</img>
				</a>
			</xsl:if>
			<xsl:text>&#160;&#160;</xsl:text>
			<a>
				<xsl:if test="$isLiteEnabled='true'">
					<xsl:attribute name="href">
						<xsl:value-of select="url/scheme"/>
						<xsl:text>://lite</xsl:text>
						<xsl:value-of select="$postfix"/>
						<xsl:value-of select="currentUrl"/>
					</xsl:attribute>
				</xsl:if>
				<img border="0" alt="Switch to lite mode">
					<xsl:attribute name="src">
						<xsl:text>/static/images/light</xsl:text>
						<xsl:if test="not($isLiteEnabled='true')">
							<xsl:text>-disabled</xsl:text>
						</xsl:if>
						<xsl:text>.png</xsl:text>
					</xsl:attribute>
				</img>
			</a>
			<xsl:text>&#160;&#160;</xsl:text>
			<a>
				<xsl:if test="$isRssEnabled='true'">
					<xsl:attribute name="href">
						<xsl:value-of select="url/scheme"/>
						<xsl:text>://rss</xsl:text>
						<xsl:value-of select="$postfix"/>
						<xsl:value-of select="$rssRelativeLink"/>
					</xsl:attribute>
				</xsl:if>
				<img border="0" alt="Switch to RSS mode">
					<xsl:attribute name="src">
						<xsl:text>/static/images/rss</xsl:text>
						<xsl:if test="not($isRssEnabled='true')">
							<xsl:text>-disabled</xsl:text>
						</xsl:if>
						<xsl:text>.png</xsl:text>
					</xsl:attribute>
				</img>
			</a>
			<xsl:if test="$isRssEnabled='true'">
				<link rel="alternate" type="application/rss+xml" title="RSS">
					<xsl:attribute name="href">
						<xsl:value-of select="url/scheme"/>
						<xsl:text>://rss</xsl:text>
						<xsl:value-of select="$postfix"/>
						<xsl:value-of select="$rssRelativeLink"/>
					</xsl:attribute>
				</link>
			</xsl:if>
		</div>
		<div class="header">
			<xsl:if test="session/sessionKey and (session/indicators/unreadPrivateMessages != '0')">
				<a href="/My/Conversations/">
					<img src="/static/images/newpm.gif" border="0">
						<xsl:attribute name="alt">
							<xsl:text>� ��� </xsl:text>
							<xsl:value-of select="session/indicators/unreadPrivateMessages"/>
							<xsl:text> ������������� ������ ���������</xsl:text>
						</xsl:attribute>
					</img>
				</a>
				<xsl:text>&#160;&#160;&#160;</xsl:text>
			</xsl:if>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/Forum/</xsl:with-param>
				<xsl:with-param name="text">�����</xsl:with-param>
			</xsl:call-template>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/Polls/</xsl:with-param>
				<xsl:with-param name="text">������</xsl:with-param>
			</xsl:call-template>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/Upload/</xsl:with-param>
				<xsl:with-param name="text">������</xsl:with-param>
				<xsl:with-param name="isDisabled">
					<xsl:if test="not(session/sessionKey)">true</xsl:if>
				</xsl:with-param>
			</xsl:call-template>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/My/</xsl:with-param>
				<xsl:with-param name="text">
					<xsl:choose>
					<xsl:when test="session/sessionKey"><xsl:value-of select="session/user/name"/></xsl:when>
					<xsl:otherwise>����</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
			</xsl:call-template>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/q/faq/</xsl:with-param>
				<xsl:with-param name="text">FAQ</xsl:with-param>
			</xsl:call-template>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/Users/</xsl:with-param>
				<xsl:with-param name="text">������������</xsl:with-param>
			</xsl:call-template>
			<xsl:if test="starts-with(/root/currentUrl, '/Forum/')">
				<xsl:call-template name="forumHeader"/>
			</xsl:if>
			<xsl:if test="starts-with(/root/currentUrl, '/Users/')">
				<xsl:call-template name="usersHeader"/>
			</xsl:if>
			<xsl:if test="starts-with(/root/currentUrl, '/My/')">
				<xsl:call-template name="myHeader"/>
			</xsl:if>
			<xsl:if test="starts-with(/root/currentUrl, '/Upload/')">
				<xsl:call-template name="uploadHeader"/>
			</xsl:if>
			<xsl:if test="starts-with(/root/currentUrl, '/Polls/')">
				<xsl:call-template name="pollsHeader"/>
			</xsl:if>
		</div>
	</xsl:template>
</xsl:stylesheet>

