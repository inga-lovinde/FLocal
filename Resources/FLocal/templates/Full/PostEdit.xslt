<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\TextEditor.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:text>��������� ��������� - </xsl:text>
		<xsl:value-of select="post/title"/>
	</xsl:template>
	<xsl:template name="isLiteEnabled">true</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>��������� ��������� (</xsl:text>
								<xsl:value-of select="board/name"/>
								<xsl:text>)</xsl:text>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td> 
								<xsl:text>��������� ����������� ���� ����� ��� ��������� ���������</xsl:text>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/Edit/" name="replier">
									<input type="hidden" name="postId">
										<xsl:attribute name="value"><xsl:value-of select="post/id"/></xsl:attribute>
									</input>
									<xsl:text>������������: </xsl:text>
									<xsl:value-of select="session/user/name"/>
									<br/>
									<br/>
									<xsl:text>����: </xsl:text>
									<br/>
									<input type="text" tabindex="1" name="title" maxlength="70" class="formboxes" size="60">
										<xsl:attribute name="value">
											<xsl:choose>
												<xsl:when test="newTitle and (newTitle != '')">
													<xsl:value-of select="newTitle"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="revision/title"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
									</input>
									<span class="small">���� ���������:</span> 
									<select class="formboxes" name="layerId">
										<xsl:apply-templates select="layers">
											<xsl:with-param name="defaultLayerId"><xsl:value-of select="post/layerId"/></xsl:with-param>
										</xsl:apply-templates>
									</select>
									<br/>
									<br/>
									<xsl:call-template name="textEditor">
										<xsl:with-param name="body"><xsl:value-of select="revision/body"/></xsl:with-param>
									</xsl:call-template>
									<input type="submit" tabindex="3" name="textcont" taborder="2" value="����������" class="buttons"/>
								</form>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>

		<br/>

		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>����� �� ���������</xsl:text>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td>
								<b>
									<xsl:text>�����: </xsl:text>
									<xsl:value-of select="post/poster/user/name"/>
									<br/>
									<xsl:text>����: </xsl:text>
									<xsl:value-of select="post/title"/>
								</b>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable">
								<xsl:value-of select="post/body" disable-output-escaping="yes"/>
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

</xsl:stylesheet>