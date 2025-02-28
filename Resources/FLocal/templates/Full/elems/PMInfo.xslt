<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:import href="UserInfoBar.xslt"/>
	<xsl:template match="message">
		<tr>
			<td>
				<table width="100%" cellspacing="1" cellpadding="3" border="0">
					<tr>
						<td width="120" valign="top" class="darktable" rowspan="2">
							<a><xsl:attribute name="name">Message<xsl:value-of select="id"/></xsl:attribute></a>
							<xsl:apply-templates select="poster/account/user" mode="userInfoBar"/>
						</td>
						<td class="subjecttable">
							<xsl:if test="isReadByInterlocutor='false'">
								<xsl:attribute name="class">warning</xsl:attribute>
							</xsl:if>
							<table width="100%" border="0" cellpadding="0" cellspacing="0">
								<tr>
									<td align="left" width="65%" valign="top">
										<a target="_blank" class="separate">
											<img border="0" alt="" style="vertical-align: text-bottom">
												<xsl:choose>
													<xsl:when test="isRead='false'">
														<xsl:attribute name="src">/static/images/message-normal-notread.gif</xsl:attribute>
													</xsl:when>
													<xsl:otherwise>
														<xsl:attribute name="src">/static/images/message-normal-read.gif</xsl:attribute>
													</xsl:otherwise>
												</xsl:choose>
											</img>
										</a>
										<b class="separate"><xsl:value-of select="title"/></b>
										<xsl:if test="isRead='false'">
											<img alt="new" src="/static/images/new.gif" />
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
																	<xsl:attribute name="href">/My/Conversations/PMReply/<xsl:value-of select="id"/>/</xsl:attribute>
																	<xsl:apply-templates select="postDate/date" mode="navigationImageFor">
																		<xsl:with-param name="src">reply.gif</xsl:with-param>
																		<xsl:with-param name="alt">�����</xsl:with-param>
																	</xsl:apply-templates>
																</a>
															</td>
															<td class="navigation">
																<a>
																	<xsl:apply-templates select="postDate/date" mode="navigationImageFor">
																		<xsl:with-param name="src">edit.gif</xsl:with-param>
																		<xsl:with-param name="alt">������</xsl:with-param>
																	</xsl:apply-templates>
																</a>
															</td>
															<td class="navigation">
																<a target="_blank">
																	<xsl:apply-templates select="postDate/date" mode="navigationImageFor">
																		<xsl:with-param name="src">print.gif</xsl:with-param>
																		<xsl:with-param name="alt">������</xsl:with-param>
																	</xsl:apply-templates>
																</a>
															</td>
															<td class="navigation">
																<a>
																	<xsl:apply-templates select="postDate/date" mode="navigationImageFor">
																		<xsl:with-param name="src">email2.gif</xsl:with-param>
																		<xsl:with-param name="alt">�������� � ��</xsl:with-param>
																	</xsl:apply-templates>
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
											<br />
											<br />
										</font>
									</td>
								</tr>
								<xsl:if test="poster/account/user/signature != ''">
									<tr>
										<td>
											<div style="width:100%;max-height:50px;height: expression( this.scrollHeight > 49 ? '50px' : 'auto' );overflow:hidden">
												<font size="-2"><xsl:value-of select="poster/account/user/signature" disable-output-escaping="yes" /><br /></font>
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