<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle">Загрузка файла</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>Загрузка файлов</xsl:text>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td> 
								<xsl:text>Выберите файл для загрузки</xsl:text>
								<br/>
								<xsl:text>Максимальный размер файла &#8211; 1МБ, допустимые разрешения: gif, jpg, png, svg, jpe, jpeg, jfif, jif</xsl:text>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/Upload/" enctype="multipart/form-data">
									<input type="file" name="file" class="formboxes" /><br/>
									<input type="submit" name="buttlogin" value="Отправить!" class="buttons" /> 
								</form> 
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

</xsl:stylesheet>