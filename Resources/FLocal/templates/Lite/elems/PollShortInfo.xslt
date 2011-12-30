<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template match="poll">
		<tr>
			<xsl:choose>
				<xsl:when test="position() mod 2 = 0">
					<xsl:attribute name="class">lighttable</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="class">darktable</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<td align="left">
				<a>
					<xsl:attribute name="href">
						<xsl:text>/Poll/</xsl:text>
						<xsl:value-of select="id"/>
						<xsl:text>/</xsl:text>
					</xsl:attribute>
					<xsl:value-of select="title"/>
				</a>
			</td>
		</tr>
	</xsl:template>


</xsl:stylesheet>