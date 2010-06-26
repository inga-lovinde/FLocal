﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FLocal.Core;
using FLocal.Common.dataobjects;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;
using System.Net;

namespace FLocal.Common {
	public static class UploadManager {

		private class _AlreadyUploadedException : FLocalException {

			public _AlreadyUploadedException() : base("Already uploaded") {
			}

		}

		public class AlreadyUploadedException : FLocalException {

			public readonly int uploadId;

			public AlreadyUploadedException(int uploadId) : base("Already uploaded " + uploadId) {
				this.uploadId = uploadId;
			}

		}

		private const int MAX_UPLOAD_FILESIZE = 1024*1024;

		private static HashSet<string> allowedExtensions = new HashSet<string>() {
			"jpg",
			"gif",
			"png",
		};

		public static Upload UploadFile(Stream fileStream, string filename, DateTime uploadDate, User uploader, int? id) {
			
			string extension = filename.Split('.').Last().ToLower();

			if(!allowedExtensions.Contains(extension)) throw new FLocalException("Unsupported extension");
			if(fileStream.Length > MAX_UPLOAD_FILESIZE) throw new FLocalException("File is too big");

			byte[] data = new byte[fileStream.Length];
			if(fileStream.Read(data, 0, (int)fileStream.Length) != fileStream.Length) {
				throw new FLocalException("File is incomplete");
			}

			string file_md5 = Util.md5(fileStream);
			AbstractCondition condition = new ComparisonCondition(
				Upload.TableSpec.instance.getColumnSpec(Upload.TableSpec.FIELD_HASH),
				ComparisonType.EQUAL,
				file_md5
			);

			int? uploadId = null;

			try {
				if(Config.instance.mainConnection.GetCountByConditions(Upload.TableSpec.instance, condition) > 0) {
					throw new _AlreadyUploadedException();
				}
				Config.Transactional(transaction => {
					Config.instance.mainConnection.lockTable(transaction, Upload.TableSpec.instance);
					/*if(Config.instance.mainConnection.GetCountByConditions(Upload.TableSpec.instance, condition, new JoinSpec[0]) > 0) {
						throw new _AlreadyUploadedException();
					}*/
					//TODO: ???

					HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Config.instance.UploaderUrl + "Upload/?extension=" + extension + "&signature=" + Util.md5(file_md5 + " " + Config.instance.SaltUploader));
					request.Method = "POST";
					request.ContentLength = data.Length;
					Stream requestStream = request.GetRequestStream();
					requestStream.Write(data, 0, data.Length);
					requestStream.Close();

					HttpWebResponse response;
					try {
						response = (HttpWebResponse)request.GetResponse();
					} catch(WebException e) {
						response = (HttpWebResponse)e.Response;
					}
					using(StreamReader reader = new StreamReader(response.GetResponseStream())) {
						string result = reader.ReadToEnd();
						if(result != "OK") {
							throw new CriticalException("Cannot upload file to upload service: " + result);
						}
					}

					Dictionary<string, string> row = new Dictionary<string,string>();
					if(id.HasValue) row[Upload.TableSpec.FIELD_ID] = id.ToString();
					row[Upload.TableSpec.FIELD_HASH] = file_md5;
					row[Upload.TableSpec.FIELD_EXTENSION] = extension;
					row[Upload.TableSpec.FIELD_UPLOADDATE] = uploadDate.ToUTCString();
					row[Upload.TableSpec.FIELD_USERID] = uploader.id.ToString();
					row[Upload.TableSpec.FIELD_SIZE] = data.Length.ToString();
					row[Upload.TableSpec.FIELD_FILENAME] = filename;
					uploadId = int.Parse(Config.instance.mainConnection.insert(transaction, Upload.TableSpec.instance, row));
				});
			} catch(_AlreadyUploadedException) {
				throw new AlreadyUploadedException(
					int.Parse(
						Config.instance.mainConnection.LoadIdsByConditions(
							Upload.TableSpec.instance,
							condition,
							Diapasone.unlimited,
							new JoinSpec[0]
						)[0]
					)
				);
			}
			return Upload.LoadById(uploadId.Value);
		}

	}
}
