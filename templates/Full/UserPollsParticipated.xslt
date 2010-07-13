<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\PollShortInfo.xslt"/>
	<xsl:template name="specificTitle">Опросы</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" border="0" class="tableborders">
						<tr class="darktable">
							<td colspan="6">
								<table width="100%" cellpadding="0" cellspacing="0">
									<tr>
										<td colspan="2" class="catandforum">
											<a>
												<xsl:attribute name="href">/Users/</xsl:attribute>
												<xsl:text>Пользователи</xsl:text>
											</a>
											<xsl:text> &gt;&gt; </xsl:text>
											<xsl:apply-templates select="user" mode="userLink"/>
											<xsl:text> &gt;&gt; </xsl:text>
											<xsl:text>Опросы с участием пользователя</xsl:text>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<br/>
		<table width="95%" align="center" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td align="left" nowrap="nowrap" width="75%" class="tdheader">Название</td>
							<td nowrap="nowrap" width="15%" class="tdheader" align="center">Автор</td>
							<td nowrap="nowrap" width="20%" class="tdheader" align="center">Дата</td>
						</tr>
						<!-- BEGIN POST LOOP DO NOT DELETE -->
						<xsl:apply-templates select="polls/poll"/>
						<!-- END OF LOOP -->
						<tr class="tdheader">
							<td colspan="3">
								<font class="onbody">
									<xsl:text>страницы:</xsl:text>
									<xsl:apply-templates select="polls/pageOuter" mode="withCurrent">
										<xsl:with-param name="baseLink">/User/<xsl:value-of select="user/id"/>/PollsParticipated/</xsl:with-param>
									</xsl:apply-templates>
								</font>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</xsl:template>

</xsl:stylesheet>