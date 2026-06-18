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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#nullable enable

namespace FileGrab
{
	public class Utils
	{
        // https://stackoverflow.com/questions/6198392/check-whether-a-path-is-valid
        private static bool IsValidPath(string path, bool allowRelativePaths = false)
        {
            bool IsValid = true;

            try
            {
                string fullPath = Path.GetFullPath(path);

                if (allowRelativePaths)
                {
                    IsValid = Path.IsPathRooted(path);
                }
                else
                {
                    string? root = Path.GetPathRoot(path);
                    IsValid = string.IsNullOrEmpty(root?.Trim(new char[] { '\\', '/' })) == false;
                }
            }
            catch (Exception)
            {
                IsValid = false;
            }

            return IsValid;
        }

        public static void CopyFileTo(string src, string dst, bool allowRelativePaths = true, string? expr = null)
		{
			if (File.Exists(src) && (IsValidPath(src, allowRelativePaths) && IsValidPath(dst, allowRelativePaths)))
			{
                if (!string.IsNullOrEmpty(expr))
                {
                    Regex regex = new(expr, RegexOptions.IgnoreCase);

                    if (regex.IsMatch(Path.GetFileName(src)))
                    {
                        File.Copy(src, dst);
                    }
                }
                else
                {
                    File.Copy(src, dst);
                }
			}
			else
			{
				throw new Exception($" { src } and/or { dst } :: Is/Are Invalid PATH(S)!");
			}

		}
	}
}
