<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:import href="UserInfoBar.xslt"/>
	<xsl:template match="post">
		<xsl:param name="isReplyDisabled">true</xsl:param>
		<tr>
			<td>
				<table width="100%" cellspacing="1" cellpadding="3" border="0">
					<tr>
						<td width="120" valign="top" class="darktable" rowspan="2">
							<a><xsl:attribute name="name">Post<xsl:value-of select="id"/></xsl:attribute></a>
							<xsl:apply-templates select="poster/user" mode="userInfoBar"/>
						</td>
						<td class="subjecttable">
							<table width="100%" border="0" cellpadding="0" cellspacing="0">
								<tr>
									<td align="left" width="65%" valign="top">
										<a target="_blank" class="separate">
											<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/</xsl:attribute>
											<img border="0" alt="" style="vertical-align: text-bottom">
												<xsl:choose>
													<xsl:when test="isUnread='true'">
														<xsl:attribute name="src">/static/images/message-<xsl:value-of select="layerName"/>-notread.gif</xsl:attribute>
													</xsl:when>
													<xsl:otherwise>
														<xsl:attribute name="src">/static/images/message-<xsl:value-of select="layerName"/>-read.gif</xsl:attribute>
													</xsl:otherwise>
												</xsl:choose>
											</img>
										</a>
										<b class="separate"><xsl:value-of select="title"/></b>
										<xsl:if test="isUnread='true'">
											<img alt="new" src="/static/images/new.gif" />
										</xsl:if>
										<xsl:if test="parentPost/post">
											<font class="small separate">
												<xsl:text>[</xsl:text>
												<a target="_blank">
													<xsl:attribute name="title"><xsl:value-of select="parentPost/post/bodyShort"/></xsl:attribute>
													<xsl:attribute name="href">/Post/<xsl:value-of select="parentPost/post/id"/>/</xsl:attribute>
													<xsl:text>re: </xsl:text>
													<xsl:value-of select="parentPost/post/poster/user/name"/>
												</a>
												<xsl:text>]</xsl:text>
											</font>
										</xsl:if>
										<br />
										<font class="small" style="padding-left:2em"><xsl:apply-templates select="postDate/date" mode="dateTime"/></font>
									</td>
									<td align="right" width="35%">
										<table cellpadding="0" cellspacing="0" border="0">
											<tr>
												<td align="right">
													<table class="tablesurround" border="0">
														<tr>
															<td class="navigation">
																<a>
																	<xsl:if test="$isReplyDisabled='false'">
																		<xsl:if test="/root/session/sessionKey">
																			<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/Reply/</xsl:attribute>
																		</xsl:if>
																	</xsl:if>
																	<img src="/static/images/reply.gif" border="0" alt="����� �� ���������" width="27" height="14" title="����� �� ���������" style="vertical-align: text-bottom" />
																</a>
															</td>
															<td class="navigation">
																<a>
																	<xsl:if test="isOwner='true'">
																		<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/Edit/</xsl:attribute>
																	</xsl:if>
																	<img src="/static/images/edit.gif" border="0" alt="������ ���������" title="������ ���������" width="21" height="14" style="vertical-align: text-bottom" />
																</a>
															</td>
															<td class="navigation">
																<a target="_blank">
																	<img src="/static/images/print.gif" border="0" alt="������ ���������" title="������ ���������" />
																</a>
															</td>
															<td class="navigation">
																<a>
																	<img src="/static/images/addreminder.gif" border="0" alt="�������� ���� � �����������!" title="�������� ���� � �����������!" />
																</a>
															</td>
															<td class="navigation">
																<a>
																	<img src="/static/images/notifymod.gif" border="0" alt="��������� ����������" title="��������� ����������" />
																</a>
															</td>
															<td class="navigation">
																<a>
																	<xsl:if test="/root/session/sessionKey">
																		<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/PMReply/</xsl:attribute>
																	</xsl:if>
																	<img src="/static/images/email2.gif" border="0" alt="�������� ��������" title="�������� ��������" />
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
					<tr>
						<td class="lighttable">
							<table width="100%" cellspacing="0" cellpadding="0" style="table-layout: fixed">
								<tr>
									<td>
										<br />
										<font class="post">
											<xsl:value-of select="body" disable-output-escaping="yes" />
											<!--<xsl:copy-of select="body"/>-->
											<br />
											<br />
										</font>
									</td>
								</tr>
								<xsl:if test="revision">
									<xsl:if test="(revision != '') and (revision != '0')">
										<tr>
											<td>
												<font size="-2">
													<xsl:text>��� ��������� ���� ��������������� ������������� </xsl:text>
													<xsl:value-of select="revision"/>
													<xsl:text> ���, ��������� ��� </xsl:text>
													<xsl:apply-templates select="lastChangeDate/date" mode="dateTime"/>
													<br />
													<br/>
												</font>
											</td>
										</tr>
									</xsl:if>
								</xsl:if>
								<xsl:if test="poster/user/signature != ''">
									<tr>
										<td>
											<div style="width:100%;max-height:50px;height: expression( this.scrollHeight > 49 ? '50px' : 'auto' );overflow:hidden">
												<font size="-2"><xsl:value-of select="poster/user/signature"/><br /></font>
											</div>
										</td>
									</tr>
								</xsl:if>
							</table>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</xsl:template>


</xsl:stylesheet>