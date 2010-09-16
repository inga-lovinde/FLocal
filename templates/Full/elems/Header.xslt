<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="MyHeader.xslt"/>
	<xsl:import href="UsersHeader.xslt"/>
	<xsl:import href="UploadHeader.xslt"/>

	<xsl:template name="isLiteEnabled">false</xsl:template>
	<xsl:template name="isRssEnabled">false</xsl:template>
	<xsl:template name="rssRelativeLink"><xsl:value-of select="currentUrl"/></xsl:template>

	<xsl:template name="headerLink">
		<xsl:param name="url"/>
		<xsl:param name="text"/>
		<xsl:param name="isDisabled"/>
		<a target="_top">
			<xsl:if test="not($isDisabled='true')">
				<xsl:attribute name="href"><xsl:value-of select="$url"/>?<xsl:value-of select="current/date/ticks"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="starts-with(/root/currentUrl, $url)">
				<xsl:attribute name="class">currentLink</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="$text"/>
		</a>
	</xsl:template>

	<xsl:template name="header">
		<table width="95%" align="center" class="tablesurround" cellpadding="1" cellspacing="1">
			<tr>
				<td>
					<table width="100%" class="tableborders" cellpadding="3" cellspacing="1">
						<tr>
							<td align="center" class="menubar">
								<table width="100%" border="0">
									<tr>
										<td width="15%">&#160;</td>
										<td width="70%" align="center">
											<xsl:if test="session/sessionKey and (session/indicators/unreadPrivateMessages != '0')">
												<a href="/My/Conversations/">
													<img src="/static/images/newpm.gif" border="0">
														<xsl:attribute name="alt">
															<xsl:text>У вас </xsl:text>
															<xsl:value-of select="session/indicators/unreadPrivateMessages"/>
															<xsl:text> непрочитанных личных сообщений</xsl:text>
														</xsl:attribute>
													</img>
												</a>
												<xsl:text> | </xsl:text>
											</xsl:if>
											<xsl:call-template name="headerLink">
												<xsl:with-param name="url">/Boards/</xsl:with-param>
												<xsl:with-param name="text">Список форумов</xsl:with-param>
											</xsl:call-template>
											<xsl:text> | </xsl:text>
											<xsl:call-template name="headerLink">
												<xsl:with-param name="url">/Upload/</xsl:with-param>
												<xsl:with-param name="text">Аплоад</xsl:with-param>
												<xsl:with-param name="isDisabled">
													<xsl:if test="not(session/sessionKey)">true</xsl:if>
												</xsl:with-param>
											</xsl:call-template>
											<xsl:text> | </xsl:text>
											<xsl:call-template name="headerLink">
												<xsl:with-param name="url">/My/</xsl:with-param>
												<xsl:with-param name="text">My Home</xsl:with-param>
											</xsl:call-template>
											<xsl:text> | </xsl:text>
											<xsl:call-template name="headerLink">
												<xsl:with-param name="url">/q/faq/</xsl:with-param>
												<xsl:with-param name="text">FAQ</xsl:with-param>
											</xsl:call-template>
											<xsl:text> | </xsl:text>
											<xsl:call-template name="headerLink">
												<xsl:with-param name="url">/Users/</xsl:with-param>
												<xsl:with-param name="text">Пользователи</xsl:with-param>
											</xsl:call-template>
										</td>
										<td width="15%" align="right">
											<xsl:variable name="isLiteEnabled">
												<xsl:call-template name="isLiteEnabled"/>
											</xsl:variable>
											<xsl:variable name="isRssEnabled">
												<xsl:call-template name="isRssEnabled"/>
											</xsl:variable>
											<xsl:variable name="rssRelativeLink">
												<xsl:call-template name="rssRelativeLink"/>
											</xsl:variable>
											<xsl:variable name="prefix">
												<xsl:text>https://</xsl:text>
												<xsl:value-of select="url/host"/>
												<xsl:text>:</xsl:text>
												<xsl:if test="url/port &gt;= 1000">
													<xsl:value-of select="floor(url/port div 1000)"/>
												</xsl:if>
											</xsl:variable>
											<a>
												<xsl:if test="$isLiteEnabled='true'">
													<xsl:attribute name="href">
														<xsl:value-of select="$prefix"/>
														<xsl:text>447</xsl:text>
														<xsl:value-of select="currentUrl"/>
													</xsl:attribute>
												</xsl:if>
												<img border="0">
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
														<xsl:value-of select="$prefix"/>
														<xsl:text>449</xsl:text>
														<xsl:value-of select="$rssRelativeLink"/>
													</xsl:attribute>
												</xsl:if>
												<img border="0">
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
														<xsl:value-of select="$prefix"/>
														<xsl:text>449</xsl:text>
														<xsl:value-of select="$rssRelativeLink"/>
													</xsl:attribute>
												</link>
											</xsl:if>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<xsl:if test="starts-with(/root/currentUrl, '/Users/')">
			<xsl:call-template name="usersHeader"/>
		</xsl:if>
		<xsl:if test="starts-with(/root/currentUrl, '/My/')">
			<xsl:call-template name="myHeader"/>
		</xsl:if>
		<xsl:if test="starts-with(/root/currentUrl, '/Upload/')">
			<xsl:call-template name="uploadHeader"/>
		</xsl:if>
		<center>&#160;&#160;</center>
	</xsl:template>
</xsl:stylesheet>

