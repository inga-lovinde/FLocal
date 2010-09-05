<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\MyHeader.xslt"/>
	<xsl:template name="specificTitle">Настройки</xsl:template>
	<xsl:template name="specific">
		<xsl:call-template name="myHeader"/>
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>Изменение настроек</xsl:text>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td> 
								<xsl:text>???</xsl:text>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/Settings/">
									<p>
										<xsl:text>Ваш текущий пароль</xsl:text>
										<br/>
										<input type="password" name="currentPassword"/>
									</p>
									<p>
										<xsl:text>Если вы хотите изменить пароль, введите сюда новый пароль</xsl:text>
										<br/>
										<input type="password" name="newPassword"/>
										<br/>
										<xsl:text>и повторите его</xsl:text>
										<br/>
										<input type="password" name="newPassword2"/>
									</p>
									<p>
										<xsl:text>e-mail для восстановления пароля (необязательно)</xsl:text>
										<br/>
										<input type="text" name="registrationEmail"/>
									</p>
									<p>
										<xsl:text>Постов на страницу:</xsl:text>
										<br/>
										<input type="text" name="postsPerPage">
											<xsl:attribute name="value"><xsl:value-of select="settings/postsPerPage"/></xsl:attribute>
										</input>
									</p>
									<p>
										<xsl:text>Тредов на страницу:</xsl:text>
										<br/>
										<input type="text" name="threadsPerPage">
											<xsl:attribute name="value"><xsl:value-of select="settings/threadsPerPage"/></xsl:attribute>
										</input>
									</p>
									<p>
										<xsl:text>Пользователей на страницу:</xsl:text>
										<br/>
										<input type="text" name="usersPerPage">
											<xsl:attribute name="value"><xsl:value-of select="settings/usersPerPage"/></xsl:attribute>
										</input>
									</p>
									<p>
										<xsl:text>Картинок на страницу:</xsl:text>
										<br/>
										<input type="text" name="uploadsPerPage">
											<xsl:attribute name="value"><xsl:value-of select="settings/uploadsPerPage"/></xsl:attribute>
										</input>
									</p>
									<p>
										<xsl:text>Цветовая схема:</xsl:text>
										<br/>
										<select name="skinId">
											<xsl:apply-templates select="skins/skin" mode="skinOption">
												<xsl:with-param name="currentSkin"><xsl:value-of select="settings/skinId"/></xsl:with-param>
											</xsl:apply-templates>
										</select>
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

	<xsl:template match="skin" mode="skinOption">
		<xsl:param name="currentSkin"/>
		<option>
			<xsl:attribute name="value"><xsl:value-of select="id"/></xsl:attribute>
			<xsl:if test="id=$currentSkin">
				<xsl:attribute name="selected">selected</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="name"/>
		</option>
	</xsl:template>

</xsl:stylesheet>