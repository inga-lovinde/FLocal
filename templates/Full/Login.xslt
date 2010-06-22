<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>����</xsl:text>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td> 
								<xsl:text>������� ���� ��� ������������ � ������ ��� ����������� � ������.</xsl:text>
								<br/>
								<xsl:text>���� �� ��� �� ������������ ���� �������, �� ������ �� ������� �����.������ &#150; �� ������ ������� ������ � ����� ��������.</xsl:text>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/Login/">
									<xsl:text>�����</xsl:text><br /> 
									<input type="text" name="name" class="formboxes" /><br/>
									<xsl:text>������</xsl:text><br/> 
									<input type="password" name="password" class="formboxes" /><br/>
									<input type="submit" name="buttlogin" value="�����!" class="buttons" /> 
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
								<xsl:text>��������</xsl:text>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td> 
								<xsl:text>���� �� ������ �� ������� �����.������ &#150; ������� ���� �����.</xsl:text>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/MigrateAccount/">
									<xsl:text>�����</xsl:text><br /> 
									<input type="text" name="username" class="formboxes" /><br/>
									<input type="submit" name="buttlogin" value="�����" class="buttons" /> 
								</form> 
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

</xsl:stylesheet>