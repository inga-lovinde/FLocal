<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\BoardInfo.xslt"/>
	<xsl:template name="specific">
				<table width="95%" align="center" class="tablesurround" cellspacing="1" cellpadding="1">
					<tr>
						<td>
							<xsl:apply-templates select="categories/category"/>
						</td>
					</tr>
				</table>
				<br />
				<table width="95%" align="center" class="tablesurround" cellpadding="1" cellspacing="1">
					<tr>
						<td>
							<table width="100%" class="tableborders" cellpadding="3" cellspacing="1">
								<tr>
									<td colspan="3" class="tdheader">
										<b>�������������� ����������</b>
									</td>
								</tr>
								<tr class="lighttable">
									<td width="45%" class="small" valign="top">
										<xsl:choose>
											<xsl:when test="session/user">
												<xsl:text>�� ����� � ����� ��� </xsl:text><xsl:value-of select="session/user/name"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:text>�� �� ����� � �����</xsl:text>
											</xsl:otherwise>
										</xsl:choose>
										<br />
										<xsl:text>18983 ������������������ �������������.</xsl:text>
										<br />
										<xsl:text>������������ ������ ������������, </xsl:text>
										<a href="/showprofile.php?User=_PC&amp;What=ubbthreads">_PC</a>
										<br />
										<xsl:text>������ 222 ������������������ � 54 ��������� ������������� � �������.</xsl:text>
										<br />
										<a href="/editdisplay.php?Cat=#offset">������� �����:</a>
										<xsl:text> 08.06.2010 14:17, �������</xsl:text>
									</td>
									<td width="30%" class="small" valign="top">
										<b>�������� ����� ���������</b>
										<br />
										<a href="/dosearch.php?Cat=&amp;Forum=All_Forums&amp;Words=&amp;Match=Entire+Phrase&amp;Limit=25&amp;src=all">��������� 7 ����</a> 
										<br />
										<a href="/dosearch.php?Cat=&amp;Forum=All_Forums&amp;Uname=summersun&amp;search_replies=1&amp;Limit=25&amp;src=all">��������� ������ �� ��� ���������</a>
										<br />
										<a href="/dosearch.php?Cat=&amp;Forum=All_Forums&amp;Uname=summersun&amp;Limit=25&amp;src=all&amp;onlyrated=1">��� ��������� � ��������</a>
										<br />
										<a href="/toprated.php?showlite=">�������� ���������</a>
									</td>
									<td class="small" valign="top">
										<b>�������:</b>
										<br />
										<img src="/static/images/newposts.gif" alt="*" />
										<xsl:text>����� ���������</xsl:text>
										<br />
										<img src="/static/images/nonewposts.gif" alt="*" />
										<xsl:text>��� ����� ���������</xsl:text>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
	</xsl:template>

	<xsl:template match="category">
		<table width="100%" align="center" class="tableborders" cellpadding="3" cellspacing="1">
			<tr>
				<td class="tdheader" colspan="2" width="61%"><xsl:value-of select="name"/></td>
				<td class="tdheader" align="center" width="7%">����</td>
				<td class="tdheader" align="center" width="7%">���������</td>
				<td class="tdheader" align="center" width="15%">���������</td>
				<td class="tdheader" align="center" width="10%">���������</td>
			</tr>
			<xsl:apply-templates select="boards/board"/>
		</table>
	</xsl:template>

</xsl:stylesheet>