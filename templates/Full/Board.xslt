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
														<img src="/static/images/newpost.gif" alt="����� ���������" border="0" width="13" height="15" style="vertical-align: text-bottom" />
														<a href="/newpost.php?Cat=&amp;Board=Common&amp;page=0&amp;src=&amp;sb=5&amp;o=&amp;showlite=">���������</a>
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
															<span>����.</span>
														</a>
													</td>
													<td class="navigation" nowrap="nowrap">
														<a href="/ubbthreads.php?Cat=&amp;C=1">
															<img src="/static/images/all.gif" alt="������ �������" border="0" width="19" height="15" style="vertical-align: text-bottom" />
															<span>������</span>
														</a>
													</td>
													<td class="navigation" nowrap="nowrap">
														<!-- nextoption is either next.gif or greynext.gif -->
														<a href="/postlist.php?Cat=&amp;Board=Common&amp;page=1&amp;fullview=&amp;src=&amp;sb=5&amp;o=&amp;showlite=">
															<img alt="��������� ��������" border="0" width="14" height="15" style="vertical-align: text-bottom">
																<xsl:attribute name="src">/static/images/next.gif</xsl:attribute>
															</img>
															<span>����.</span>
														</a>
													</td>
													<td class="navigation">
														<a href="/ubbthreads.php?check=Common&amp;src=&amp;showlite=">
															<img src="/static/images/lb.gif" border="0" alt="��������� ��" style="vertical-align: text-bottom" />
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
							<td nowrap="nowrap" width="5%" class="tdheader" align="center">���������</td>
							<td nowrap="nowrap" width="5%" class="tdheader" align="center">������</td>
							<td nowrap="nowrap" width="20%" class="tdheader" align="center">���������</td>
						</tr>
						<!-- BEGIN POST LOOP DO NOT DELETE -->
						<xsl:apply-templates select="threads/thread"/>
						<!-- END OF LOOP -->
						<tr class="tdheader">
							<td colspan="5">
								<font class="onbody">
									<span class="separate">��������:</span>
									<a class="separate">0</a>
									<a class="separate">
										<xsl:attribute name="href">/Board/1/p20/</xsl:attribute>
										<span>20</span>
									</a>
									<a class="separate">
										<xsl:attribute name="href">/Board/1/p40/</xsl:attribute>
										<span>40</span>
									</a>
									<a class="separate">
										<xsl:attribute name="href">/Board/1/p60/</xsl:attribute>
										<span>60</span>
									</a>
								</font>
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
					<table cellpadding="3" cellspacing="1" width="100%" border="0" class="tableborders">
						<tr class="tdheader">
							<td colspan="3">�������������� ����������</td>
						</tr>
						<tr class="lighttable">
							<td width="33%" class="small" valign="top">
								<span>11 ������������������ � 1 ��������� ������������� ������������� ���� �����.</span>
								<br />
								<br />
								<span>����������:</span>
								<a class="separate" href="/showprofile.php?User=Sash&amp;What=ubbthreads">Sash</a>
								<a class="separate" href="/showprofile.php?User=DeadmoroZ&amp;What=ubbthreads">DeadmoroZ</a>&#160;
								<br />
								<br />
							</td>
							<td valign="top" align="left" class="small" width="33%">
								<b>�����</b>
								<table style="margin-left:4em">
									<tr><td>�� ������ ��������� ����� ����</td></tr>
									<tr><td>�� ������ �������� �� ���������</td></tr>
									<tr><td>HTML ��������</td></tr>
									<tr><td>UBBCode �������</td></tr>
								</table>
							</td>
							<td class="small" valign="top">
								<b>�������:</b>
								<table style="margin-left:4em">
									<tr>
										<td><img src="/static/images/book-notread.gif" alt="" style="vertical-align: text-bottom" /></td>
										<td><span>����� ���������</span></td>
									</tr>
									<tr>
										<td><img src="/static/images/book-read.gif" alt="" style="vertical-align: text-bottom" /></td>
										<td><span>��� ����� ���������</span></td>
									</tr>
									<tr>
										<td><img src="/static/images/chat-notread.gif" alt="" style="vertical-align: text-bottom" /></td>
										<td><span>����� ���������</span></td>
									</tr>
									<tr>
										<td><img src="/static/images/chat-read.gif" alt="" style="vertical-align: text-bottom" /></td>
										<td><span>��� ����� ���������</span></td>
									</tr>
								</table>
							</td>
						</tr>
						<tr class="lighttable">
							<td align="left" width="33%" class="small">
								<form method="get" action="/dosearch.php">
									<span>����� � ������</span>
									<input type="text" name="Words" class="formboxes" style="font-size:10px" />
									<input type="submit" name="textsearch" value="�����" class="buttons" style="font-size:10px" />
								</form>
							</td>
							<td align="left" width="33%" class="small">
								<form method="post" action="/addfav.php?Cat=&amp;Board=Common&amp;type=board&amp;src=&amp;showlite=">
									<input type="submit" value="� ��������!" class="buttons iconize" style="background-image:url('/images/favorites.gif');" />
								</form>
								<form method="post" action="/togglesub.php?Cat=&amp;Board=Common&amp;page=0&amp;src=&amp;sb=5&amp;o=">
									<input type="submit" value="��������" class="buttons iconize" style="background-image:url('/images/hide.gif');" />
								</form>
								<form action="/changerss.php?Cat=&amp;rss_board_add=Common&amp;src=" method="post">
									<input type="submit" value="&#160;&#160;" class="buttons iconize" style="width:32px;background-image:url('/images/rss.png');" />
								</form>
							</td>
							<td align="left" class="small">
								<form method="post" action="/jumper.php">
									<span>������� �</span>
									<select name="board" class="formboxes" style="font-size:10px" >
										<option value ="-CATJUMP-1">-General-</option>
										<option value="Common" selected="selected">&#160;&#160;&#160;Common</option>
										<option value="current" >&#160;&#160;&#160;Current</option>
										<option value="University" >&#160;&#160;&#160;University</option>
									</select>
									<input type="submit" style="font-size:10px" name="Jump" value="�������" class="buttons" />
								</form>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</xsl:template>

</xsl:stylesheet>