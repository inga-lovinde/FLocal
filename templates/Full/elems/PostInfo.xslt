<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template match="post">
		<tr>
			<td>
				<table width="100%" cellspacing="1" cellpadding="3" border="0">
					<tr>
						<td width="120" valign="top" class="darktable" rowspan="2">
							<a><xsl:attribute name="name">Post<xsl:value-of select="id"/></xsl:attribute></a>
							<table border="0" cellpadding="0" cellspacing="0">
								<tr>
									<td>
										<a>
											<xsl:attribute name="href">/User/<xsl:value-of select="poster/user/id"/>/</xsl:attribute>
											<xsl:value-of select="poster/user/name"/>
										</a>
									</td>
								</tr>
								<tr>
									<td class="small">
										<b><xsl:value-of select="poster/user/title"/></b>
									</td>
								</tr>
								<tr>
									<td class="small">
										<i></i> 
									</td>
								</tr>
								<tr>
									<td class="small">
										<i><font color="red"></font></i>
									</td>
								</tr>
								<tr>
									<td class="small">
										<img src="/user/7901.jpg" alt="" width="80" height="80" />
									</td>
								</tr>
								<tr>
									<td class="small">
										<xsl:text>Рег.: </xsl:text>
										<xsl:apply-templates select="poster/user/regDate/date" mode="date"/>
									</td>
								</tr>
								<tr>
									<td class="small">
										<xsl:text>Сообщений: </xsl:text>
										<xsl:value-of select="poster/user/totalPosts"/>
									</td>
								</tr>
								<tr>
									<td class="small">
										<xsl:text>Из: </xsl:text>
										<xsl:value-of select="poster/user/location"/>
									</td>
								</tr>
							</table>
						</td>
						<td class="subjecttable">
							<table width="100%" border="0" cellpadding="0" cellspacing="0">
								<tr>
									<td align="left" width="65%" valign="top">
										<a target="_blank" class="separate">
											<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/</xsl:attribute>
											<img border="0" src="/static/images/book-notread.gif"  alt="" style="vertical-align: text-bottom" />
										</a>
										<b class="separate"><xsl:value-of select="title"/></b>
										<img alt="new" src="/static/images/new.gif" />
										<xsl:if test="parentPost/post">
											<font class="small separate">
												<xsl:text>[</xsl:text>
												<a target="_blank">
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
																	<xsl:attribute name="href">/PostReply/<xsl:value-of select="id"/>/</xsl:attribute>
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
																	<img src="/static/images/addreminder.gif" border="0" alt="Добавить тему в напоминания!" title="Добавить тему в напоминания!" />
																</a>
															</td>
															<td class="navigation">
																<a>
																	<img src="/static/images/notifymod.gif" border="0" alt="Известить модератора" title="Известить модератора" />
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