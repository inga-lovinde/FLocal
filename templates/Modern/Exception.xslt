<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle"><xsl:call-template name="Messages_Error"/></xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:call-template name="Messages_Error"/>
							</td> 
						</tr>
						<tr class="lighttable"> 
							<td>
								<xsl:value-of select="exception/message"/>
								<xsl:text> (</xsl:text>
								<xsl:value-of select="exception/type"/>
								<xsl:text>, ID </xsl:text>
								<xsl:value-of select="exception/guid"/>
								<xsl:text>)</xsl:text>
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

</xsl:stylesheet>