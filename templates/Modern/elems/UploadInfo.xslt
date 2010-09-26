<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:import href="UserInfoBar.xslt"/>
	<xsl:template match="upload">
		<div class="uploadcontainer" style="clear:both">
			<a><xsl:attribute name="name">Upload<xsl:value-of select="id"/></xsl:attribute></a>
			<div class="userbarcontainer" style="float:left">
				<xsl:apply-templates select="uploader/user" mode="userInfoBar"/>
			</div>
			<div class="linkscontainer" style="float:right;text-align:right;">
				<xsl:comment>fill</xsl:comment>
			</div>
			<div class="uploadcontent">
				<xsl:variable name="uploadDate"><xsl:apply-templates select="uploadDate/date" mode="dateTime"/></xsl:variable>
				<a target="_blank" class="separate">
					<xsl:attribute name="href">/Upload/Info/<xsl:value-of select="id"/>/</xsl:attribute>
					<xsl:attribute name="title"><xsl:value-of select="$uploadDate"/></xsl:attribute>
					<img border="0" alt="" style="vertical-align: text-bottom" width="16" height="16">
						<xsl:attribute name="src">/static/images/woocons/Pictures.png</xsl:attribute>
					</img>
				</a>
				<xsl:text> </xsl:text>
				<xsl:value-of select="filename"/>
				<xsl:text>&#160;&#160;</xsl:text>
				<font style="font-size:0.6em">
					<xsl:text>(</xsl:text>
					<xsl:value-of select="$uploadDate"/>
					<xsl:text>)</xsl:text>
				</font>
				<div class="uploadbody">
					<img style="max-width:800px;max-height:600px;">
						<xsl:attribute name="src">/Upload/Item/<xsl:value-of select="id"/>/</xsl:attribute>
					</img>
					<br/>
					<br/>
					<xsl:text>Для вставки ссылки в форум используйте тэг [uploadLink]</xsl:text>
					<xsl:value-of select="id"/>
					<xsl:text>[/uploadLink]</xsl:text>
					<br/>
					<xsl:text>Для вставки изображения в форум используйте тэг [uploadImage]</xsl:text>
					<xsl:value-of select="id"/>
					<xsl:text>[/uploadImage]</xsl:text>
				</div>
				<xsl:if test="uploader/user/signature != ''">
					<div class="signature" style="float:right;">
						<xsl:value-of select="uploader/user/signature" disable-output-escaping="yes" />
					</div>
				</xsl:if>
			</div>
			<div style="clear:both;" class="zerosize">
				<xsl:comment>fill</xsl:comment>
			</div>
		</div>
<!--
		<tr>
			<td>
				<table width="100%" cellspacing="1" cellpadding="3" border="0">
					<tr>
						<td width="120" valign="top" class="darktable" rowspan="2">
							<xsl:apply-templates select="uploader/user" mode="userInfoBar"/>
						</td>
						<td class="subjecttable">
							<a target="_blank" class="separate">
								<xsl:attribute name="href">/Upload/Info/<xsl:value-of select="id"/>/</xsl:attribute>
								<img border="0" src="/static/images/woocons/Pictures.png"  alt="" style="vertical-align: text-bottom" />
							</a>
							<b class="separate"><xsl:value-of select="filename"/></b>
							<br />
							<font class="small" style="padding-left:2em"><xsl:apply-templates select="uploadDate/date" mode="dateTime"/></font>
						</td>
					</tr>
					<tr>
						<td class="lighttable">
							<table width="100%" cellspacing="0" cellpadding="0" style="table-layout: fixed">
								<tr>
									<td>
										<br />
										<font class="post">
											<img style="max-width:800px;max-height:600px;">
												<xsl:attribute name="src">/Upload/Item/<xsl:value-of select="id"/>/</xsl:attribute>
											</img>
											<br/>
											<br/>
											<xsl:text>Для вставки ссылки в форум используйте тэг [uploadLink]</xsl:text>
											<xsl:value-of select="id"/>
											<xsl:text>[/uploadLink]</xsl:text>
											<br/>
											<xsl:text>Для вставки изображения в форум используйте тэг [uploadImage]</xsl:text>
											<xsl:value-of select="id"/>
											<xsl:text>[/uploadImage]</xsl:text>
											<br/>
											<br/>
										</font>
									</td>
								</tr>
								<xsl:if test="uploader/user/signature != ''">
									<tr>
										<td>
											<div style="width:100%;max-height:50px;height: expression( this.scrollHeight > 49 ? '50px' : 'auto' );overflow:hidden">
												<font size="-2"><xsl:value-of select="uploader/user/signature" disable-output-escaping="yes" /><br /></font>
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
-->
	</xsl:template>

</xsl:stylesheet>