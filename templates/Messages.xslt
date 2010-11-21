<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:variable name="lang">ru</xsl:variable>

	<xsl:template name="Messages_AllPosts"><xsl:choose>
		<xsl:when test="$lang='ru'">��� ���������</xsl:when>
		<xsl:otherwise>All posts</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_AllThreads"><xsl:choose>
		<xsl:when test="$lang='ru'">��� ����</xsl:when>
		<xsl:otherwise>All threads</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_PageOuterTitle"><xsl:choose>
		<xsl:when test="$lang='ru'">��������</xsl:when>
		<xsl:otherwise>pages</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_AvatarsSettings"><xsl:choose>
		<xsl:when test="$lang='ru'">��������� ��������</xsl:when>
		<xsl:otherwise>Avatars settings</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_CurrentAvatars"><xsl:choose>
		<xsl:when test="$lang='ru'">����������� ��������</xsl:when>
		<xsl:otherwise>Current avatars</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_DisableAvatar"><xsl:choose>
		<xsl:when test="$lang='ru'">��������� ��������</xsl:when>
		<xsl:otherwise>Disable avatar</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_UploadNewAvatar"><xsl:choose>
		<xsl:when test="$lang='ru'">�������� �������� ��� ��������</xsl:when>
		<xsl:otherwise>Upload new avatar</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_MaxAvatarSize"><xsl:choose>
		<xsl:when test="$lang='ru'">������������ ������ ����� &#8211; 80��, ���������� ����������: gif, jpg, png, svg, jpe, jpeg, jfif, jif</xsl:when>
		<xsl:otherwise>File size should be less than 80KB; only gif, jpg, png, svg, jpe, jpeg, jfif, jif are allowed</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_UploadButton"><xsl:choose>
		<xsl:when test="$lang='ru'">���������!</xsl:when>
		<xsl:otherwise>Upload!</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_NewAvatarFromUpload"><xsl:choose>
		<xsl:when test="$lang='ru'">...��� ������� ����� ����� � �������</xsl:when>
		<xsl:otherwise>...or, alternatively, you may enter uploaded entry id</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_SubmitButton"><xsl:choose>
		<xsl:when test="$lang='ru'">���������!</xsl:when>
		<xsl:otherwise>Submit!</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_SetAvatarAsDefault"><xsl:choose>
		<xsl:when test="$lang='ru'">������� �����������</xsl:when>
		<xsl:otherwise>Make it my default avatar</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_Remove"><xsl:choose>
		<xsl:when test="$lang='ru'">�������</xsl:when>
		<xsl:otherwise>Remove</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_PageOuter"><xsl:choose>
		<xsl:when test="$lang='ru'">��������</xsl:when>
		<xsl:otherwise>pages</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_YouAreXXX">
		<xsl:param name="userLink"/>
		<xsl:choose>
			<xsl:when test="$lang='ru'">�� ����� � ����� ��� <xsl:copy-of select="$userLink"/></xsl:when>
			<xsl:otherwise>You are using <xsl:copy-of select="$userLink"/> username</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Messages_YouAreAnonymous"><xsl:choose>
		<xsl:when test="$lang='ru'">�� �� ����� � �����</xsl:when>
		<xsl:otherwise>You are not logged in</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_TotalRegisteredUsers">
		<xsl:param name="usersNumber"/>
		<xsl:choose>
			<xsl:when test="$lang='ru'"><xsl:value-of select="$usersNumber"/> ������������������ �������������</xsl:when>
			<xsl:otherwise>Total <xsl:value-of select="$usersNumber"/> users registered</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Messages_TotalOnlineUsers">
		<xsl:param name="threshold"/>
		<xsl:param name="sessionsNumber"/>
		<xsl:choose>
			<xsl:when test="$lang='ru'">�� ��������� <xsl:value-of select="$threshold"/> ����� ����� �������� <xsl:value-of select="$sessionsNumber"/> ������������������ �������������</xsl:when>
			<xsl:otherwise>There were <xsl:value-of select="$sessionsNumber"/> registered users active during last <xsl:value-of select="$threshold"/> minutes</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Messages_LastPosts"><xsl:choose>
		<xsl:when test="$lang='ru'">��������� ���������</xsl:when>
		<xsl:otherwise>Last posts</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_LastThreads"><xsl:choose>
		<xsl:when test="$lang='ru'">��������� ����</xsl:when>
		<xsl:otherwise>Last threads</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_LastReplies"><xsl:choose>
		<xsl:when test="$lang='ru'">��������� ������ �� ��� ���������</xsl:when>
		<xsl:otherwise>Last replies to my posts</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_LastPollsParticipated"><xsl:choose>
		<xsl:when test="$lang='ru'">��������� ������ � ���� ��������</xsl:when>
		<xsl:otherwise>Last polls i've participated in</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_Error"><xsl:choose>
		<xsl:when test="$lang='ru'">������</xsl:when>
		<xsl:otherwise>Error</xsl:otherwise>
	</xsl:choose></xsl:template>

</xsl:stylesheet>

