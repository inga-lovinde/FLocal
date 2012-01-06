<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template name="uploadHeader">
		<div class="header">
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/Upload/List/</xsl:with-param>
				<xsl:with-param name="text">Список</xsl:with-param>
			</xsl:call-template>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/Upload/New/</xsl:with-param>
				<xsl:with-param name="text">Новый</xsl:with-param>
			</xsl:call-template>
		</div>
	</xsl:template>
</xsl:stylesheet>

