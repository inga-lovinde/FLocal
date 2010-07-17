<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template match="board">
		<p>
			<xsl:choose>
				<xsl:when test="(hasNewPosts='true') and (lastPostInfo/post/id &gt; 10000000)">
					<xsl:text>(*)</xsl:text>
				</xsl:when>
				<xsl:otherwise>
				</xsl:otherwise>
			</xsl:choose>
			<a>
				<xsl:attribute name="href">/Board/<xsl:value-of select="id"/>/</xsl:attribute>
				<xsl:value-of select="name"/>
			</a>
		</p>
	</xsl:template>

</xsl:stylesheet>