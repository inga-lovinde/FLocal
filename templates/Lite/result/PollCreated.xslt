<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="..\elems\Main.xslt"/>
	<xsl:template name="specificTitle">Опрос создан</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>Создание опроса</xsl:text>
							</td> 
						</tr>
						<tr> 
							<td class="lighttable"> 
								<xsl:text>Опрос успешно создан.</xsl:text>
								<br/>
								<xsl:text>Теперь вы можете вставлять ссылки на него с помощью тэга [poll]</xsl:text>
									<xsl:value-of select="poll/id"/>
								<xsl:text>[/poll]</xsl:text>
								<br/>
								<a>
									<xsl:attribute name="href">/Poll/<xsl:value-of select="poll/id"/>/</xsl:attribute>
									<xsl:text>Перейти к опросу</xsl:text>
								</a>
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

</xsl:stylesheet>