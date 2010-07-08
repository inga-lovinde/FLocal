<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\BoardInfo.xslt"/>
	<xsl:template name="specificTitle">Разделы</xsl:template>
	<xsl:template name="specific">
				<table width="95%" align="center" class="tablesurround" cellspacing="1" cellpadding="1">
					<tr>
						<td>
							<xsl:apply-templates select="categories/category"/>
						</td>
					</tr>
				</table>
				<br />
				<table width="95%" align="center" class="tablesurround" cellpadding="1" cellspacing="1">
					<tr>
						<td>
							<table width="100%" class="tableborders" cellpadding="3" cellspacing="1">
								<tr>
									<td colspan="3" class="tdheader">
										<b>Дополнительная информация</b>
									</td>
								</tr>
								<tr class="lighttable">
									<td width="45%" class="small" valign="top">
										<xsl:choose>
											<xsl:when test="session/user">
												<xsl:text>Вы вошли в форум как </xsl:text>
												<a>
													<xsl:attribute name="href">/User/<xsl:value-of select="session/user/id"/>/</xsl:attribute>
													<xsl:value-of select="session/user/name"/>
												</a>
											</xsl:when>
											<xsl:otherwise>
												<xsl:text>Вы не вошли в форум</xsl:text>
											</xsl:otherwise>
										</xsl:choose>
										<br />
										<xsl:value-of select="totalRegistered"/>
										<xsl:text> Зарегистрированных пользователей.</xsl:text>
										<br />
										<!--xsl:text>Приветствуем нового пользователя, </xsl:text>
										<a href="/showprofile.php?User=_PC&amp;What=ubbthreads">_PC</a>
										<br /-->
										<xsl:text>За последние </xsl:text>
										<xsl:value-of select="activity/threshold"/>
										<xsl:text> форум посещало </xsl:text>
										<xsl:value-of select="activity/sessions"/>
										<xsl:text> зарегистрированных пользователей.</xsl:text>
										<br />
										<a>Текущее время:</a><xsl:text> </xsl:text>
										<xsl:apply-templates select="currentDate" mode="dateTime"/>
									</td>
									<td width="30%" class="small" valign="top">
										<b>Просмотр новых сообщений</b>
										<br />
										<a>
											<xsl:attribute name="href">/AllPosts/</xsl:attribute>
											<xsl:text>Последние 7 дней</xsl:text>
										</a>
										<br />
										<a>
											<xsl:attribute name="href">/User/<xsl:value-of select="session/user/id"/>/Replies/</xsl:attribute>
											<xsl:text>Последние ответы на мои сообщения</xsl:text>
										</a>
										<br />
										<a>Мои сообщения с оценками</a>
										<br />
										<a>Рейтинги сообщений</a>
									</td>
									<td class="small" valign="top">
										<b>Легенда:</b>
										<br />
										<img src="/static/images/newposts.gif" alt="*" />
										<xsl:text>Новые сообщения</xsl:text>
										<br />
										<img src="/static/images/nonewposts.gif" alt="*" />
										<xsl:text>Нет новых сообщений</xsl:text>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
	</xsl:template>

	<xsl:template match="category">
		<table width="100%" align="center" class="tableborders" cellpadding="3" cellspacing="1">
			<tr>
				<td class="tdheader" colspan="2" width="61%"><xsl:value-of select="name"/></td>
				<td class="tdheader" align="center" width="7%">Темы</td>
				<td class="tdheader" align="center" width="7%">Сообщений</td>
				<td class="tdheader" align="center" width="15%">Последнее</td>
				<td class="tdheader" align="center" width="10%">Модератор</td>
			</tr>
			<xsl:apply-templates select="boards/board"/>
		</table>
	</xsl:template>

</xsl:stylesheet>