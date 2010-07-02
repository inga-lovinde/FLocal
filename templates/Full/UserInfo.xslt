<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>������������ ���������� � </xsl:text><xsl:value-of select="user/name"/>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr class="lighttable">
							<td>
								<table border="0" width="100%" class="lighttable" cellpadding="0">
									<tr>
										<td width="10%" class="darktable">
											<xsl:text>Email</xsl:text>
										</td>
										<td width="40%">
											<xsl:text>Not implemented</xsl:text>
										</td>
										<td align="right" valign="top" rowspan="11">
											<img alt="Picture" width="80" height="80">
												<xsl:choose>
													<xsl:when test="user/avatar">
														<xsl:attribute name="src">/Upload/Item/<xsl:value-of select="user/avatar"/>/</xsl:attribute>
													</xsl:when>
													<xsl:otherwise>
														<xsl:attribute name="src">/static/images/noavatar.gif</xsl:attribute>
													</xsl:otherwise>
												</xsl:choose>
											</img>
										</td>
									</tr>
									<tr>
										<td class="darktable">
											<xsl:text>���</xsl:text>
										</td>
										<td>
											<xsl:text>Not implemented</xsl:text>
										</td>
									</tr>
									<tr>
										<td class="darktable">
											<xsl:text>�����</xsl:text>
										</td>
										<td>
											<xsl:value-of select="user/title"/>
										</td>
									</tr>
									<tr>
										<td class="darktable">
											<xsl:text>����� ���������</xsl:text>
										</td>
										<td>
											<xsl:value-of select="user/totalPosts"/>
										</td>
									</tr>
									<tr>
										<td class="darktable">
											<xsl:text>�������</xsl:text>
										</td>
										<td>
											<xsl:text>Not implemented</xsl:text>
										</td>
									</tr>
									<tr>
										<td class="darktable">
											<xsl:text>�������� ��������</xsl:text>
										</td>
										<td>
											<xsl:text>Not implemented</xsl:text>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>��� �������</xsl:text>
										</td>
										<td>
											<xsl:text>Not implemented</xsl:text>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>�����</xsl:text>
										</td>
										<td>
											<xsl:text>Not implemented</xsl:text>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>������������</xsl:text>
										</td>
										<td>
											<xsl:value-of select="user/location"/>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>���������</xsl:text>
										</td>
										<td colspan="2">
											<xsl:value-of select="user/biography"/>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>����� ICQ</xsl:text>
										</td>
										<td>
											<xsl:text>Not implemented</xsl:text>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>��� (gender)</xsl:text>
										</td>
										<td>
											<xsl:text>Not implemented</xsl:text>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>���� �����������</xsl:text>
										</td>
										<td>
											<xsl:apply-templates select="user/regDate" mode="dateTime"/>
										</td>
									</tr>
									<tr>                                                          
										<td valign="top" class="darktable">                           
											<xsl:text>��������� ��� ������</xsl:text>
										</td>                                                         
										<td>                                                          
											<xsl:text>Not implemented</xsl:text>
										</td>                                                         
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>�������</xsl:text>
										</td>
										<td>
											<xsl:value-of select="user/signature"/>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>��������</xsl:text>
										</td>
										<td title="�������� �������� ����� ��������">
											<input type="submit" class="buttons" value=" " style="width:24px;background-image:url('/static/images/heart.gif');background-position:center left;background-repeat:no-repeat;"/>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>Penalties</xsl:text>
										</td>
										<td colspan="2">
											<xsl:text>Not implemented</xsl:text>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>�����������</xsl:text>
										</td>
										<td>
											<xsl:text>Not implemented</xsl:text>
										</td>
									</tr>
									<tr>
										<td valign="top" colspan="3" align="center" class="darktable">
											<a>
												<xsl:if test="accountId">
													<xsl:attribute name="href">/PMSend/<xsl:value-of select="accountId"/>/</xsl:attribute>
												</xsl:if>
												<img src="/static/images/sendprivate.gif" border="0" style="vertical-align: text-bottom" />
												<xsl:text> ������� ������ ���������</xsl:text>
											</a>
											<xsl:text> | </xsl:text>
											<a>
												<xsl:if test="accountId">
													<xsl:attribute name="href">/Conversation/<xsl:value-of select="accountId"/>/</xsl:attribute>
												</xsl:if>
												<img src="/static/images/sundial.gif" border="0" style="vertical-align: text-bottom" />
												<xsl:text> �������</xsl:text>
											</a>
											<xsl:text> | </xsl:text>
											<a>
												<img src="/static/images/addtobook.gif" align="top" style="vertical-align: text-bottom" />
												<xsl:text> �������� � �������� �����</xsl:text>
											</a>
											<xsl:text> | </xsl:text>
											<a>
												<xsl:attribute name="href">/User/<xsl:value-of select="user/id"/>/Posts/</xsl:attribute>
												<img src="/static/images/viewposts.gif" align="top" style="vertical-align: text-bottom" />
												<xsl:text> �������� ��� ��������� ������������</xsl:text>
											</a>
											<xsl:text> | </xsl:text>
											<a>
												<img src="/static/images/graph.gif" align="top" border="0" style="vertical-align: text-bottom" />
												<xsl:text> ����������</xsl:text>
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
	</xsl:template>

</xsl:stylesheet>