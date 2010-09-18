<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="BoardHeader.xslt"/>

	<xsl:template name="forumHeader">
		<table width="95%" align="center" class="tablesurround" cellpadding="1" cellspacing="1">
			<tr>
				<td>
					<table width="100%" class="tableborders" cellpadding="3" cellspacing="1">
						<tr>
							<td align="center" class="menubar">
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/Forum/Boards/</xsl:with-param>
									<xsl:with-param name="text">Список разделов</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/Forum/AllThreads/</xsl:with-param>
									<xsl:with-param name="text">Последние темы</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/Forum/AllPosts/</xsl:with-param>
									<xsl:with-param name="text">Последние сообщения</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<img src="/static/images/shortcut.png" border="0"/>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/Users/User/<xsl:value-of select="session/user/id"/>/Replies/</xsl:with-param>
									<xsl:with-param name="text">Ответы мне</xsl:with-param>
									<xsl:with-param name="isDisabled">
										<xsl:if test="not(session/sessionKey)">true</xsl:if>
									</xsl:with-param>
								</xsl:call-template>
								<xsl:if test="currentLocation//board">
									<xsl:text> | </xsl:text>
									<xsl:call-template name="headerLink">
										<xsl:with-param name="url">/Forum/Board/<xsl:value-of select="currentLocation//board/id"/>/</xsl:with-param>
										<xsl:with-param name="text"><xsl:value-of select="currentLocation//board/name"/></xsl:with-param>
									</xsl:call-template>
								</xsl:if>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<xsl:if test="currentLocation/board">
			<xsl:call-template name="boardHeader"/>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>

