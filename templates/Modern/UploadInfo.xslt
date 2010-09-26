<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\UploadInfo.xslt"/>
	<xsl:template name="specificTitle">Аплоад</xsl:template>
	<xsl:template name="specific">
		<div id="uploadscontainer">
			<xsl:apply-templates select="upload"/>
		</div>
	</xsl:template>

</xsl:stylesheet>