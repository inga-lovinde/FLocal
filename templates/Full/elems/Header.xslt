<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:template name="header">
		<table width="95%" align="center" class="tablesurround" cellpadding="1" cellspacing="1">
			<tr>
				<td>
					<table width="100%" class="tableborders" cellpadding="3" cellspacing="1">
						<tr>
							<td align="center" class="menubar">
								<a target="_top">
									<xsl:attribute name="href">/Forum/Boards/?<xsl:value-of select="current/date/ticks"/></xsl:attribute>
									<xsl:text>Список форумов</xsl:text>
								</a>
								<xsl:text> | </xsl:text>
								<a target="_top">
									<xsl:if test="session/sessionKey">
										<xsl:attribute name="href">/My/Conversations/?<xsl:value-of select="current/date/ticks"/></xsl:attribute>
										<xsl:if test="session/indicators/unreadPrivateMessages != '0'">
											<img src="/static/images/newpm.gif" border="0">
												<xsl:attribute name="alt">
													<xsl:text>У вас </xsl:text>
													<xsl:value-of select="session/indicators/unreadPrivateMessages"/>
													<xsl:text> непрочитанных личных сообщений</xsl:text>
												</xsl:attribute>
											</img>
										</xsl:if>
									</xsl:if>
									<xsl:text>Личные сообщения</xsl:text>
								</a>
								<xsl:text> | </xsl:text>
								<a target="_top">
									<xsl:if test="session/sessionKey">
										<xsl:attribute name="href">/Upload/List/?<xsl:value-of select="current/date/ticks"/></xsl:attribute>
									</xsl:if>
									<xsl:text>Аплоад</xsl:text>
								</a>
								<xsl:text> | </xsl:text>
								<a target="_top">
									<xsl:if test="session/sessionKey">
										<xsl:attribute name="href">/My/Settings/</xsl:attribute>
									</xsl:if>
									<xsl:text>Настройки</xsl:text>
								</a>
								<xsl:text> | </xsl:text>
								<a target="_top">
									<xsl:if test="session/notLoggedIn">
										<xsl:attribute name="href">/My/Login/</xsl:attribute>
									</xsl:if>
									<xsl:text>Вход</xsl:text>
								</a>
								<xsl:text> | </xsl:text>
								<a target="_top">
									<xsl:attribute name="href">/Users/Online/?<xsl:value-of select="current/date/ticks"/></xsl:attribute>
									<xsl:text>Кто в онлайне</xsl:text>
								</a>
								<xsl:text> | </xsl:text>
								<a target="_top">
									<xsl:attribute name="href">/q/faq</xsl:attribute>
									<xsl:text>FAQ</xsl:text>
								</a>
								<xsl:text> | </xsl:text>
								<a target="_top">
									<xsl:if test="session/sessionKey">
										<xsl:attribute name="href">/do/Logout/?sessionKey=<xsl:value-of select="session/sessionKey"/></xsl:attribute>
									</xsl:if>
									<xsl:text>Выход</xsl:text>
								</a>
								<xsl:text> | </xsl:text>
								<a href="/Users/" target="_top">Пользователи</a>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<center>&#160;&#160;</center>
	</xsl:template>
</xsl:stylesheet>

