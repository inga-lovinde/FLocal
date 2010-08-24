<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle">�����������</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>����������� �� �������</xsl:text>
							</td> 
						</tr>
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/RegisterByInvite/">
									<input type="hidden" name="inviteId">
										<xsl:attribute name="value"><xsl:value-of select="invite/id"/></xsl:attribute>
									</input>
									<input type="hidden" name="code">
										<xsl:attribute name="value"><xsl:value-of select="invite/code"/></xsl:attribute>
									</input>
									<xsl:text>��� ������������</xsl:text><br/>
									<input type="text" name="login" class="formboxes" maxlength="16"/><br/>
									<xsl:text>����� ������</xsl:text><br /> 
									<input type="password" name="password" class="formboxes" /><br/>
									<xsl:text>��������� ������</xsl:text><br/> 
									<input type="password" name="password2" class="formboxes" /><br/>
									<br/>
									<input type="checkbox" name="constitution" value="constitution" id="constitution"/>
									<label for="constitution"> ��, � ��������/��������/�������� � ���, ��� �� ���� ������ ��������� </label>
									<a href="/q/constitution/" style="text-decoration:underline">��������� �� ���� ������ �����������</a>
									<xsl:text>.</xsl:text>
									<i> (�����������)</i>
									<br/>
									<input type="checkbox" name="showPostsToAll" value="showPostsToAll" id="showPostsToAll"/>
									<label for="showPostsToAll"> ��, � ��������/��������/�������� � ���, ����� ��� ���������, ����������� �� ���� ������, ���� �������� ��������.</label>
									<i> (�����������)</i>
									<br/>
									<input type="checkbox" name="law" value="law" id="law"/>
									<label for="law"> ��, � �������� ��������� ������ ���������� ��������� � ������������ ���������� �������� � ������� �� ���� ������.</label>
									<i> (�����������)</i>
									<br/>
									<input type="submit" name="buttlogin" value="������������������!" class="buttons" /> 
								</form>
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

</xsl:stylesheet>