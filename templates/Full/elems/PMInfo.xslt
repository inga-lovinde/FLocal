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
							<table width="100%" border="0" cellpadding="0" cellspacing="0">
								<tr>
									<td align="left" width="65%" valign="top">
										<xsl:if test="isReadByInterlocutor='false'">
											<xsl:attribute name="class">warning</xsl:attribute>
										</xsl:if>
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
																	<xsl:attribute name="href">/PMReply/<xsl:value-of select="id"/>/</xsl:attribute>
																	<img src="/static/images/reply.gif" border="0" alt="Ответ на сообщение" width="27" height="14" title="Ответ на сообщение" style="vertical-align: text-bottom" />
																</a>
															</td>
															<td class="navigation">
																<a>
																	<img src="/static/images/edit.gif" border="0" alt="Правка сообщения" title="Правка сообщения" width="21" height="14" style="vertical-align: text-bottom" />
																</a>
															</td>
															<td class="navigation">
																<a target="_blank">
																	<img src="/static/images/print.gif" border="0" alt="Печать сообщения" title="Печать сообщения" />
																</a>
															</td>
															<td class="navigation">
																<a>
																	<img src="/static/images/email2.gif" border="0" alt="Ответить приватом" title="Ответить приватом" />
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
												<font size="-2"><xsl:value-of select="poster/account/user/signature"/><br /></font>
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