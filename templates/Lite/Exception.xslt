<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle">Ошибка</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>Произошла ошибка</xsl:text>
							</td> 
						</tr>
						<tr class="lighttable"> 
							<td>
								<xsl:text>Тип ошибки: </xsl:text>
								<xsl:value-of select="exception/type"/>
								<br/>
								<xsl:text>Сообщение об ошибке: </xsl:text>
								<xsl:value-of select="exception/message"/>
								<br/>
								<br/>
								<font size="-3">
									<pre>
										<xsl:value-of select="exception/trace"/>
									</pre>
								</font>
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

</xsl:stylesheet>