<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template name="uploadHeader">
		<table width="95%" align="center" class="tablesurround" cellpadding="1" cellspacing="1">
			<tr>
				<td>
					<table width="100%" class="tableborders" cellpadding="3" cellspacing="1">
						<tr>
							<td align="center" class="menubar">
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/Upload/List/</xsl:with-param>
									<xsl:with-param name="text">Список</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/Upload/New/</xsl:with-param>
									<xsl:with-param name="text">Загрузить новый</xsl:with-param>
								</xsl:call-template>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</xsl:template>
</xsl:stylesheet>

