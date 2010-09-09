<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle">Пользователи</xsl:template>
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
							<td align="left" width="15%" nowrap="nowrap">
								<a>
									<xsl:text>Последняя активность</xsl:text>
								</a>
							</td>
							<td align="left" width="70%" nowrap="nowrap">
								<a>
									<xsl:text>Последняя посещённая страница</xsl:text>
								</a>
							</td>
						</tr>
						<xsl:apply-templates select="users/user"/>
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
				<xsl:apply-templates select="lastActivity" mode="dateTime"/>
			</td>
			<td>
				<a>
					<xsl:attribute name="href"><xsl:value-of select="lastUrl"/></xsl:attribute>
					<xsl:value-of select="lastUrl"/>
				</a>
			</td>
		</tr>
	</xsl:template>

</xsl:stylesheet>