/*
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * Copyright (c) 2014, 2017, 2021 Fernando Mercês
 * Copyright (c) 2021 FallAngel1337
 *
 * This program is free software: you can redistribute it and/or modify it under
 * the terms of the GNU General Public License as published by the Free Software
 * Foundation, either version 3 of the License, or (at your option) any later
 * version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with
 * this program. If not, see <https://www.gnu.org/licenses/>.
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Windows.Forms;

#nullable enable

namespace FileGrab
{
	class FtpUpload
	{
        private readonly FtpWebRequest Ftp;

        private FtpUpload(Uri uri)
        {
            Ftp = (FtpWebRequest)WebRequest.Create(uri);
        }

        public static FtpUpload? Create(string host, int port = 21, string? file = null)
        {
            Uri ftpuri = new($"ftp://{ host }:{ port }/{ file }");

            if (ftpuri.Scheme != Uri.UriSchemeFtp
                || string.IsNullOrEmpty(ftpuri.Host)
                || (string.IsNullOrEmpty(ftpuri.Port.ToString()) || ftpuri.Port < 0))
            {
                return null;
            }

            return new(ftpuri);
        }

		public void Upload(string file)
		{
            Ftp.Method = WebRequestMethods.Ftp.UploadFile;
            Ftp.UseBinary = true;
            Ftp.UsePassive = true;

			try
			{
			    byte[] buffer = File.ReadAllBytes(file);
				Stream requestStream = Ftp.GetRequestStream();
				requestStream.Write(buffer, 0, buffer.Length);
				requestStream.Close();
                requestStream.Flush();
			}
			catch (Exception ex)
			{
				throw new Exception($"FTP ERROR :: { ex.Message }");
			}
		}

        public void UseCredentials(string? user, string? password, bool anon = false)
        {
            if (anon)
            {
                Ftp.Credentials = new NetworkCredential("anonymous", "anonymous@anonymous.net");
            }
            else
            {
                Ftp.Credentials = new NetworkCredential(user, password);
            }
        }
	}
}