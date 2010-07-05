<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specific">
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
						<tr class="tdheader">
							<td colspan="7">
								<table width="100%" cellspacing="1" cellpadding="3" border="0">
									<tr>
										<td>
											<xsl:text>страницы:</xsl:text>
											<xsl:apply-templates select="users/pageOuter" mode="withCurrent">
												<xsl:with-param name="baseLink">/Users/</xsl:with-param>
											</xsl:apply-templates>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr class="tdheader">
							<td colspan="7">
								<table width="100%" cellspacing="1" cellpadding="3" border="0">
									<tr>
										<td>
											<a href="/Users/Active/">
												<xsl:text>Активные (смигрированные)</xsl:text>
											</a>
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
				<a>
					<xsl:attribute name="href">/User/<xsl:value-of select="id"/>/</xsl:attribute>
					<xsl:value-of select="name"/>
				</a>
			</td>
			<td>
				<xsl:text>Пользователь</xsl:text>
			</td>
			<td>
				<xsl:text>TODO</xsl:text>
			</td>
			<td>
				<xsl:value-of select="totalPosts"/>
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

</xsl:stylesheet>