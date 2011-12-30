<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" indent="no" />
	<xsl:template match="/root">
		<xsl:copy-of select="/"/>
	</xsl:template>
</xsl:stylesheet>

