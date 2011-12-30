<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle">Личные данные</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>Изменение личных данных</xsl:text>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td> 
								<xsl:text>???</xsl:text>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/UserData/">
									<p>
										<xsl:text>Титул</xsl:text>
										<br/>
										<input type="text" name="title">
											<xsl:attribute name="value"><xsl:value-of select="user/title"/></xsl:attribute>
										</input>
									</p>
									<p>
										<xsl:text>Из:</xsl:text>
										<br/>
										<input type="text" name="location">
											<xsl:attribute name="value"><xsl:value-of select="user/location"/></xsl:attribute>
										</input>
									</p>
									<p>
										<xsl:text>Биография:</xsl:text>
										<br/>
										<textarea cols="100" tabindex="2" rows="4" class="formboxes" name="biography">
											<xsl:choose>
												<xsl:when test="user/biographyUbb">
													<xsl:value-of select="user/biographyUbb"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:text> </xsl:text>
												</xsl:otherwise>
											</xsl:choose>
										</textarea>
									</p>
									<p>
										<xsl:text>Подпись:</xsl:text>
										<br/>
										<textarea cols="100" tabindex="2" rows="4" class="formboxes" name="signature">
											<xsl:choose>
												<xsl:when test="user/signatureUbb">
													<xsl:value-of select="user/signatureUbb"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:text> </xsl:text>
												</xsl:otherwise>
											</xsl:choose>
										</textarea>
									</p>
									<input type="submit" name="buttlogin" value="Отправить!" class="buttons" /> 
								</form> 
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

</xsl:stylesheet>