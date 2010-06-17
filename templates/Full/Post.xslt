<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\PostInfo.xslt"/>
	<xsl:template name="specific">
		<xsl:call-template name="threadInfo"/>
		<br />
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="0" cellspacing="1" width="100%" class="tableborders">
						<xsl:apply-templates select="posts/post"/>
					</table>
				</td>
			</tr>
		</table>
		<br />
		<xsl:call-template name="threadInfo"/>
	</xsl:template>

	<xsl:template name="threadInfo">
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
												<xsl:apply-templates select="currentLocation" mode="breadcrumbs"/>
											</font>
										</td>
										<td width="33%" align="right">
											<table border="0" class="tablesurround">
												<tr>
													<td class="navigation" nowrap="nowrap">
														<a>
															<xsl:attribute name="href">/Board/<xsl:value-of select="currentLocation/post/parent/thread/parent/board/id"/>/</xsl:attribute>
															<img src="/static/images/all.gif" alt="" border="0" width="19" height="15" style="vertical-align: text-bottom" />
															<xsl:text>Список</xsl:text>
														</a>
													</td>
													<td class="navigation" nowrap="nowrap">
														<form method="get" action="/showflat.php" name="fullview">
															<select name="fullview" class="formboxes" style="font-size:10px" onchange="document.forms.fullview.submit()">
																<option value="0" >Норм</option>
																<option value="1" >Офт</option>
																<option value="2" selected="selected">Мус</option>
															</select>
														</form>
													</td>
													<td class="navigation" nowrap="nowrap">
														<a>
															<xsl:attribute name="href">/Thread/<xsl:value-of select="currentLocation/post/parent/thread/id"/>/p<xsl:value-of select="currentLocation/post/id"/></xsl:attribute>
															<img alt="*" src="/static/images/flat.gif" style="vertical-align: text-bottom" />
															<xsl:text>Плоский</xsl:text>
														</a>
													</td>
													<td class="navigation" nowrap="nowrap">
														<a>
															<img src="/static/images/greythreaded.gif" border="0" style="vertical-align: text-bottom" alt="Дерево" />
															<xsl:text>Дерево</xsl:text>
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
	</xsl:template>

</xsl:stylesheet>