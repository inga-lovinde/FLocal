<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle"><xsl:value-of select="user/name"/></xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>Персональная информация о </xsl:text><xsl:value-of select="user/name"/>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr class="lighttable">
							<td>
								<table border="0" width="100%" class="lighttable" cellpadding="0">
									<tr>
										<td width="10%" class="darktable">
											<xsl:text>Email</xsl:text>
										</td>
										<td width="40%">
											<xsl:text>Not implemented</xsl:text>
										</td>
										<td align="right" valign="top" rowspan="11">
											<img alt="Picture" width="80" height="80">
												<xsl:choose>
													<xsl:when test="user/avatar">
														<xsl:attribute name="src">/Upload/Item/<xsl:value-of select="user/avatar"/>/</xsl:attribute>
													</xsl:when>
													<xsl:otherwise>
														<xsl:attribute name="src">/static/images/noavatar.gif</xsl:attribute>
													</xsl:otherwise>
												</xsl:choose>
											</img>
										</td>
									</tr>
									<tr>
										<td class="darktable">
											<xsl:text>Имя</xsl:text>
										</td>
										<td>
											<xsl:text>Not implemented</xsl:text>
										</td>
									</tr>
									<tr>
										<td class="darktable">
											<xsl:text>Титул</xsl:text>
										</td>
										<td>
											<xsl:value-of select="user/title"/>
										</td>
									</tr>
									<tr>
										<td class="darktable">
											<xsl:text>Итого сообщений</xsl:text>
										</td>
										<td>
											<xsl:value-of select="user/totalPosts"/>
										</td>
									</tr>
									<tr>
										<td class="darktable">
											<xsl:text>Рейтинг</xsl:text>
										</td>
										<td>
											<xsl:text>Not implemented</xsl:text>
										</td>
									</tr>
									<tr>
										<td class="darktable">
											<xsl:text>Домашняя страница</xsl:text>
										</td>
										<td>
											<xsl:text>Not implemented</xsl:text>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>Род занятий</xsl:text>
										</td>
										<td>
											<xsl:text>Not implemented</xsl:text>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>Хобби</xsl:text>
										</td>
										<td>
											<xsl:text>Not implemented</xsl:text>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>Расположение</xsl:text>
										</td>
										<td>
											<xsl:value-of select="user/location"/>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>Биография</xsl:text>
										</td>
										<td colspan="2">
											<xsl:value-of select="user/biography" disable-output-escaping="yes" />
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>Номер ICQ</xsl:text>
										</td>
										<td>
											<xsl:text>Not implemented</xsl:text>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>Пол (gender)</xsl:text>
										</td>
										<td>
											<xsl:text>Not implemented</xsl:text>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>Дата регистрации</xsl:text>
										</td>
										<td>
											<xsl:apply-templates select="user/regDate" mode="dateTime"/>
										</td>
									</tr>
									<tr>                                                          
										<td valign="top" class="darktable">                           
											<xsl:text>Последний раз онлайн</xsl:text>
										</td>                                                         
										<td>                                                          
											<xsl:apply-templates select="lastActivity" mode="dateTime"/>
										</td>                                                         
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>Подпись</xsl:text>
										</td>
										<td>
											<xsl:value-of select="user/signature" disable-output-escaping="yes" />
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>Симпатии</xsl:text>
										</td>
										<td title="Выразить симпатию этому человеку">
											<input type="submit" class="buttons" value=" " style="width:24px;background-image:url('/static/images/heart.gif');background-position:center left;background-repeat:no-repeat;"/>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>Penalties</xsl:text>
										</td>
										<td colspan="2">
											<table border="1" width="90%">
												<tr class="tdheader">
													<td width="4%">Вес</td>
													<td width="38%">Сообщение</td>
													<td width="10%">Модератор</td>
													<td width="38%">Комментарий</td>
													<td width="10%">Истекает</td>
												</tr>
												<xsl:apply-templates select="punishments/punishment"/>
											</table>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>Комментарий</xsl:text>
										</td>
										<td>
											<xsl:text>Not implemented</xsl:text>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</xsl:template>

	<xsl:template match="punishment">
		<tr>
			<td><xsl:value-of select="punishmentType/weightDescription"/></td>
			<td>
				<a>
					<xsl:attribute name="href">/Post/<xsl:value-of select="post/id"/>/</xsl:attribute>
					<xsl:value-of select="post/title"/>
				</a>
			</td>
			<td><xsl:apply-templates select="moderator" mode="userLink"/></td>
			<td><xsl:value-of select="comment"/></td>
			<td><xsl:apply-templates select="expires" mode="dateTime"/></td>
		</tr>
	</xsl:template>

</xsl:stylesheet>