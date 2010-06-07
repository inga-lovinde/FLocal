// Copyright (c) 2004-2008 MySQL AB, 2008-2009 Sun Microsystems, Inc.
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License version 2 as published by
// the Free Software Foundation
//
// There are special exceptions to the terms and conditions of the GPL 
// as it is applied to this software. View the full text of the 
// exception in file EXCEPTIONS in the directory of this software 
// distribution.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA 

namespace MySql.Data.MySqlClient
{
	/// <summary>
	/// Collection of error codes that can be returned by the server
	/// </summary>
	public class MySqlError
	{
		private string	level;
		private int		code;
		private string	message;

		/// <summary></summary>
		/// <param name="level"></param>
		/// <param name="code"></param>
		/// <param name="message"></param>
		public MySqlError(string level, int code, string message) 
		{
			this.level = level;
			this.code = code;
			this.message = message;
		}

		/// <summary>
		/// Error level
		/// </summary>
		public string Level 
		{
			get { return level; }
		}

		/// <summary>
		/// Error code
		/// </summary>
		public int Code 
		{
			get { return code; }
		}

		/// <summary>
		/// Error message
		/// </summary>
		public string Message 
		{
			get { return message; }
		}
	};

    /// <summary>
    /// Provides a reference to error codes returned by MySQL.
    /// </summary>
	public enum MySqlErrorCode 
	{
        None = 0,
/*		ER_HASHCHK=1000,
		ER_NISAMCHK=1001,
		ER_NO=1002,
		ER_YES 1003
		ER_CANT_CREATE_FILE 1004
		ER_CANT_CREATE_TABLE 1005
		ER_CANT_CREATE_DB 1006
		ER_DB_CREATE_EXISTS 1007
		ER_DB_DROP_EXISTS 1008
		ER_DB_DROP_DELETE 1009
		ER_DB_DROP_RMDIR 1010
		ER_CANT_DELETE_FILE 1011
		ER_CANT_FIND_SYSTEM_REC 1012
		ER_CANT_GET_STAT 1013
		ER_CANT_GET_WD 1014
		ER_CANT_LOCK 1015
		ER_CANT_OPEN_FILE 1016
		ER_FILE_NOT_FOUND 1017
		ER_CANT_READ_DIR 1018
		ER_CANT_SET_WD 1019
		ER_CHECKREAD 1020
		ER_DISK_FULL 1021
				*/
        /// <summary>
        /// There is already a key with the given values.
        /// </summary>
		DuplicateKey = 1022,

	/*		ER_ERROR_ON_CLOSE 1023
			ER_ERROR_ON_READ 1024
			ER_ERROR_ON_RENAME 1025
			ER_ERROR_ON_WRITE 1026
			ER_FILE_USED 1027
			ER_FILSORT_ABORT 1028
			ER_FORM_NOT_FOUND 1029
			ER_GET_ERRNO 1030
			ER_ILLEGAL_HA 1031*/
        /// <summary>
        /// The specified key was not found.
        /// </summary>
		KeyNotFound = 1032,
        /*		ER_NOT_FORM_FILE 1033
                ER_NOT_KEYFILE 1034
                ER_OLD_KEYFILE 1035
                ER_OPEN_AS_READONLY 1036
                ER_OUTOFMEMORY 1037
                ER_OUT_OF_SORTMEMORY 1038
                ER_UNEXPECTED_EOF 1039
                ER_CON_COUNT_ERROR 1040
                ER_OUT_OF_RESOURCES 1041*/
        /// <summary>
        /// Given when the connection is unable to successfully connect to host.
        /// </summary>
        UnableToConnectToHost = 1042,
/*                ER_HANDSHAKE_ERROR 1043
                ER_DBACCESS_DENIED_ERROR 1044*/
        /// <summary>
        /// Normally returned when an incorrect password is given
        /// </summary>
        AccessDenied = 1045,
            /*                ER_NO_DB_ERROR 1046
                            ER_UNKNOWN_COM_ERROR 1047
                            ER_BAD_NULL_ERROR 1048*/
        UnknownDatabase = 1049,
                            /*ER_BAD_DB_ERROR 1049
                            ER_TABLE_EXISTS_ERROR 1050
                            ER_BAD_TABLE_ERROR 1051
                            ER_NON_UNIQ_ERROR 1052
                            ER_SERVER_SHUTDOWN 1053
                            ER_BAD_FIELD_ERROR 1054
                            ER_WRONG_FIELD_WITH_GROUP 1055
                            ER_WRONG_GROUP_FIELD 1056
                            ER_WRONG_SUM_SELECT 1057
                            ER_WRONG_VALUE_COUNT 1058
                            ER_TOO_LONG_IDENT 1059
                            ER_DUP_FIELDNAME 1060*/
        /// <summary>
        /// Duplicate Key Name
        /// </summary>
		DuplicateKeyName = 1061,
        /// <summary>
        /// Duplicate Key Entry
        /// </summary>
		DuplicateKeyEntry = 1062,

	/*		ER_WRONG_FIELD_SPEC 1063
		ER_PARSE_ERROR 1064
		ER_EMPTY_QUERY 1065
		ER_NONUNIQ_TABLE 1066
		ER_INVALID_DEFAULT 1067
		ER_MULTIPLE_PRI_KEY 1068
		ER_TOO_MANY_KEYS 1069
		ER_TOO_MANY_KEY_PARTS 1070
		ER_TOO_LONG_KEY 1071
		ER_KEY_COLUMN_DOES_NOT_EXITS 1072
		ER_BLOB_USED_AS_KEY 1073
		ER_TOO_BIG_FIELDLENGTH 1074
		ER_WRONG_AUTO_KEY 1075
		ER_READY 1076
		ER_NORMAL_SHUTDOWN 1077
		ER_GOT_SIGNAL 1078
		ER_SHUTDOWN_COMPLETE 1079
		ER_FORCING_CLOSE 1080
		ER_IPSOCK_ERROR 1081
		ER_NO_SUCH_INDEX 1082
		ER_WRONG_FIELD_TERMINATORS 1083
		ER_BLOBS_AND_NO_TERMINATED 1084
		ER_TEXTFILE_NOT_READABLE 1085
		ER_FILE_EXISTS_ERROR 1086
		ER_LOAD_INFO 1087
		ER_ALTER_INFO 1088
		ER_WRONG_SUB_KEY 1089
		ER_CANT_REMOVE_ALL_FIELDS 1090
		ER_CANT_DROP_FIELD_OR_KEY 1091
		ER_INSERT_INFO 1092
		ER_INSERT_TABLE_USED 1093
		ER_NO_SUCH_THREAD 1094
		ER_KILL_DENIED_ERROR 1095
		ER_NO_TABLES_USED 1096
		ER_TOO_BIG_SET 1097
		ER_NO_UNIQUE_LOGFILE 1098
		ER_TABLE_NOT_LOCKED_FOR_WRITE 1099
		ER_TABLE_NOT_LOCKED 1100
		ER_BLOB_CANT_HAVE_DEFAULT 1101
		ER_WRONG_DB_NAME 1102
		ER_WRONG_TABLE_NAME 1103
		ER_TOO_BIG_SELECT 1104
		ER_UNKNOWN_ERROR 1105
		ER_UNKNOWN_PROCEDURE 1106
		ER_WRONG_PARAMCOUNT_TO_PROCEDURE 1107
		ER_WRONG_PARAMETERS_TO_PROCEDURE 1108
		ER_UNKNOWN_TABLE 1109
		ER_FIELD_SPECIFIED_TWICE 1110
		ER_INVALID_GROUP_FUNC_USE 1111
		ER_UNSUPPORTED_EXTENSION 1112
		ER_TABLE_MUST_HAVE_COLUMNS 1113
		ER_RECORD_FILE_FULL 1114
		ER_UNKNOWN_CHARACTER_SET 1115
		ER_TOO_MANY_TABLES 1116
		ER_TOO_MANY_FIELDS 1117
		ER_TOO_BIG_ROWSIZE 1118
		ER_STACK_OVERRUN 1119
		ER_WRONG_OUTER_JOIN 1120
		ER_NULL_COLUMN_IN_INDEX 1121
		ER_CANT_FIND_UDF 1122
		ER_CANT_INITIALIZE_UDF 1123
		ER_UDF_NO_PATHS 1124
		ER_UDF_EXISTS 1125
		ER_CANT_OPEN_LIBRARY 1126
		ER_CANT_FIND_DL_ENTRY 1127
		ER_FUNCTION_NOT_DEFINED 1128
		ER_HOST_IS_BLOCKED 1129
		*/
        /// <summary>
        /// The given host is not allowed to connect
        /// </summary>
		HostNotPrivileged = 1130,
        /// <summary>
        /// The anonymous user is not allowed to connect
        /// </summary>
		AnonymousUser = 1131,
        /// <summary>
        /// The given password is not allowed
        /// </summary>
		PasswordNotAllowed = 1132,
        /// <summary>
        /// The given password does not match
        /// </summary>
		PasswordNoMatch = 1133,
	/*		ER_UPDATE_INFO 1134
			ER_CANT_CREATE_THREAD 1135
			ER_WRONG_VALUE_COUNT_ON_ROW 1136
			ER_CANT_REOPEN_TABLE 1137
			ER_INVALID_USE_OF_NULL 1138
			ER_REGEXP_ERROR 1139
			ER_MIX_OF_GROUP_FUNC_AND_FIELDS 1140
			ER_NONEXISTING_GRANT 1141*/        TableAccessDenied = 1142,        ColumnAccessDenied = 1143,        IllegalGrantForTable = 1144,/*			ER_GRANT_WRONG_HOST_OR_USER 1145 */        NoSuchTable = 1146,
        NonExistingTableGrant = 1147,

/*			ER_NOT_ALLOWED_COMMAND 1148
			ER_SYNTAX_ERROR 1149
			ER_DELAYED_CANT_CHANGE_LOCK 1150
			ER_TOO_MANY_DELAYED_THREADS 1151
			ER_ABORTING_CONNECTION 1152
	*/		
        /// <summary>
        /// An attempt was made to send or receive a packet larger than
        /// max_allowed_packet_size
        /// </summary>
		PacketTooLarge=1153
	/*
			ER_NET_READ_ERROR_FROM_PIPE 1154
			ER_NET_FCNTL_ERROR 1155
			ER_NET_PACKETS_OUT_OF_ORDER 1156
			ER_NET_UNCOMPRESS_ERROR 1157
			ER_NET_READ_ERROR 1158
			ER_NET_READ_INTERRUPTED 1159
			ER_NET_ERROR_ON_WRITE 1160
			ER_NET_WRITE_INTERRUPTED 1161
			ER_TOO_LONG_STRING 1162
			ER_TABLE_CANT_HANDLE_BLOB 1163
			ER_TABLE_CANT_HANDLE_AUTO_INCREMENT 1164
			ER_DELAYED_INSERT_TABLE_LOCKED 1165
			ER_WRONG_COLUMN_NAME 1166
			ER_WRONG_KEY_COLUMN 1167
			ER_WRONG_MRG_TABLE 1168
			ER_DUP_UNIQUE 1169
			ER_BLOB_KEY_WITHOUT_LENGTH 1170
			ER_PRIMARY_CANT_HAVE_NULL 1171
			ER_TOO_MANY_ROWS 1172
			ER_REQUIRES_PRIMARY_KEY 1173
			ER_NO_RAID_COMPILED 1174
			ER_UPDATE_WITHOUT_KEY_IN_SAFE_MODE 1175
			ER_KEY_DOES_NOT_EXITS 1176
			ER_CHECK_NO_SUCH_TABLE 1177
			ER_CHECK_NOT_IMPLEMENTED 1178
			ER_CANT_DO_THIS_DURING_AN_TRANSACTION 1179
			ER_ERROR_DURING_COMMIT 1180
			ER_ERROR_DURING_ROLLBACK 1181
			ER_ERROR_DURING_FLUSH_LOGS 1182
			ER_ERROR_DURING_CHECKPOINT 1183
			ER_NEW_ABORTING_CONNECTION 1184
			ER_DUMP_NOT_IMPLEMENTED    1185
			ER_FLUSH_MASTER_BINLOG_CLOSED 1186
			ER_INDEX_REBUILD  1187
			ER_MASTER 1188
			ER_MASTER_NET_READ 1189
			ER_MASTER_NET_WRITE 1190
			ER_FT_MATCHING_KEY_NOT_FOUND 1191
			ER_LOCK_OR_ACTIVE_TRANSACTION 1192
			ER_UNKNOWN_SYSTEM_VARIABLE 1193
			ER_CRASHED_ON_USAGE 1194
			ER_CRASHED_ON_REPAIR 1195
			ER_WARNING_NOT_COMPLETE_ROLLBACK 1196
			ER_TRANS_CACHE_FULL 1197
			ER_SLAVE_MUST_STOP 1198
			ER_SLAVE_NOT_RUNNING 1199
			ER_BAD_SLAVE 1200
			ER_MASTER_INFO 1201
			ER_SLAVE_THREAD 1202
			ER_TOO_MANY_USER_CONNECTIONS 1203
			ER_SET_CONSTANTS_ONLY 1204
			ER_LOCK_WAIT_TIMEOUT 1205
			ER_LOCK_TABLE_FULL 1206
			ER_READ_ONLY_TRANSACTION 1207
			ER_DROP_DB_WITH_READ_LOCK 1208
			ER_CREATE_DB_WITH_READ_LOCK 1209
			ER_WRONG_ARGUMENTS 1210
			ER_NO_PERMISSION_TO_CREATE_USER 1211
			ER_UNION_TABLES_IN_DIFFERENT_DIR 1212
			ER_LOCK_DEADLOCK 1213
			ER_TABLE_CANT_HANDLE_FULLTEXT 1214
			ER_CANNOT_ADD_FOREIGN 1215
			ER_NO_REFERENCED_ROW 1216
			ER_ROW_IS_REFERENCED 1217
			ER_CONNECT_TO_MASTER 1218
			ER_QUERY_ON_MASTER 1219
			ER_ERROR_WHEN_EXECUTING_COMMAND 1220
			ER_WRONG_USAGE 1221
			ER_WRONG_NUMBER_OF_COLUMNS_IN_SELECT 1222
			ER_CANT_UPDATE_WITH_READLOCK 1223
			ER_MIXING_NOT_ALLOWED 1224
			ER_DUP_ARGUMENT 1225
			ER_USER_LIMIT_REACHED 1226
			ER_SPECIFIC_ACCESS_DENIED_ERROR 1227
			ER_LOCAL_VARIABLE 1228
			ER_GLOBAL_VARIABLE 1229
			ER_NO_DEFAULT 1230
			ER_WRONG_VALUE_FOR_VAR 1231
			ER_WRONG_TYPE_FOR_VAR 1232
			ER_VAR_CANT_BE_READ 1233
			ER_CANT_USE_OPTION_HERE 1234
			ER_NOT_SUPPORTED_YET	1235
			ER_MASTER_FATAL_ERROR_READING_BINLOG 1236
			ER_SLAVE_IGNORED_TABLE 1237 // only the slave SQL thread can be sent this 
			ER_ERROR_MESSAGES 238*/
	}
}
