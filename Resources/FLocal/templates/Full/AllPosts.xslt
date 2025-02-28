<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\PostInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:text>��� ���������</xsl:text>
	</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders"> 
						<tr class="darktable">
							<td>
								<table width="100%" cellpadding="0" cellspacing="0">
									<tr class="darktable">
										<td align="left" width="33%">
											<font class="catandforum">
												<xsl:text>��� ���������</xsl:text>
											</font>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<br />
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="0" cellspacing="1" width="100%" class="tableborders">
						<tr class="tdheader">
							<td>
								<table width="100%" cellspacing="1" cellpadding="3" border="0">
									<tr>
										<td>
											<xsl:text>��������:</xsl:text>
											<xsl:apply-templates select="posts/pageOuter" mode="withCurrent"/>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<xsl:apply-templates select="posts/post"/>
						<tr class="tdheader">
							<td>
								<table width="100%" cellspacing="1" cellpadding="3" border="0">
									<tr>
										<td>
											<xsl:text>��������:</xsl:text>
											<xsl:apply-templates select="posts/pageOuter" mode="withCurrent"/>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</xsl:template>

</xsl:stylesheet>