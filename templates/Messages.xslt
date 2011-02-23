<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:variable name="lang">ru</xsl:variable>

	<xsl:template name="Messages_AllPosts"><xsl:choose>
		<xsl:when test="$lang='ru'">Все сообщения</xsl:when>
		<xsl:otherwise>All posts</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_AllThreads"><xsl:choose>
		<xsl:when test="$lang='ru'">Все темы</xsl:when>
		<xsl:otherwise>All threads</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_PageOuterTitle"><xsl:choose>
		<xsl:when test="$lang='ru'">страницы</xsl:when>
		<xsl:otherwise>pages</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_AvatarsSettings"><xsl:choose>
		<xsl:when test="$lang='ru'">Настройка аватарок</xsl:when>
		<xsl:otherwise>Avatars settings</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_CurrentAvatars"><xsl:choose>
		<xsl:when test="$lang='ru'">Загруженные аватарки</xsl:when>
		<xsl:otherwise>Current avatars</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_DisableAvatar"><xsl:choose>
		<xsl:when test="$lang='ru'">Отключить аватарку</xsl:when>
		<xsl:otherwise>Disable avatar</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_UploadNewAvatar"><xsl:choose>
		<xsl:when test="$lang='ru'">Выберите аватарку для загрузки</xsl:when>
		<xsl:otherwise>Upload new avatar</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_MaxAvatarSize"><xsl:choose>
		<xsl:when test="$lang='ru'">Максимальный размер файла &#8211; 80КБ, допустимые разрешения: gif, jpg, png, svg, jpe, jpeg, jfif, jif</xsl:when>
		<xsl:otherwise>File size should be less than 80KB; only gif, jpg, png, svg, jpe, jpeg, jfif, jif are allowed</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_UploadButton"><xsl:choose>
		<xsl:when test="$lang='ru'">Загрузить!</xsl:when>
		<xsl:otherwise>Upload!</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_NewAvatarFromUpload"><xsl:choose>
		<xsl:when test="$lang='ru'">...или укажите номер файла в аплоаде</xsl:when>
		<xsl:otherwise>...or, alternatively, you may enter uploaded entry id</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_SubmitButton"><xsl:choose>
		<xsl:when test="$lang='ru'">Отправить!</xsl:when>
		<xsl:otherwise>Submit!</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_SetAvatarAsDefault"><xsl:choose>
		<xsl:when test="$lang='ru'">Сделать стандартной</xsl:when>
		<xsl:otherwise>Make it my default avatar</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_Remove"><xsl:choose>
		<xsl:when test="$lang='ru'">Удалить</xsl:when>
		<xsl:otherwise>Remove</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_PageOuter"><xsl:choose>
		<xsl:when test="$lang='ru'">страницы</xsl:when>
		<xsl:otherwise>pages</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_YouAreXXX">
		<xsl:param name="userLink"/>
		<xsl:choose>
			<xsl:when test="$lang='ru'">Вы вошли в форум как <xsl:copy-of select="$userLink"/></xsl:when>
			<xsl:otherwise>You are using <xsl:copy-of select="$userLink"/> username</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Messages_YouAreAnonymous"><xsl:choose>
		<xsl:when test="$lang='ru'">Вы не вошли в форум</xsl:when>
		<xsl:otherwise>You are not logged in</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_TotalRegisteredUsers">
		<xsl:param name="usersNumber"/>
		<xsl:choose>
			<xsl:when test="$lang='ru'"><xsl:value-of select="$usersNumber"/> зарегистрированных пользователей</xsl:when>
			<xsl:otherwise>Total <xsl:value-of select="$usersNumber"/> users registered</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Messages_TotalOnlineUsers">
		<xsl:param name="threshold"/>
		<xsl:param name="sessionsNumber"/>
		<xsl:choose>
			<xsl:when test="$lang='ru'">За последние <xsl:value-of select="$threshold"/> минут форум посещало <xsl:value-of select="$sessionsNumber"/> зарегистрированных пользователей</xsl:when>
			<xsl:otherwise>There were <xsl:value-of select="$sessionsNumber"/> registered users active during last <xsl:value-of select="$threshold"/> minutes</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Messages_LastPosts"><xsl:choose>
		<xsl:when test="$lang='ru'">Последние сообщения</xsl:when>
		<xsl:otherwise>Last posts</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_LastThreads"><xsl:choose>
		<xsl:when test="$lang='ru'">Последние темы</xsl:when>
		<xsl:otherwise>Last threads</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_LastReplies"><xsl:choose>
		<xsl:when test="$lang='ru'">Последние ответы на мои сообщения</xsl:when>
		<xsl:otherwise>Last replies to my posts</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_LastPollsParticipated"><xsl:choose>
		<xsl:when test="$lang='ru'">Последние опросы с моим участием</xsl:when>
		<xsl:otherwise>Last polls i've participated in</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_Error"><xsl:choose>
		<xsl:when test="$lang='ru'">Ошибка</xsl:when>
		<xsl:otherwise>Error</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_Boards"><xsl:choose>
		<xsl:when test="$lang='ru'">Разделы</xsl:when>
		<xsl:otherwise>Boards</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_PrivateMessages"><xsl:choose>
		<xsl:when test="$lang='ru'">Личные сообщения</xsl:when>
		<xsl:otherwise>Private messages</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_LocalNetworks"><xsl:choose>
		<xsl:when test="$lang='ru'">Список разрешённых подсетей.</xsl:when>
		<xsl:otherwise>List of qualified subnets.</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_Subnet"><xsl:choose>
		<xsl:when test="$lang='ru'">Подсеть</xsl:when>
		<xsl:otherwise>Subnet</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_Description"><xsl:choose>
		<xsl:when test="$lang='ru'">Описание</xsl:when>
		<xsl:otherwise>Description</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_State"><xsl:choose>
		<xsl:when test="$lang='ru'">Состояние</xsl:when>
		<xsl:otherwise>State</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_Enabled"><xsl:choose>
		<xsl:when test="$lang='ru'">Включено</xsl:when>
		<xsl:otherwise>Enabled</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_Disabled"><xsl:choose>
		<xsl:when test="$lang='ru'">Отключено</xsl:when>
		<xsl:otherwise>Disabled</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_Login"><xsl:choose>
		<xsl:when test="$lang='ru'">Вход</xsl:when>
		<xsl:otherwise>Login</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_LoginText"><xsl:choose>
		<xsl:when test="$lang='ru'">Если вы уже зарегистрированы, введите ваши логин и пароль.</xsl:when>
		<xsl:otherwise>Registered users please enter your user name and password.</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_LoginMigrationText"><xsl:choose>
		<xsl:when test="$lang='ru'">Если вы ещё не пользовались этим форумом, но пришли со старого форум.локала &#8211; вы можете создать пароль в форме миграции.</xsl:when>
		<xsl:otherwise>Users of forum.local please use the migration form below if not already.</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_Username"><xsl:choose>
		<xsl:when test="$lang='ru'">Логин</xsl:when>
		<xsl:otherwise>User name</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_Password"><xsl:choose>
		<xsl:when test="$lang='ru'">Пароль</xsl:when>
		<xsl:otherwise>Password</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_NewPassword"><xsl:choose>
		<xsl:when test="$lang='ru'">Новый пароль</xsl:when>
		<xsl:otherwise>New password</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_NewPasswordRepeat"><xsl:choose>
		<xsl:when test="$lang='ru'">Повторите новый пароль</xsl:when>
		<xsl:otherwise>Repeat new password</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_AccountEmail"><xsl:choose>
		<xsl:when test="$lang='ru'">e-mail для восстановления пароля (необязательно)</xsl:when>
		<xsl:otherwise>e-mail</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_Migration">
		<xsl:param name="userName"/>
		<xsl:choose>
			<xsl:when test="$lang='ru'">Миграция<xsl:if test="$userName"> пользователя <xsl:value-of select="$userName"/></xsl:if></xsl:when>
			<xsl:otherwise>First-time migration<xsl:if test="$userName"> for user <xsl:value-of select="$userName"/></xsl:if></xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Messages_MigrationText"><xsl:choose>
		<xsl:when test="$lang='ru'">Если вы пришли со старого форум.локала &#8211; введите свой логин.</xsl:when>
		<xsl:otherwise>Users of forum.local please enter your forum.local user name.</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template match="user" mode="Messages_MigrationUserFound"><xsl:choose>
		<xsl:when test="$lang='ru'">Пользователь <xsl:apply-templates select="current()" mode="userLink"/> найден в списке пользователей старого форума.</xsl:when>
		<xsl:otherwise>User <xsl:apply-templates select="current()" mode="userLink"/> is found in the list of users of forum.local.</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_MigrationKeyInfo">
		<xsl:param name="key"/>
		<xsl:choose>
			<xsl:when test="$lang='ru'">Введите строку <b>(fhn:<xsl:value-of select="$key"/>)</b> (вместе со скобками) в поле биографии в своём профайле на старом форуме.</xsl:when>
			<xsl:otherwise>Write the key <b>(fhn:<xsl:value-of select="$key"/>)</b> (including the braces) in your `biography' field on forum.local.</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Messages_Registration"><xsl:choose>
		<xsl:when test="$lang='ru'">Регистрация</xsl:when>
		<xsl:otherwise>First-time registration</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_IpForbidden">
		<xsl:param name="ip"/>
		<xsl:choose>
			<xsl:when test="$lang='ru'">Ваш IP <xsl:value-of select="$ip"/> не входит в список разрешённых подсетей. Если вы считаете, что это ошибка, свяжитесь с администратором форума.</xsl:when>
			<xsl:otherwise>Registration from your IP <xsl:value-of select="$ip"/> is not allowed. If you believe this is a mistake, contact the forum administrator.</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Messages_IpAllowed">
		<xsl:param name="ip"/>
		<xsl:choose>
			<xsl:when test="$lang='ru'">Ваш IP <xsl:value-of select="$ip"/> входит в список разрешённых подсетей.</xsl:when>
			<xsl:otherwise>Registration from your IP <xsl:value-of select="$ip"/> is allowed.</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Messages_AcceptanceRequired"><xsl:choose>
		<xsl:when test="$lang='ru'">обязательно</xsl:when>
		<xsl:otherwise>required</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_AcceptConstitution"><xsl:choose>
		<xsl:when test="$lang='ru'">Да, я согласен/согласна/согласно с тем, что на этом форуме действует <a href="/q/constitution/" style="text-decoration:underline">описанная по этой ссылке конституция</a>.</xsl:when>
		<xsl:otherwise>I accept the <a href="/q/constitution">constitution described on this link</a>.</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_AcceptPostsVisibility"><xsl:choose>
		<xsl:when test="$lang='ru'">Да, я согласен/согласна/согласно с тем, чтобы мои сообщения, размещённые на этом форуме, были доступны публично.</xsl:when>
		<xsl:otherwise>I accept that all the messages hosted on this resource are to be available publicly.</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_AcceptLaws"><xsl:choose>
		<xsl:when test="$lang='ru'">Да, я обязуюсь соблюдать законы Российской Федерации и Федеративной Республики Германия в общении на этом форуме.</xsl:when>
		<xsl:otherwise>I swear not to violate state laws of the Russian Federation and of the Federal Republic of Germany.</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_NewPoll"><xsl:choose>
		<xsl:when test="$lang='ru'">Создание нового опроса</xsl:when>
		<xsl:otherwise>Create new poll</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template match="user" mode="Messages_CurrentUser"><xsl:choose>
		<xsl:when test="$lang='ru'">Пользователь: <xsl:apply-templates select="current()" mode="userLink"/></xsl:when>
		<xsl:otherwise>User: <xsl:apply-templates select="current()" mode="userLink"/></xsl:otherwise>
	</xsl:choose></xsl:template>
	
	<xsl:template match="user" mode="Messages_Receiver"><xsl:choose>
		<xsl:when test="$lang='ru'">Получатель: <xsl:apply-templates select="current()" mode="userLink"/></xsl:when>
		<xsl:otherwise>Receiver: <xsl:apply-templates select="current()" mode="userLink"/></xsl:otherwise>
	</xsl:choose></xsl:template>
	
	<xsl:template match="user" mode="Messages_Author"><xsl:choose>
		<xsl:when test="$lang='ru'">Автор: <xsl:apply-templates select="current()" mode="userLink"/></xsl:when>
		<xsl:otherwise>Author: <xsl:apply-templates select="current()" mode="userLink"/></xsl:otherwise>
	</xsl:choose></xsl:template>
	
	<xsl:template name="Messages_Title"><xsl:choose>
		<xsl:when test="$lang='ru'">Заголовок</xsl:when>
		<xsl:otherwise>Title</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_Layer"><xsl:choose>
		<xsl:when test="$lang='ru'">Слой</xsl:when>
		<xsl:otherwise>Layer</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_PollIsDetailed"><xsl:choose>
		<xsl:when test="$lang='ru'">Показывать имена проголосовавших</xsl:when>
		<xsl:otherwise>Show users voted</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_PollIsMultiOption"><xsl:choose>
		<xsl:when test="$lang='ru'">Разрешить выбор нескольких вариантов</xsl:when>
		<xsl:otherwise>Allow multiple chooses</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_NewThread">
		<xsl:param name="boardName"/>
		<xsl:choose>
			<xsl:when test="$lang='ru'">Создать новую тему в <xsl:value-of select="$boardName"/></xsl:when>
			<xsl:otherwise>Create new thread in <xsl:value-of select="$boardName"/></xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="Messages_NewPM">
		<xsl:param name="userName"/>
		<xsl:choose>
			<xsl:when test="$lang='ru'">Написать личное сообщение<xsl:if test="$userName!=''"> пользователю <xsl:value-of select="$userName"/></xsl:if></xsl:when>
			<xsl:otherwise>Send a private message<xsl:if test="$userName!=''"> to user <xsl:value-of select="$userName"/></xsl:if></xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="Messages_OriginalMessage"><xsl:choose>
		<xsl:when test="$lang='ru'">Исходное сообщение</xsl:when>
		<xsl:otherwise>Original message</xsl:otherwise>
	</xsl:choose></xsl:template>

</xsl:stylesheet>
