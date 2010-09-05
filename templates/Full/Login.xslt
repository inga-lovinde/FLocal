<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\MyHeader.xslt"/>
	<xsl:template name="specificTitle">Вход</xsl:template>
	<xsl:template name="specific">
		<xsl:call-template name="myHeader"/>
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>Вход</xsl:text>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td> 
								<xsl:text>Введите ваше имя пользователя и пароль для регистрации в форуме.</xsl:text>
								<br/>
								<xsl:text>Если вы ещё не пользовались этим форумом, но пришли со старого форум.локала &#8211; вы можете создать пароль в форме миграции.</xsl:text>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/Login/">
									<xsl:text>Логин</xsl:text><br /> 
									<input type="text" name="name" class="formboxes" /><br/>
									<xsl:text>Пароль</xsl:text><br/> 
									<input type="password" name="password" class="formboxes" /><br/>
									<input type="submit" name="buttlogin" value="Войти!" class="buttons" /> 
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
								<xsl:text>Миграция</xsl:text>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td> 
								<xsl:text>Если вы пришли со старого форум.локала &#8211; введите свой логин.</xsl:text>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/My/Login/MigrateAccount/">
									<xsl:text>Логин</xsl:text><br /> 
									<input type="text" name="username" class="formboxes" /><br/>
									<input type="submit" name="buttlogin" value="Далее" class="buttons" /> 
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
								<xsl:text>Регистрация</xsl:text>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td>
								<xsl:choose>
									<xsl:when test="isLocalNetwork='false'">
										<xsl:text>Ваш IP </xsl:text>
										<xsl:value-of select="ip"/>
										<xsl:text> не входит в список разрешённых подсетей.</xsl:text>
										<br/>
										<xsl:text>Если вы считаете, что это ошибка, сообщите администратору свой IP-адрес и описание сети (общежитие/учебный корпус, университет и прочее).</xsl:text>
										<br/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:text>Ваш IP </xsl:text>
										<xsl:value-of select="ip"/>
										<xsl:text> входит в список разрешённых подсетей.</xsl:text>
										<br/>
									</xsl:otherwise>
								</xsl:choose>
								<a href="/LocalNetworks/">Список разрешённых подсетей</a>
							</td> 
						</tr>
						<xsl:if test="isLocalNetwork='true'">
							<tr> 
								<td class="lighttable"> 
									<form method="post" action="/do/Register/">
										<xsl:text>Имя пользователя</xsl:text><br/>
										<input type="text" name="login" class="formboxes" maxlength="16"/><br/>
										<xsl:text>Новый пароль</xsl:text><br /> 
										<input type="password" name="password" class="formboxes" /><br/>
										<xsl:text>Повторите пароль</xsl:text><br/> 
										<input type="password" name="password2" class="formboxes" /><br/>
										<xsl:text>e-mail для восстановления пароля (необязательно)</xsl:text><br/>
										<input type="text" name="registrationEmail" class="formboxes" />
										<br/>
										<input type="checkbox" name="constitution" value="constitution" id="constitution"/>
										<label for="constitution"> Да, я согласен/согласна/согласно с тем, что на этом форуме действует </label>
										<a href="/q/constitution/" style="text-decoration:underline">описанная по этой ссылке конституция</a>
										<xsl:text>.</xsl:text>
										<i> (обязательно)</i>
										<br/>
										<input type="checkbox" name="showPostsToAll" value="showPostsToAll" id="showPostsToAll"/>
										<label for="showPostsToAll"> Да, я согласен/согласна/согласно с тем, чтобы мои сообщения, размещённые на этом форуме, были доступны публично.</label>
										<i> (обязательно)</i>
										<br/>
										<input type="checkbox" name="law" value="law" id="law"/>
										<label for="law"> Да, я обязуюсь соблюдать законы Российской Федерации и Федеративной Республики Германия в общении на этом форуме.</label>
										<i> (обязательно)</i>
										<br/>
										<input type="submit" name="buttlogin" value="Зарегистрироваться!" class="buttons" /> 
									</form>
								</td> 
							</tr> 
						</xsl:if>
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

</xsl:stylesheet>