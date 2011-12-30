<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle">Настройки</xsl:template>
	<xsl:template name="specific">
		<script language="Javascript" type="text/javascript">
<xsl:text disable-output-escaping="yes"><![CDATA[
function changeSkin(newSkin) {
	var links = document.getElementsByTagName("link");
	for(i=0;i<links.length;i++) {
		var link = links[i];
		if((typeof(link.getAttribute) != "undefined") && (link.getAttribute("rel") == "stylesheet") && (link.getAttribute("skin") == "skin")) {
			var newLink = document.createElement("link");
			newLink.setAttribute("skin", "skin");
			newLink.setAttribute("rel", "stylesheet");
			newLink.setAttribute("type", "text/css");
			newLink.setAttribute("href", "/static/css/modern/" + newSkin + ".css");
			link.parentNode.replaceChild(newLink, link);
		}
	}
}

function changeMachichara(newMachichara) {
	document.getElementById("machicharaPreview").src = "/static/images/machichara/" + newMachichara + "-preview.gif";
}
]]></xsl:text>
		</script>
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
										<xsl:text>Максимальный размер картинки:</xsl:text>
										<br/>
										<input type="text" name="maxUploadImageWidth" size="4">
											<xsl:attribute name="value"><xsl:value-of select="settings/maxUploadImageWidth"/></xsl:attribute>
										</input>
										<xsl:text> x </xsl:text>
										<input type="text" name="maxUploadImageHeight" size="4">
											<xsl:attribute name="value"><xsl:value-of select="settings/maxUploadImageHeight"/></xsl:attribute>
										</input>
									</p>
									<p>
										<xsl:text>Цветовая схема:</xsl:text>
										<br/>
										<select name="modernSkinId">
											<xsl:attribute name="onChange">changeSkin(this.options[this.selectedIndex].innerText);</xsl:attribute>
											<xsl:apply-templates select="modernSkins/modernSkin" mode="skinOption">
												<xsl:with-param name="currentSkin"><xsl:value-of select="settings/modernSkinId"/></xsl:with-param>
											</xsl:apply-templates>
										</select>
									</p>
									<p>
										<xsl:text>Цветовая схема для классического интерфейса и лайта:</xsl:text>
										<br/>
										<select name="skinId">
											<xsl:apply-templates select="skins/skin" mode="skinOption">
												<xsl:with-param name="currentSkin"><xsl:value-of select="settings/skinId"/></xsl:with-param>
											</xsl:apply-templates>
										</select>
									</p>
									<p>
										<xsl:text>Machi-chara:</xsl:text>
										<br/>
										<select name="machicharaId">
											<xsl:attribute name="onChange">changeMachichara(this.options[this.selectedIndex].innerText);</xsl:attribute>
											<xsl:apply-templates select="machicharas/machichara" mode="machicharaOption">
												<xsl:with-param name="currentMachichara"><xsl:value-of select="settings/machicharaId"/></xsl:with-param>
											</xsl:apply-templates>
										</select>
										<br/>
										<img id="machicharaPreview" border="0">
											<xsl:attribute name="src">/static/images/machichara/<xsl:value-of select="machichara/name"/>-preview.gif</xsl:attribute>
										</img>
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

	<xsl:template match="modernSkin" mode="skinOption">
		<xsl:param name="currentSkin"/>
		<option>
			<xsl:attribute name="value"><xsl:value-of select="id"/></xsl:attribute>
			<xsl:if test="id=$currentSkin">
				<xsl:attribute name="selected">selected</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="name"/>
		</option>
	</xsl:template>

	<xsl:template match="machichara" mode="machicharaOption">
		<xsl:param name="currentMachichara"/>
		<option>
			<xsl:attribute name="value"><xsl:value-of select="id"/></xsl:attribute>
			<xsl:if test="id=$currentMachichara">
				<xsl:attribute name="selected">selected</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="name"/>
		</option>
	</xsl:template>

</xsl:stylesheet>