<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:call-template name="Messages_AvatarsSettings"/>
	</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:call-template name="Messages_AvatarsSettings"/>
							</td> 
						</tr>
						<tr>
							<td class="lighttable">
								<p>
									<xsl:call-template name="Messages_CurrentAvatars"/>
									<xsl:text>:</xsl:text>
								</p>
								<xsl:apply-templates select="avatars/upload"/>
							</td>
						</tr>
						<tr>
							<td class="lighttable">
								<form method="post" action="/do/Avatars/SetAsDefault">
									<input type="hidden" name="uploadId" value=""/>
									<input type="submit" class="buttons">
										<xsl:attribute name="value"><xsl:call-template name="Messages_DisableAvatar"/></xsl:attribute>
										<xsl:if test="not(/root/currentAvatar)">
											<xsl:attribute name="disabled">disabled</xsl:attribute>
										</xsl:if>
									</input>
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
								<xsl:call-template name="Messages_UploadNewAvatar"/>
							</td>
						</tr>
						<tr class="darktable">
							<td>
								<xsl:call-template name="Messages_MaxAvatarSize"/>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/Avatars/Add" enctype="multipart/form-data">
									<input type="file" name="file" class="formboxes" /><br/>
									<input type="submit" name="buttlogin" class="buttons"> 
										<xsl:attribute name="value"><xsl:call-template name="Messages_UploadButton"/></xsl:attribute>
									</input>
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
						<tr class="tdheader"> 
							<td> 
								<xsl:call-template name="Messages_NewAvatarFromUpload"/>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/Avatars/Add">
									<input type="text" name="uploadId"/><br/>
									<input type="submit" name="buttlogin" class="buttons">
										<xsl:attribute name="value"><xsl:call-template name="Messages_SubmitButton"/></xsl:attribute>
									</input>
								</form> 
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

	<xsl:template match="upload">
		<table border="0">
			<tr>
				<td valign="middle">
					<img alt="" class="avatar">
						<xsl:attribute name="src">/Upload/Item/<xsl:value-of select="id"/>/</xsl:attribute>
					</img>
				</td>
				<td valign="middle">
					<form method="post" action="/do/Avatars/SetAsDefault">
						<input type="hidden" name="uploadId">
							<xsl:attribute name="value"><xsl:value-of select="id"/></xsl:attribute>
						</input>
						<input type="submit" class="buttons">
							<xsl:attribute name="value"><xsl:call-template name="Messages_SetAvatarAsDefault"/></xsl:attribute>
							<xsl:if test="/root/currentAvatar/upload/id = id">
								<xsl:attribute name="disabled">disabled</xsl:attribute>
							</xsl:if>
						</input>
					</form><br/>
					<form method="post" action="/do/Avatars/Remove">
						<input type="hidden" name="uploadId">
							<xsl:attribute name="value"><xsl:value-of select="id"/></xsl:attribute>
						</input>
						<input type="submit" class="buttons">
							<xsl:attribute name="value"><xsl:call-template name="Messages_Remove"/></xsl:attribute>
							<xsl:if test="/root/currentAvatar/upload/id = id">
								<xsl:attribute name="disabled">disabled</xsl:attribute>
							</xsl:if>
						</input>
					</form>
				</td>
			</tr>
		</table>
	</xsl:template>

</xsl:stylesheet>