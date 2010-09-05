<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle">Пользователи</xsl:template>
	<xsl:template name="specific">
		<xsl:call-template name="subHeader"/>
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr class="tdheader">
							<td align="left" width="15%" nowrap="nowrap">
								<a>
									<xsl:text>Пользователь</xsl:text>
								</a>
							</td>
							<td align="left" width="12%" nowrap="nowrap">
								<a>
									<xsl:text>Статус</xsl:text>
								</a>
							</td>
							<td align="left" width="28%" nowrap="nowrap">
								<a>
									<xsl:text>Домашняя страница</xsl:text>
								</a>
							</td>
							<td align="left" width="5%" nowrap="nowrap">
								<a>
									<xsl:text>Итого сообщений</xsl:text>
								</a>
							</td>
							<td align="left" width="12%" nowrap="nowrap">
								<a>
									<xsl:text>Номер ICQ</xsl:text>
								</a>
							</td>
							<td align="left" width="5%" nowrap="nowrap">
								<a>
									<xsl:text>Рейтинг</xsl:text>
								</a>
							</td>
							<td align="left" width="18%" nowrap="nowrap">
								<a>
									<xsl:text>Дата регистрации</xsl:text>
								</a>
							</td>
						</tr>
						<xsl:apply-templates select="users/user"/>
						<xsl:if test="users/pageOuter">
							<tr class="tdheader">
								<td colspan="7">
									<table width="100%" cellspacing="1" cellpadding="3" border="0">
										<tr>
											<td>
												<xsl:text>страницы:</xsl:text>
												<xsl:apply-templates select="users/pageOuter" mode="withCurrent">
													<xsl:with-param name="baseLink">/Users/All/</xsl:with-param>
												</xsl:apply-templates>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</xsl:if>
					</table>
				</td>
			</tr>
		</table>
	</xsl:template>

	<xsl:template match="users/user">
		<tr>
			<xsl:choose>
				<xsl:when test="position() mod 2 = 0">
					<xsl:attribute name="class">lighttable</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="class">darktable</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<td>
				<xsl:apply-templates select="current()" mode="userLink"/>
			</td>
			<td>
				<xsl:text>Пользователь</xsl:text>
			</td>
			<td>
				<xsl:text>TODO</xsl:text>
			</td>
			<td>
				<a>
					<xsl:attribute name="href">/User/<xsl:value-of select="id"/>/Posts/</xsl:attribute>
					<xsl:value-of select="totalPosts"/>
					<xsl:if test="actualPosts">
						<xsl:text> (</xsl:text>
						<xsl:value-of select="actualPosts"/>
						<xsl:text>)</xsl:text>
					</xsl:if>
				</a>
			</td>
			<td>
				<xsl:text>TODO</xsl:text>
			</td>
			<td>
				<xsl:text>TODO</xsl:text>
			</td>
			<td>
				<xsl:apply-templates select="regDate" mode="dateTime"/>
			</td>
		</tr>
	</xsl:template>

	<xsl:template name="subHeader">
		<table width="95%" align="center" class="tablesurround" cellpadding="1" cellspacing="1">
			<tr>
				<td>
					<table width="100%" class="tableborders" cellpadding="3" cellspacing="1">
						<tr>
							<td align="center" class="menubar">
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/Users/All/</xsl:with-param>
									<xsl:with-param name="text">Все</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/Users/Active/</xsl:with-param>
									<xsl:with-param name="text">Активные (смигрированные)</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/Users/Online/</xsl:with-param>
									<xsl:with-param name="text">В онлайне</xsl:with-param>
								</xsl:call-template>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<center>&#160;&#160;</center>
	</xsl:template>

</xsl:stylesheet>