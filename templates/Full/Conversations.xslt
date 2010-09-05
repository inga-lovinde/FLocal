<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\ConversationInfo.xslt"/>
	<xsl:template name="specificTitle">Личные сообщения</xsl:template>
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
											<xsl:text>Приватные сообщения</xsl:text>
										</td>
										<td align="right" valign="bottom">
											<table border="0" class="tablesurround">
												<tr>
													<td class="navigation" nowrap="nowrap">
														<!-- postoption is either newpost.gif or greynewpost.gif -->
														<a>
															<xsl:attribute name="href">/My/Conversations/PMSend/</xsl:attribute>
															<img src="/static/images/newpost.gif" alt="Новое сообщение" border="0" width="13" height="15" style="vertical-align: text-bottom" />
															<xsl:text>Сообщение</xsl:text>
														</a>
													</td>
													<td class="navigation" nowrap="nowrap">
														<!-- prevoption is either previous.gif or greyprevious.gif -->
														<a>
															<img alt="Предыдущая страница" border="0" width="12" height="15" style="vertical-align: text-bottom">
																<xsl:attribute name="src">/static/images/greyprevious.gif</xsl:attribute>
															</img>
															<xsl:text>Пред.</xsl:text>
														</a>
													</td>
													<td class="navigation" nowrap="nowrap">
														<a>
															<img src="/static/images/all.gif" alt="Список форумов" border="0" width="19" height="15" style="vertical-align: text-bottom" />
															<xsl:text>Список</xsl:text>
														</a>
													</td>
													<td class="navigation" nowrap="nowrap">
														<!-- nextoption is either next.gif or greynext.gif -->
														<a>
															<img alt="Следующая страница" border="0" width="14" height="15" style="vertical-align: text-bottom">
																<xsl:attribute name="src">/static/images/next.gif</xsl:attribute>
															</img>
															<xsl:text>След.</xsl:text>
														</a>
													</td>
												</tr>
											</table>
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
							<td align="left" nowrap="nowrap" width="75%" class="tdheader">Собеседник</td>
							<td nowrap="nowrap" width="5%" class="tdheader" align="center">Постов</td>
							<td nowrap="nowrap" width="20%" class="tdheader" align="center">Последнее</td>
						</tr>
						<!-- BEGIN POST LOOP DO NOT DELETE -->
						<xsl:apply-templates select="conversations/conversation"/>
						<!-- END OF LOOP -->
						<tr class="tdheader">
							<td colspan="3">
								<font class="onbody">
									<xsl:text>страницы:</xsl:text>
									<xsl:apply-templates select="conversations/pageOuter" mode="withCurrent">
										<xsl:with-param name="baseLink">/My/Conversations/</xsl:with-param>
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