<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle">Локальные сети</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders"> 
						<tr class="darktable">
							<td>
								<table width="100%" cellpadding="0" cellspacing="0">
									<tr class="darktable">
										<td align="left" width="33%">
											<font class="catandforum">
												<xsl:text>Локальные сети</xsl:text>
											</font>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<br />
		<xsl:if test="localNetworks/localNetwork">
			<table width="95%" align="center" class="tableborders" border="1">
				<tr>
					<td class="tdheader" align="center" width="15%">Подсеть</td>
					<td class="tdheader" width="70%">Описание</td>
					<td class="tdheader" align="center" width="15%">Состояние</td>
				</tr>
				<xsl:apply-templates select="localNetworks/localNetwork"/>
			</table>
			<br/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="localNetwork">
		<tr>
			<xsl:choose>
				<xsl:when test="position() mod 2 = 0">
					<xsl:attribute name="class">lighttable</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="class">darktable</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<td align="center">
				<xsl:value-of select="network"/>
			</td>
			<td align="left">
				<xsl:value-of select="comment"/>
			</td>
			<td align="center">
				<xsl:choose>
					<xsl:when test="isEnabled='true'">
						<xsl:text>Включена</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>Отключена</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</td>
		</tr>
	</xsl:template>

</xsl:stylesheet>