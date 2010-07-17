<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="..\elems\Main.xslt"/>
	<xsl:template name="specificTitle">Файл загружен</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>Загрузка файла</xsl:text>
							</td> 
						</tr>
						<tr> 
							<td class="lighttable"> 
								<xsl:text>Файл успешно загружен.</xsl:text>
								<br/>
								<xsl:text>Теперь вы можете вставлять ссылки на него с помощью тэга [uploadLink]</xsl:text>
									<xsl:value-of select="uploadedId"/>
								<xsl:text>[/uploadLink]</xsl:text>
								<br/>
								<xsl:text>Для вставки картинки воспользуйтесь тэгом [uploadImage]</xsl:text>
									<xsl:value-of select="uploadedId"/>
								<xsl:text>[/uploadImage]</xsl:text>
								<br/>
								<img>
									<xsl:attribute name="src">/Upload/Item/<xsl:value-of select="uploadedId"/>/</xsl:attribute>
								</img>
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

</xsl:stylesheet>