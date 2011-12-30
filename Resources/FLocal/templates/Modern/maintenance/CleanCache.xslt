<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="..\elems\Main.xslt"/>
	<xsl:template name="specificTitle">Очистка кэша</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>Очистка кэша</xsl:text>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td> 
								<xsl:text>Выберите таблицу и номера</xsl:text>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/Maintenance/CleanCache/">
									<xsl:text>Таблица</xsl:text><br /> 
									<select name="table" class="formboxes">
										<xsl:apply-templates select="tables/table"/>
									</select>
									<br/>
									<xsl:text>Начать с ID</xsl:text><br/> 
									<input type="text" name="start" class="formboxes" /><br/>
									<xsl:text>Количество</xsl:text><br/> 
									<input type="text" name="length" class="formboxes" value="1" /><br/>
									<input type="submit" name="buttlogin" value="Очистить!" class="buttons" /> 
								</form> 
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

	<xsl:template match="table">
		<option>
			<xsl:attribute name="value"><xsl:value-of select="current()"/></xsl:attribute>
			<xsl:value-of select="current()"/>
		</option>
	</xsl:template>

</xsl:stylesheet>