<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="..\elems\Main.xslt"/>
	<xsl:template name="specificTitle">Учётная запись смигрирована</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>Миграция пользователя</xsl:text>
							</td> 
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr> 
							<td class="lighttable"> 
								<p>Миграция была успешно завершена, теперь вы можете войти в форум, используя свой логин и новый пароль</p>
								<p align="center">
									<xsl:text>[&#8592;] </xsl:text>
									<a id="actionLink_left">
										<xsl:attribute name="href">/Boards/</xsl:attribute>
										<xsl:text>Вернуться к списку разделов</xsl:text>
									</a>
									<xsl:text> | </xsl:text>
									<a id="actionLink_right">
										<xsl:attribute name="href">/Login/</xsl:attribute>
										<xsl:text>Войти</xsl:text>
									</a>
									<xsl:text> [&#8594;]</xsl:text>
								</p>
								<script type="text/javascript" language="Javascript">
									<xsl:text>assignArrowsHandlers();</xsl:text>
								</script>
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

</xsl:stylesheet>