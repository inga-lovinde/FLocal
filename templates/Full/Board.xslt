<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\BoardInfo.xslt"/>
	<xsl:import href="elems\ThreadInfo.xslt"/>
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
															<img src="/static/images/newpost.gif" alt="����� ���������" border="0" width="13" height="15" style="vertical-align: text-bottom" />
															<xsl:text>���������</xsl:text>
														</a>
													</td>
													<td class="navigation" nowrap="nowrap">
														<form method="get" action="/postlist.php" name="fullview">
															<select name="fullview" class="formboxes" style="font-size:10px" onchange="document.forms.fullview.submit()">
																<option value="0">����</option>
																<option value="1">���</option>
																<option value="2" selected="selected">���</option>
															</select>
														</form>
													</td>
													<td class="navigation" nowrap="nowrap">
														<!-- prevoption is either previous.gif or greyprevious.gif -->
														<a>
															<img alt="���������� ��������" border="0" width="12" height="15" style="vertical-align: text-bottom">
																<xsl:attribute name="src">/static/images/greyprevious.gif</xsl:attribute>
															</img>
															<xsl:text>����.</xsl:text>
														</a>
													</td>
													<td class="navigation" nowrap="nowrap">
														<a>
															<img src="/static/images/all.gif" alt="������ �������" border="0" width="19" height="15" style="vertical-align: text-bottom" />
															<xsl:text>������</xsl:text>
														</a>
													</td>
													<td class="navigation" nowrap="nowrap">
														<!-- nextoption is either next.gif or greynext.gif -->
														<a>
															<img alt="��������� ��������" border="0" width="14" height="15" style="vertical-align: text-bottom">
																<xsl:attribute name="src">/static/images/next.gif</xsl:attribute>
															</img>
															<xsl:text>����.</xsl:text>
														</a>
													</td>
													<td class="navigation">
														<a>
															<img src="/static/images/lb.gif" border="0" alt="��������� ��" style="vertical-align: text-bottom" />
														</a>
													</td>
													<td class="navigation">
														<a>
															<xsl:text>��� ������</xsl:text>
														</a>
													</td>
													<td class="navigation">
														<a>
															<xsl:attribute name="href">/BoardAsThread/<xsl:value-of select="currentLocation/board/id"/>/</xsl:attribute>
															<xsl:text>��� ����</xsl:text>
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
		<xsl:if test="boards/board">
			<table width="95%" align="center" class="tableborders" border="1">
				<tr>
					<td class="tdheader" colspan="2" width="61%">����������</td>
					<td class="tdheader" align="center" width="7%">����</td>
					<td class="tdheader" align="center" width="7%">���������</td>
					<td class="tdheader" align="center" width="15%">���������</td>
					<td class="tdheader" align="center" width="10%">���������</td>
				</tr>
				<xsl:apply-templates select="boards/board"/>
			</table>
			<br/>
		</xsl:if>
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
									<xsl:apply-templates select="threads/pageOuter" mode="withCurrent">
										<xsl:with-param name="baseLink">/Board/<xsl:value-of select="currentLocation/board/id"/>/</xsl:with-param>
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