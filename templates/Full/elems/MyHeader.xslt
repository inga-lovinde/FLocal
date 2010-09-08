<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template name="myHeader">
		<table width="95%" align="center" class="tablesurround" cellpadding="1" cellspacing="1">
			<tr>
				<td>
					<table width="100%" class="tableborders" cellpadding="3" cellspacing="1">
						<tr>
							<td align="center" class="menubar">
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/My/Login/</xsl:with-param>
									<xsl:with-param name="text">Вход</xsl:with-param>
									<xsl:with-param name="isDisabled">
										<xsl:if test="session/sessionKey">true</xsl:if>
									</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/My/Conversations/</xsl:with-param>
									<xsl:with-param name="text">
										<xsl:choose>
											<xsl:when test="session/sessionKey and (session/indicators/unreadPrivateMessages != '0')">
												<b>
													<xsl:text>Личные сообщения (</xsl:text>
													<xsl:value-of select="session/indicators/unreadPrivateMessages"/>
													<xsl:text>)</xsl:text>
												</b>
											</xsl:when>
											<xsl:otherwise>
												<xsl:text>Личные сообщения</xsl:text>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:with-param>
									<xsl:with-param name="isDisabled">
										<xsl:if test="not(session/sessionKey)">true</xsl:if>
									</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/My/Settings/</xsl:with-param>
									<xsl:with-param name="text">Настройки</xsl:with-param>
									<xsl:with-param name="isDisabled">
										<xsl:if test="not(session/sessionKey)">true</xsl:if>
									</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/My/UserData/</xsl:with-param>
									<xsl:with-param name="text">Личные данные</xsl:with-param>
									<xsl:with-param name="isDisabled">
										<xsl:if test="not(session/sessionKey)">true</xsl:if>
									</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | &gt;&gt;</xsl:text>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/Users/User/<xsl:value-of select="session/user/id"/>/Info/</xsl:with-param>
									<xsl:with-param name="text">Профиль</xsl:with-param>
									<xsl:with-param name="isDisabled">
										<xsl:if test="not(session/sessionKey)">true</xsl:if>
									</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<a target="_top">
									<xsl:if test="session/sessionKey">
										<xsl:attribute name="href">/do/Logout/?sessionKey=<xsl:value-of select="session/sessionKey"/></xsl:attribute>
									</xsl:if>
									<xsl:text>Выход</xsl:text>
								</a>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</xsl:template>
</xsl:stylesheet>

