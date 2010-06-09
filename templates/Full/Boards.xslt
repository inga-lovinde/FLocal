<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:output method="xml" indent="yes" encoding="UTF-8"/>
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
										�� ����� � ����� ��� summersun
										<br />
										18983 ������������������ �������������.
										<br />
										������������ ������ ������������,
										<a href="/showprofile.php?User=_PC&amp;What=ubbthreads">_PC</a>
										<br />
										������ 222 ������������������ � 54 ��������� ������������� � �������.
										<br />
										<a href="/editdisplay.php?Cat=#offset">������� �����:</a>
										 08.06.2010 14:17, �������
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
										����� ���������
										<br />
										<img src="/static/images/nonewposts.gif" alt="*" />
										��� ����� ���������
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
				<td class="tdheader" colspan="2"><xsl:value-of select="name"/></td>
				<td class="tdheader" align="center">����</td>
				<td class="tdheader" align="center">���������</td>
				<td class="tdheader" align="center">���������</td>
				<td class="tdheader" align="center">���������</td>
			</tr>
			<xsl:apply-templates select="boards/board"/>
		</table>
	</xsl:template>

	<xsl:template match="board">
		<tr>
			<td valign="top" width="1%" class="darktable">
				<a href="/ubbthreads.php?Cat=&amp;C=&amp;check=Common&amp;src=">
					<xsl:attribute name="onClick">if (!confirm('�������� ��� ��������� ��� �����������?')) {event.returnValue=false; return false;}</xsl:attribute>
					<img border="0" width="17" height="21" src="/static/images/newposts.gif" alt=""/>
				</a>
			</td>
			<td width="55%" class="darktable">
				<font class="forumtitle">
					<a href="/postlist.php?Cat=&amp;Board=Common&amp;time=1275991384&amp;showlite="><xsl:value-of select="name"/></a>
				</font>
				<br />
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td class="forumdescript">
							<a href="/apostlist.php?Cat=&amp;Board=Common">A</a>&#160;&#160;
						</td>
						<td class="forumdescript"><xsl:value-of select="description"/></td>
					</tr>
				</table>
			</td>
			<td width="7%" align="center" class="threadtotal" nowrap="nowrap">373</td>
			<td width="7%" align="center" class="posttotal" nowrap="nowrap">128648</td>
			<td width="15%" nowrap="nowrap" class="posttime">
				08.06.2010 14:03<br />
				<a href="/showflat.php?Cat=&amp;Board=Common&amp;Number=9554129&amp;src=#Post9554129">�� igor</a>
			</td>
			<td width="10%" class="modcolumn" align="center">
				&#160;<a href="/showprofile.php?User=Sash&amp;What=ubbthreads">Sash</a>,
				<a href="/showprofile.php?User=DeadmoroZ&amp;What=ubbthreads">DeadmoroZ</a>
			</td>
		</tr>
	</xsl:template>
</xsl:stylesheet>