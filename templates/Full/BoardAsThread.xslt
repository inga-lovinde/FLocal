<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\PostInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:value-of select="currentLocation/board/name"/>
	</xsl:template>
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
											<xsl:apply-templates select="currentLocation" mode="breadcrumbs"/>
										</td>
										<td align="right" valign="bottom">
											<table border="0" class="tablesurround">
												<tr>
													<td class="navigation" nowrap="nowrap">
														<!-- postoption is either newpost.gif or greynewpost.gif -->
														<a>
															<xsl:if test="session/sessionKey">
																<xsl:attribute name="href">/Board/<xsl:value-of select="currentLocation/board/id"/>/NewThread/</xsl:attribute>
															</xsl:if>
															<img src="/static/images/newpost.gif" alt="Новое сообщение" border="0" width="13" height="15" style="vertical-align: text-bottom" />
															<xsl:text>Сообщение</xsl:text>
														</a>
													</td>
													<td class="navigation" nowrap="nowrap">
														<form method="get" name="fullview">
															<select name="fullview" class="formboxes" style="font-size:10px">
																<option value="0">Норм</option>
																<option value="1">Офт</option>
																<option value="2" selected="selected">Мус</option>
															</select>
														</form>
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
													<td class="navigation">
														<a>
															<img src="/static/images/lb.gif" border="0" alt="Прочитать всё" style="vertical-align: text-bottom" />
														</a>
													</td>
													<td class="navigation">
														<a>
															<xsl:attribute name="href">/Board/<xsl:value-of select="currentLocation/board/id"/>/</xsl:attribute>
															<xsl:text>Как раздел</xsl:text>
														</a>
													</td>
													<td class="navigation">
														<a>
															<xsl:text>Как тред</xsl:text>
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
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="0" cellspacing="1" width="100%" class="tableborders">
						<tr class="tdheader">
							<td colspan="5">
								<font class="onbody">
									<xsl:text>страницы:</xsl:text>
									<xsl:apply-templates select="posts/pageOuter" mode="withCurrent">
										<xsl:with-param name="baseLink">/BoardAsThread/<xsl:value-of select="currentLocation/board/id"/>/</xsl:with-param>
									</xsl:apply-templates>
								</font>
							</td>
						</tr>
						<!-- BEGIN POST LOOP DO NOT DELETE -->
						<xsl:apply-templates select="posts/post"/>
						<!-- END OF LOOP -->
						<tr class="tdheader">
							<td colspan="5">
								<font class="onbody">
									<xsl:text>страницы:</xsl:text>
									<xsl:apply-templates select="posts/pageOuter" mode="withCurrent">
										<xsl:with-param name="baseLink">/BoardAsThread/<xsl:value-of select="currentLocation/board/id"/>/</xsl:with-param>
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