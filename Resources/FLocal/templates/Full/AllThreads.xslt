<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\ThreadInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:text>��� ����</xsl:text>
	</xsl:template>
	<xsl:template name="isRssEnabled">true</xsl:template>
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
												<xsl:text>��� ����</xsl:text>
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
		<table width="95%" align="center" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td align="left" width="55%" class="tdheader">����</td>
							<td align="left" nowrap="nowrap" width="15%" class="tdheader">�����</td>
							<td nowrap="nowrap" width="5%" class="tdheader" align="center">����������</td>
							<td nowrap="nowrap" width="5%" class="tdheader" align="center">������</td>
							<td nowrap="nowrap" width="20%" class="tdheader" align="center">���������</td>
						</tr>
						<!-- BEGIN POST LOOP DO NOT DELETE -->
						<xsl:apply-templates select="threads/thread"/>
						<!-- END OF LOOP -->
						<tr class="tdheader">
							<td colspan="5">
								<font class="onbody">
									<xsl:text>��������:</xsl:text>
									<xsl:apply-templates select="threads/pageOuter" mode="withCurrent"/>
								</font>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</xsl:template>

</xsl:stylesheet>