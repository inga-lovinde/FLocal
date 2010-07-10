<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:template name="header">
		<table border="0" cellpadding="2" cellspacing="0" width="95%" class="tableborders" align="center">
			<tr>
				<td align="center">
					<table border="0" cellpadding="2" cellspacing="0" width="100%">
						<tr>
							<td width="100%" class="topmenu" align="center">
								<a href ="/" >Root</a>
								<xsl:text> | </xsl:text>
								<a href ="http://google.com/" target="_blank">Google</a>
								<xsl:text> | </xsl:text>
								<a href ="http://yandex.ru/" target="_blank">Yandex</a>
								<xsl:text> | </xsl:text>
								<a href ="http://mail.ru" target="_blank">Mail.ru</a>
								<xsl:text> | </xsl:text>
								<a href ="http://www.vedomosti.ru/" target="_blank">Vedomosti</a>
								<xsl:text> | </xsl:text>
								<a href ="http://www.afisha.ru/" target="_blank">Afisha</a>
								<xsl:text> | </xsl:text>
								<a href ="http://weather.yandex.ru/27612/" target="_blank">Weather</a>
								<xsl:text> | </xsl:text>
								<a target="_blank">LAN Support</a>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<table width="95%" align="center" class="tablesurround" cellpadding="1" cellspacing="1">
			<tr>
				<td>
					<table width="100%" class="tableborders" cellpadding="3" cellspacing="1">
						<tr>
							<td align="center" class="menubar">
								<a target="_top">
									<xsl:attribute name="href">/Boards/?<xsl:value-of select="timestamp"/></xsl:attribute>
									<xsl:text>������ �������</xsl:text>
								</a>
								<xsl:text> | </xsl:text>
								<a target="_top">
									<xsl:if test="session/sessionKey">
										<xsl:attribute name="href">/Conversations/?<xsl:value-of select="timestamp"/></xsl:attribute>
										<xsl:if test="session/indicators/unreadPrivateMessages != '0'">
											<img src="/static/images/newpm.gif" border="0">
												<xsl:attribute name="alt">
													<xsl:text>� ��� </xsl:text>
													<xsl:value-of select="session/indicators/unreadPrivateMessages"/>
													<xsl:text> ������������� ������ ���������</xsl:text>
												</xsl:attribute>
											</img>
										</xsl:if>
									</xsl:if>
									<xsl:text>������ ���������</xsl:text>
								</a>
								<xsl:text> | </xsl:text>
								<a target="_top">
									<xsl:if test="session/sessionKey">
										<xsl:attribute name="href">/Upload/List/?<xsl:value-of select="timestamp"/></xsl:attribute>
									</xsl:if>
									<xsl:text>������</xsl:text>
								</a>
								<xsl:text> | </xsl:text>
								<a target="_top">
									<xsl:if test="session/sessionKey">
										<xsl:attribute name="href">/Settings/</xsl:attribute>
									</xsl:if>
									<xsl:text>���������</xsl:text>
								</a>
								<xsl:text> | </xsl:text>
								<a target="_top">
									<xsl:if test="session/notLoggedIn">
										<xsl:attribute name="href">/Login/</xsl:attribute>
									</xsl:if>
									<xsl:text>����</xsl:text>
								</a>
								<xsl:text> | </xsl:text>
								<a target="_top">
									<xsl:attribute name="href">/Users/Online/?<xsl:value-of select="timestamp"/></xsl:attribute>
									<xsl:text>��� � �������</xsl:text>
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
									<xsl:text>�����</xsl:text>
								</a>
								<xsl:text> | </xsl:text>
								<a href="/Users/" target="_top">������������</a>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<center>&#160;&#160;</center>
	</xsl:template>
</xsl:stylesheet>

