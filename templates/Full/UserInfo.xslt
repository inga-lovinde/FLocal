<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle"><xsl:value-of select="user/name"/></xsl:template>
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
											<xsl:value-of select="user/biography" disable-output-escaping="yes" />
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
											<xsl:apply-templates select="lastActivity" mode="dateTime"/>
										</td>                                                         
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>�������</xsl:text>
										</td>
										<td>
											<xsl:value-of select="user/signature" disable-output-escaping="yes" />
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>Penalties</xsl:text>
										</td>
										<td colspan="2">
											<xsl:choose>
												<xsl:when test="punishments/punishment">
													<table border="1" width="90%">
														<tr class="tdheader">
															<td width="4%">���</td>
															<td width="10%">������</td>
															<td width="33%">���������</td>
															<td width="10%">���������</td>
															<td width="33%">�����������</td>
															<td width="10%">��������</td>
														</tr>
														<xsl:apply-templates select="punishments/punishment"/>
													</table>
												</xsl:when>
												<xsl:otherwise>
													<xsl:text>���</xsl:text>
												</xsl:otherwise>
											</xsl:choose>
										</td>
									</tr>
									<tr>
										<td valign="top" class="darktable">
											<xsl:text>Restrictions</xsl:text>
										</td>
										<td colspan="2">
											<xsl:choose>
												<xsl:when test="restrictions/restriction">
													<table border="1" width="90%">
														<tr class="tdheader">
															<td width="20%">������</td>
															<td width="80%">�����������</td>
														</tr>
														<xsl:apply-templates select="restrictions/restriction"/>
													</table>
												</xsl:when>
												<xsl:otherwise>
													<xsl:text>���</xsl:text>
												</xsl:otherwise>
											</xsl:choose>
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
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</xsl:template>

	<xsl:template match="punishment">
		<tr>
			<td><xsl:value-of select="punishmentType/weightDescription"/></td>
			<td>
				<a>
					<xsl:attribute name="href">/Board/<xsl:value-of select="originalBoard/board/id"/>/</xsl:attribute>
					<xsl:value-of select="originalBoard/board/name"/>
				</a>
			</td>
			<td>
				<a>
					<xsl:attribute name="href">/Post/<xsl:value-of select="post/id"/>/</xsl:attribute>
					<xsl:value-of select="post/title"/>
				</a>
			</td>
			<td><xsl:apply-templates select="moderator" mode="userLink"/></td>
			<td><xsl:value-of select="comment"/></td>
			<td><xsl:apply-templates select="expires" mode="dateTime"/></td>
		</tr>
	</xsl:template>

	<xsl:template match="restriction">
		<tr>
			<td>
				<a>
					<xsl:attribute name="href">/Board/<xsl:value-of select="board/id"/>/</xsl:attribute>
					<xsl:value-of select="board/name"/>
				</a>
			</td>
			<td>
				<xsl:apply-templates select="layers/layer"/>
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="restriction/layers/layer">
		<xsl:if test="position() &gt; 1"><br/></xsl:if>
		<xsl:text>��������� ������������� ���� </xsl:text>
		<xsl:value-of select="name"/>
		<xsl:text> �� </xsl:text>
		<xsl:apply-templates select="restrictionExpires" mode="dateTime"/>
	</xsl:template>

</xsl:stylesheet>